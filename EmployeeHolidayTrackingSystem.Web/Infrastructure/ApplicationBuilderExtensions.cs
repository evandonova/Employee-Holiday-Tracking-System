using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;

using static EmployeeHolidayTrackingSystem.Web.Constants;
using static EmployeeHolidayTrackingSystem.Web.Areas.Employees.EmployeeConstants;
using static EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.SupervisorConstants;

namespace EmployeeHolidayTrackingSystem.Web.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        private static User employeeUser = null!;
        private static User supervisorUser = null!;
        private static Supervisor supervisor = null!;

        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);
            SeedDatabase(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<EmployeeHolidayDbContext>();
            data.Database.Migrate();
        }

        private static void SeedDatabase(IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<EmployeeHolidayDbContext>();

            SeedHolidayStatuses(dbContext);

            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            SeedAdminRoleAndUser(userManager, roleManager);
            SeedSupervisorRoleAndUser(userManager, roleManager);
            SeedEmployeeRoleAndUser(userManager, roleManager);

            if(supervisorUser is not null)
            {
                SeedSupervisor(dbContext);
            }

            if(employeeUser is not null)
            {
                SeedEmployee(dbContext);
            }
        }

        private static void SeedHolidayStatuses(EmployeeHolidayDbContext dbContext)
        {
            if (dbContext.HolidayRequestStatuses.Any())
            {
                return;
            }

            var holidayRequestStatuses = new List<HolidayRequestStatus>()
            {
                new HolidayRequestStatus()
                {
                    Title = RequestStatusEnum.Pending.ToString()
                },
                new HolidayRequestStatus()
                {
                    Title = RequestStatusEnum.Approved.ToString()
                },
                new HolidayRequestStatus()
                {
                    Title = RequestStatusEnum.Disapproved.ToString()
                }
            };

            Task
               .Run(async () =>
               {
                   await dbContext.HolidayRequestStatuses.AddRangeAsync(holidayRequestStatuses);
                   await dbContext.SaveChangesAsync();
               })
               .GetAwaiter()
               .GetResult();
        }

        private static void SeedAdminRoleAndUser(UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdminRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdminRoleName };

                    await roleManager.CreateAsync(role);

                    var adminUser = new User()
                    {
                        UserName = "admin@mail.com",
                        NormalizedUserName = "ADMIN@MAIL.COM",
                        Email = "admin@mail.com",
                        NormalizedEmail = "ADMIN@MAIL.COM",
                        FirstName = "Admin",
                        LastName = "Harrison"
                    };

                    await userManager.CreateAsync(adminUser, "admin123#");

                    await userManager.AddToRoleAsync(adminUser, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedSupervisorRoleAndUser(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(SupervisorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = SupervisorRoleName };

                    await roleManager.CreateAsync(role);

                    supervisorUser = new User()
                    {
                        UserName = "supervisor@mail.com",
                        NormalizedUserName = "SUPERVISOR@MAIL.COM",
                        Email = "supervisor@mail.com",
                        NormalizedEmail = "SUPERVISOR@MAIL.COM",
                        FirstName = "Supervisor",
                        LastName = "Jefferson"
                    };

                    await userManager.CreateAsync(supervisorUser, "supervisor123#");
                    await userManager.AddToRoleAsync(supervisorUser, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedEmployeeRoleAndUser(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(EmployeeRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = EmployeeRoleName };

                    await roleManager.CreateAsync(role);

                    employeeUser = new User()
                    {
                        UserName = "employee@mail.com",
                        NormalizedUserName = "EMPLOYEE@MAIL.COM",
                        Email = "employee@mail.com",
                        NormalizedEmail = "EMPLOYEE@MAIL.COM",
                        FirstName = "Employee",
                        LastName = "Andersen"
                    };

                    await userManager.CreateAsync(employeeUser, "employee123#");
                    await userManager.AddToRoleAsync(employeeUser, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedSupervisor(EmployeeHolidayDbContext dbContext)
        {
            if (dbContext.Supervisors.Any())
            {
                return;
            }

            supervisor = new Supervisor()
            {
                Id = Guid.Parse("4d7c6287-5d0d-4c5a-a8ef-a01f7be06fa2"),
                UserId = supervisorUser.Id
            };

            Task
                .Run(async () =>
                {
                    await dbContext.Supervisors.AddAsync(supervisor);
                    await dbContext.SaveChangesAsync();
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedEmployee(EmployeeHolidayDbContext dbContext)
        {
            if (dbContext.Employees.Any())
            {
                return;
            }

            Task
                .Run(async () =>
                {
                    var employee = new Employee()
                    {
                        Id = Guid.Parse("b80b70e0-683d-48bb-a10f-39892ee16f9c"),
                        UserId = employeeUser.Id,
                        SupervisorId = supervisor.Id
                    };

                    await dbContext.Employees.AddAsync(employee);
                    await dbContext.SaveChangesAsync();
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
