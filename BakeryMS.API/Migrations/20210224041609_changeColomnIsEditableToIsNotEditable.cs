using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class changeColomnIsEditableToIsNotEditable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEditable",
                table: "ProductionOrderHeaders");

            migrationBuilder.AddColumn<bool>(
                name: "IsNotEditable",
                table: "ProductionOrderHeaders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotEditable",
                table: "ProductionOrderHeaders");

            migrationBuilder.AddColumn<bool>(
                name: "IsEditable",
                table: "ProductionOrderHeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
