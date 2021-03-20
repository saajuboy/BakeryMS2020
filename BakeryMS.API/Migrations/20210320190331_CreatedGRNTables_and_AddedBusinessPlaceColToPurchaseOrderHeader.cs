using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class CreatedGRNTables_and_AddedBusinessPlaceColToPurchaseOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessPlaceId",
                table: "PurchaseOrderHeaders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GRNHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderHeaderId = table.Column<int>(nullable: false),
                    ReceivedDate = table.Column<DateTime>(nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRNHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNHeaders_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                        column: x => x.PurchaseOrderHeaderId,
                        principalTable: "PurchaseOrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GRNDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GRNHeaderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRNDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNDetails_GRNHeaders_GRNHeaderId",
                        column: x => x.GRNHeaderId,
                        principalTable: "GRNHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeaders_BusinessPlaceId",
                table: "PurchaseOrderHeaders",
                column: "BusinessPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNDetails_GRNHeaderId",
                table: "GRNDetails",
                column: "GRNHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNDetails_ItemId",
                table: "GRNDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNHeaders_PurchaseOrderHeaderId",
                table: "GRNHeaders",
                column: "PurchaseOrderHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderHeaders_BusinessPlaces_BusinessPlaceId",
                table: "PurchaseOrderHeaders",
                column: "BusinessPlaceId",
                principalTable: "BusinessPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderHeaders_BusinessPlaces_BusinessPlaceId",
                table: "PurchaseOrderHeaders");

            migrationBuilder.DropTable(
                name: "GRNDetails");

            migrationBuilder.DropTable(
                name: "GRNHeaders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderHeaders_BusinessPlaceId",
                table: "PurchaseOrderHeaders");

            migrationBuilder.DropColumn(
                name: "BusinessPlaceId",
                table: "PurchaseOrderHeaders");
        }
    }
}
