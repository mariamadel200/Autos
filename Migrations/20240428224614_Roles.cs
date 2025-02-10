using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Autos.Migrations
{
    /// <inheritdoc />
    public partial class Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "73241c8a-1a40-41d8-8e1e-63cc1fe6458e", "57e957fa-181a-43b4-806f-f3cec0df6c74", "Admin", "ADMIN" },
                    { "d56add48-ecaf-4e94-b7d2-b9d28e7eb8a6", "7cefefde-61c4-4906-bdde-61522c77d0f1", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73241c8a-1a40-41d8-8e1e-63cc1fe6458e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d56add48-ecaf-4e94-b7d2-b9d28e7eb8a6");
        }
    }
}
