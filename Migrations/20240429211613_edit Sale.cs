using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Autos.Migrations
{
    /// <inheritdoc />
    public partial class editSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a424235e-8266-44a8-baa3-f5aa87f94aa4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e0e2c0aa-e5d6-4a62-81a1-4e74a9110fab");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32dd0c79-0bd9-4ba5-8e4a-a97d9a94549f", "02e1c944-d38f-4757-8e79-e5889f91532d", "Admin", "ADMIN" },
                    { "5c7abb69-bcef-4e4f-93a0-5448c978506d", "4487191d-d88e-4a0a-bfcd-860fee2994c4", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32dd0c79-0bd9-4ba5-8e4a-a97d9a94549f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c7abb69-bcef-4e4f-93a0-5448c978506d");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Sales",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a424235e-8266-44a8-baa3-f5aa87f94aa4", "30196308-9b90-489b-b8ea-17d77dc52407", "User", "USER" },
                    { "e0e2c0aa-e5d6-4a62-81a1-4e74a9110fab", "3417d652-2425-4a97-94c1-3ff93da7bc16", "Admin", "ADMIN" }
                });
        }
    }
}
