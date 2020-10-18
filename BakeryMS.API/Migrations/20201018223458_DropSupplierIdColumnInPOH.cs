using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class DropSupplierIdColumnInPOH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "PurchaseOrderHeaders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "PurchaseOrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
