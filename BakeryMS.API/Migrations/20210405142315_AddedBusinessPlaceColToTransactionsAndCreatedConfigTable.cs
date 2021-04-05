using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class AddedBusinessPlaceColToTransactionsAndCreatedConfigTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessPlaceId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configurations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BusinessPlaceId",
                table: "Transactions",
                column: "BusinessPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Configurations_UserId",
                table: "Configurations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BusinessPlaces_BusinessPlaceId",
                table: "Transactions",
                column: "BusinessPlaceId",
                principalTable: "BusinessPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BusinessPlaces_BusinessPlaceId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BusinessPlaceId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BusinessPlaceId",
                table: "Transactions");
        }
    }
}
