using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeHolidayTrackingSystem.Data.Migrations
{
    public partial class MovedSeedOutOfDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("b80b70e0-683d-48bb-a10f-39892ee16f9c"));

            migrationBuilder.DeleteData(
                table: "HolidayRequestStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "HolidayRequestStatuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "HolidayRequestStatuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cbd615eb-e41f-48a7-9611-51d126377966");

            migrationBuilder.DeleteData(
                table: "Supervisors",
                keyColumn: "Id",
                keyValue: new Guid("4d7c6287-5d0d-4c5a-a8ef-a01f7be06fa2"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00ac9dd5-0bb7-46e2-b8d8-0cf24d426689");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "00ac9dd5-0bb7-46e2-b8d8-0cf24d426689", 0, "8be90515-1b99-4c39-84d2-ff4671cec750", "supervisor@mail.com", false, "Supervisor", "Jefferson", false, null, "SUPERVISOR@MAIL.COM", "SUPERVISOR@MAIL.COM", "AQAAAAEAACcQAAAAECoNGLRrIgwUFw0UJUFlWR4PrghknVA61mKhIYoO1HNFB7aQVbL4yk7Kbf36F9HGvw==", null, false, "8c7c526e-d2f4-4315-8adc-bbf0fc5ab19f", false, "supervisor@mail.com" },
                    { "cbd615eb-e41f-48a7-9611-51d126377966", 0, "e90b7bc6-ffc6-4e3a-9272-5ad728b0fb4e", "employee@mail.com", false, "Employee", "Andersen", false, null, "EMPLOYEE@MAIL.COM", "EMPLOYEE@MAIL.COM", "AQAAAAEAACcQAAAAEA1FAf/5YrbZZPuVGFo6xE2d4ALU1MxRV59CEoPiDovAp6CVqnBUW1m3a9nhyg5mTQ==", null, false, "830d258d-4073-4142-a139-9406627814b8", false, "employee@mail.com" }
                });

            migrationBuilder.InsertData(
                table: "HolidayRequestStatuses",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Approved" },
                    { 3, "Disapproved" }
                });

            migrationBuilder.InsertData(
                table: "Supervisors",
                columns: new[] { "Id", "UserId" },
                values: new object[] { new Guid("4d7c6287-5d0d-4c5a-a8ef-a01f7be06fa2"), "00ac9dd5-0bb7-46e2-b8d8-0cf24d426689" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "HolidayDaysRemaining", "SupervisorId", "UserId" },
                values: new object[] { new Guid("b80b70e0-683d-48bb-a10f-39892ee16f9c"), 20, new Guid("4d7c6287-5d0d-4c5a-a8ef-a01f7be06fa2"), "cbd615eb-e41f-48a7-9611-51d126377966" });
        }
    }
}
