using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class AddedProdItemTable_AndChangesToItemTable_AndProdOrderHeaderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "isProcessed",
                table: "ProductionOrderHeaders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpireDays",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "Items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductionItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(nullable: false),
                    ManufacturedDate = table.Column<DateTime>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: true),
                    BatchNo = table.Column<int>(nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentPlaceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionItems_BusinessPlaces_CurrentPlaceId",
                        column: x => x.CurrentPlaceId,
                        principalTable: "BusinessPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionItems_CurrentPlaceId",
                table: "ProductionItems",
                column: "CurrentPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionItems_ItemId",
                table: "ProductionItems",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionItems");

            migrationBuilder.DropColumn(
                name: "isProcessed",
                table: "ProductionOrderHeaders");

            migrationBuilder.DropColumn(
                name: "ExpireDays",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "Items");
        }
    }
}
