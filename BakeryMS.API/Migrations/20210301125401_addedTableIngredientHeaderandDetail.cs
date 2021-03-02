using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class addedTableIngredientHeaderandDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IngredientHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ServingSize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Method = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngredientHeaders_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IngredientDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngredientsHeaderId = table.Column<int>(nullable: true),
                    ItemId = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngredientDetails_IngredientHeaders_IngredientsHeaderId",
                        column: x => x.IngredientsHeaderId,
                        principalTable: "IngredientHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IngredientDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientDetails_IngredientsHeaderId",
                table: "IngredientDetails",
                column: "IngredientsHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientDetails_ItemId",
                table: "IngredientDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientHeaders_ItemId",
                table: "IngredientHeaders",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientDetails");

            migrationBuilder.DropTable(
                name: "IngredientHeaders");
        }
    }
}
