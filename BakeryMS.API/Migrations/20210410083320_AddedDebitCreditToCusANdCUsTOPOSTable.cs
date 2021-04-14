using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryMS.API.Migrations
{
    public partial class AddedDebitCreditToCusANdCUsTOPOSTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "SalesHeaders",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Credit",
                table: "Customers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Debit",
                table: "Customers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_SalesHeaders_CustomerId",
                table: "SalesHeaders",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesHeaders_Customers_CustomerId",
                table: "SalesHeaders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesHeaders_Customers_CustomerId",
                table: "SalesHeaders");

            migrationBuilder.DropIndex(
                name: "IX_SalesHeaders_CustomerId",
                table: "SalesHeaders");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SalesHeaders");

            migrationBuilder.DropColumn(
                name: "Credit",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Debit",
                table: "Customers");
        }
    }
}
