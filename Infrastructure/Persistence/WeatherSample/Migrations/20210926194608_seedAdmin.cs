using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherSample.Persistence.Migrations
{
    public partial class seedAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, "99e999c6-0e08-4d9b-bd45-f5decf3c1563", "m.zatkhahi@gmail.com", true, "Mohammad", false, "Zatkhahi", false, null, "M.ZATKHAHI@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEHxRI+/2kwpKM6joP8POC1u+AdHbJIAX5a1klksGZ6hwdELsrz00VrIYZE+Jt2NhCw==", null, false, null, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
