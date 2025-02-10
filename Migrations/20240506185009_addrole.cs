using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Autos.Migrations
{
    /// <inheritdoc />
    public partial class addrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehiclesForSale_Sales_SaleId",
                table: "VehiclesForSale");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a87f8a11-c8cb-487c-88bf-e3cfe499f578");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e49ca5fe-c397-4610-b2c0-75fef1e3ac0c");

            migrationBuilder.AlterColumn<int>(
                name: "SaleId",
                table: "VehiclesForSale",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0eb13831-e6d5-40bb-baaa-1f538565dfde", "43e9215c-8337-44ef-ab2c-550bdcb830bb", "SuperAdmin", "SUPERADMIN" },
                    { "1f307a17-0010-4e9c-b771-2b0a7b2c17db", "4838d28d-964e-4391-ba69-40cdf2f2b99e", "User", "USER" },
                    { "e3110d07-d560-4513-8a43-d3a80b9de48d", "78e72f04-d999-48cc-87ca-307d9329cd7c", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_VehiclesForSale_Sales_SaleId",
                table: "VehiclesForSale",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehiclesForSale_Sales_SaleId",
                table: "VehiclesForSale");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0eb13831-e6d5-40bb-baaa-1f538565dfde");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f307a17-0010-4e9c-b771-2b0a7b2c17db");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3110d07-d560-4513-8a43-d3a80b9de48d");

            migrationBuilder.AlterColumn<int>(
                name: "SaleId",
                table: "VehiclesForSale",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a87f8a11-c8cb-487c-88bf-e3cfe499f578", "4494c3ae-5092-4be0-8872-51825563fff3", "Admin", "ADMIN" },
                    { "e49ca5fe-c397-4610-b2c0-75fef1e3ac0c", "18a5ca1f-5676-4319-8bed-e3e03d2658fb", "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_VehiclesForSale_Sales_SaleId",
                table: "VehiclesForSale",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
