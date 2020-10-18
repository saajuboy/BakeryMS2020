using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class AddedSupplierTableANDRelatedToPOH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "PurchaseOrderHeaders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeaders_SupplierId",
                table: "PurchaseOrderHeaders",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderHeaders_Suppliers_SupplierId",
                table: "PurchaseOrderHeaders",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderHeaders_Suppliers_SupplierId",
                table: "PurchaseOrderHeaders");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderHeaders_SupplierId",
                table: "PurchaseOrderHeaders");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "PurchaseOrderHeaders");
        }
    }
}
