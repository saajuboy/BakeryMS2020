using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class CreatedCompanyAndRawItemsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyItems",
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
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentPlaceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyItems_BusinessPlaces_CurrentPlaceId",
                        column: x => x.CurrentPlaceId,
                        principalTable: "BusinessPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RawItems",
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
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentPlaceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RawItems_BusinessPlaces_CurrentPlaceId",
                        column: x => x.CurrentPlaceId,
                        principalTable: "BusinessPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RawItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyItems_CurrentPlaceId",
                table: "CompanyItems",
                column: "CurrentPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyItems_ItemId",
                table: "CompanyItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RawItems_CurrentPlaceId",
                table: "RawItems",
                column: "CurrentPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_RawItems_ItemId",
                table: "RawItems",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyItems");

            migrationBuilder.DropTable(
                name: "RawItems");
        }
    }
}
