using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class AddedPlanIdandDescriptionColumnTOProdOrdHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductionOrderHeaders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "ProductionOrderHeaders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductionOrderHeaders");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "ProductionOrderHeaders");
        }
    }
}
