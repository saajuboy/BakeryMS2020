using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class AddedISdeletedColumnToCus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Customers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Customers");
        }
    }
}
