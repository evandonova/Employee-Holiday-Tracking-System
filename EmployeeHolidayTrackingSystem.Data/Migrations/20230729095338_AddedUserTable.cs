using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeHolidayTrackingSystem.Data.Migrations
{
    public partial class AddedUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00ac9dd5-0bb7-46e2-b8d8-0cf24d426689",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8be90515-1b99-4c39-84d2-ff4671cec750", "Supervisor", "Jefferson", "AQAAAAEAACcQAAAAECoNGLRrIgwUFw0UJUFlWR4PrghknVA61mKhIYoO1HNFB7aQVbL4yk7Kbf36F9HGvw==", "8c7c526e-d2f4-4315-8adc-bbf0fc5ab19f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cbd615eb-e41f-48a7-9611-51d126377966",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e90b7bc6-ffc6-4e3a-9272-5ad728b0fb4e", "Employee", "Andersen", "AQAAAAEAACcQAAAAEA1FAf/5YrbZZPuVGFo6xE2d4ALU1MxRV59CEoPiDovAp6CVqnBUW1m3a9nhyg5mTQ==", "830d258d-4073-4142-a139-9406627814b8" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00ac9dd5-0bb7-46e2-b8d8-0cf24d426689",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e612c5b-1e41-4a72-b635-a3ed194fdd27", "AQAAAAEAACcQAAAAEG3wGjk6W8l5eqLnMakfBgKUISzwxqZh0n7pDYFY4drXwt73IjINMix6O/sdV06uTg==", "072c5f3b-2926-4059-8b66-291c0cccbc14" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cbd615eb-e41f-48a7-9611-51d126377966",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "13308728-6e52-4d2e-b7f2-5d768e77c4ef", "AQAAAAEAACcQAAAAEMDICMUTw8pifIRQidrUwGh7rbQdTI9bbF7W8g5hVZb/GPcg/XM6qT026gpTUdG4NQ==", "73ea162a-4ce3-43f2-b436-01c2c412c48d" });
        }
    }
}
