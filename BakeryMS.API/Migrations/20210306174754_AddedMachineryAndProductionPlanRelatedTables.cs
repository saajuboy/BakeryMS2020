using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class AddedMachineryAndProductionPlanRelatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Machineries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Capacity = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BusinessPlaceId = table.Column<int>(nullable: false),
                    PurchaseDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machineries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Machineries_BusinessPlaces_BusinessPlaceId",
                        column: x => x.BusinessPlaceId,
                        principalTable: "BusinessPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionPlanHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionSessionId = table.Column<int>(nullable: false),
                    BusinessPlaceId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPlanHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionPlanHeaders_BusinessPlaces_BusinessPlaceId",
                        column: x => x.BusinessPlaceId,
                        principalTable: "BusinessPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionPlanHeaders_ProductionSessions_ProductionSessionId",
                        column: x => x.ProductionSessionId,
                        principalTable: "ProductionSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionPlanHeaders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionPlanDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionPlanHeaderId = table.Column<int>(nullable: true),
                    ItemId = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPlanDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionPlanDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionPlanDetails_ProductionPlanHeaders_ProductionPlanHeaderId",
                        column: x => x.ProductionPlanHeaderId,
                        principalTable: "ProductionPlanHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionPlanMachines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionPlanHeaderId = table.Column<int>(nullable: true),
                    MachineryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPlanMachines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionPlanMachines_Machineries_MachineryId",
                        column: x => x.MachineryId,
                        principalTable: "Machineries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionPlanMachines_ProductionPlanHeaders_ProductionPlanHeaderId",
                        column: x => x.ProductionPlanHeaderId,
                        principalTable: "ProductionPlanHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionPlanRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productionPlanHeaderId = table.Column<int>(nullable: true),
                    ItemId = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPlanRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionPlanRecipes_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionPlanRecipes_ProductionPlanHeaders_productionPlanHeaderId",
                        column: x => x.productionPlanHeaderId,
                        principalTable: "ProductionPlanHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionPlanWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionPlanHeaderId = table.Column<int>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPlanWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionPlanWorkers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionPlanWorkers_ProductionPlanHeaders_ProductionPlanHeaderId",
                        column: x => x.ProductionPlanHeaderId,
                        principalTable: "ProductionPlanHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Machineries_BusinessPlaceId",
                table: "Machineries",
                column: "BusinessPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanDetails_ItemId",
                table: "ProductionPlanDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanDetails_ProductionPlanHeaderId",
                table: "ProductionPlanDetails",
                column: "ProductionPlanHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanHeaders_BusinessPlaceId",
                table: "ProductionPlanHeaders",
                column: "BusinessPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanHeaders_ProductionSessionId",
                table: "ProductionPlanHeaders",
                column: "ProductionSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanHeaders_UserId",
                table: "ProductionPlanHeaders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanMachines_MachineryId",
                table: "ProductionPlanMachines",
                column: "MachineryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanMachines_ProductionPlanHeaderId",
                table: "ProductionPlanMachines",
                column: "ProductionPlanHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanRecipes_ItemId",
                table: "ProductionPlanRecipes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanRecipes_productionPlanHeaderId",
                table: "ProductionPlanRecipes",
                column: "productionPlanHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanWorkers_EmployeeId",
                table: "ProductionPlanWorkers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlanWorkers_ProductionPlanHeaderId",
                table: "ProductionPlanWorkers",
                column: "ProductionPlanHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionPlanDetails");

            migrationBuilder.DropTable(
                name: "ProductionPlanMachines");

            migrationBuilder.DropTable(
                name: "ProductionPlanRecipes");

            migrationBuilder.DropTable(
                name: "ProductionPlanWorkers");

            migrationBuilder.DropTable(
                name: "Machineries");

            migrationBuilder.DropTable(
                name: "ProductionPlanHeaders");
        }
    }
}
