using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class addedProductionOrderHeaderANdDetail_ProductionSession_BusinessPlace_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    RegistrationNumber = table.Column<string>(nullable: true),
                    RegistrationStartDate = table.Column<DateTime>(nullable: true),
                    RegistrationEndDate = table.Column<DateTime>(nullable: true),
                    EstablishedDate = table.Column<DateTime>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPlaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionSessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Session = table.Column<string>(nullable: true),
                    StartTime = table.Column<TimeSpan>(nullable: true),
                    EndTime = table.Column<TimeSpan>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrderHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderNo = table.Column<int>(nullable: false),
                    SessionId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    EnteredDate = table.Column<DateTime>(nullable: true),
                    RequiredDate = table.Column<DateTime>(nullable: true),
                    BusinessPlaceId = table.Column<int>(nullable: true),
                    IsEditable = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrderHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionOrderHeaders_BusinessPlaces_BusinessPlaceId",
                        column: x => x.BusinessPlaceId,
                        principalTable: "BusinessPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOrderHeaders_ProductionSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "ProductionSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionOrderHeaders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderHeaderId = table.Column<int>(nullable: true),
                    ItemId = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionOrderDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionOrderDetails_ProductionOrderHeaders_ProductionOrderHeaderId",
                        column: x => x.ProductionOrderHeaderId,
                        principalTable: "ProductionOrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrderDetails_ItemId",
                table: "ProductionOrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrderDetails_ProductionOrderHeaderId",
                table: "ProductionOrderDetails",
                column: "ProductionOrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrderHeaders_BusinessPlaceId",
                table: "ProductionOrderHeaders",
                column: "BusinessPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrderHeaders_SessionId",
                table: "ProductionOrderHeaders",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrderHeaders_UserId",
                table: "ProductionOrderHeaders",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionOrderDetails");

            migrationBuilder.DropTable(
                name: "ProductionOrderHeaders");

            migrationBuilder.DropTable(
                name: "BusinessPlaces");

            migrationBuilder.DropTable(
                name: "ProductionSessions");
        }
    }
}
