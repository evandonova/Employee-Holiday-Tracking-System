using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeHolidayTrackingSystem.Data.Migrations
{
    public partial class CreatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HolidayRequestStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayRequestStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supervisors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supervisors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HolidayDaysRemaining = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SupervisorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Supervisors_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Supervisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HolidayRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    DisapprovalStatement = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupervisorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidayRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolidayRequests_HolidayRequestStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "HolidayRequestStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolidayRequests_Supervisors_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Supervisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "00ac9dd5-0bb7-46e2-b8d8-0cf24d426689", 0, "1e612c5b-1e41-4a72-b635-a3ed194fdd27", "supervisor@mail.com", false, false, null, "SUPERVISOR@MAIL.COM", "SUPERVISOR@MAIL.COM", "AQAAAAEAACcQAAAAEG3wGjk6W8l5eqLnMakfBgKUISzwxqZh0n7pDYFY4drXwt73IjINMix6O/sdV06uTg==", null, false, "072c5f3b-2926-4059-8b66-291c0cccbc14", false, "supervisor@mail.com" },
                    { "cbd615eb-e41f-48a7-9611-51d126377966", 0, "13308728-6e52-4d2e-b7f2-5d768e77c4ef", "employee@mail.com", false, false, null, "EMPLOYEE@MAIL.COM", "EMPLOYEE@MAIL.COM", "AQAAAAEAACcQAAAAEMDICMUTw8pifIRQidrUwGh7rbQdTI9bbF7W8g5hVZb/GPcg/XM6qT026gpTUdG4NQ==", null, false, "73ea162a-4ce3-43f2-b436-01c2c412c48d", false, "employee@mail.com" }
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

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SupervisorId",
                table: "Employees",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayRequests_EmployeeId",
                table: "HolidayRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayRequests_StatusId",
                table: "HolidayRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayRequests_SupervisorId",
                table: "HolidayRequests",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Supervisors_UserId",
                table: "Supervisors",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolidayRequests");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "HolidayRequestStatuses");

            migrationBuilder.DropTable(
                name: "Supervisors");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cbd615eb-e41f-48a7-9611-51d126377966");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00ac9dd5-0bb7-46e2-b8d8-0cf24d426689");
        }
    }
}
