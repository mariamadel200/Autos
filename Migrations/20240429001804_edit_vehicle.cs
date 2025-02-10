using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Autos.Migrations
{
    /// <inheritdoc />
    public partial class edit_vehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73241c8a-1a40-41d8-8e1e-63cc1fe6458e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d56add48-ecaf-4e94-b7d2-b9d28e7eb8a6");

            migrationBuilder.RenameColumn(
                name: "Tybe",
                table: "Vehicles",
                newName: "Type");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a424235e-8266-44a8-baa3-f5aa87f94aa4", "30196308-9b90-489b-b8ea-17d77dc52407", "User", "USER" },
                    { "e0e2c0aa-e5d6-4a62-81a1-4e74a9110fab", "3417d652-2425-4a97-94c1-3ff93da7bc16", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a424235e-8266-44a8-baa3-f5aa87f94aa4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e0e2c0aa-e5d6-4a62-81a1-4e74a9110fab");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Vehicles",
                newName: "Tybe");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "73241c8a-1a40-41d8-8e1e-63cc1fe6458e", "57e957fa-181a-43b4-806f-f3cec0df6c74", "Admin", "ADMIN" },
                    { "d56add48-ecaf-4e94-b7d2-b9d28e7eb8a6", "7cefefde-61c4-4906-bdde-61522c77d0f1", "User", "USER" }
                });
        }
    }
}
