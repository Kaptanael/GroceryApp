using Microsoft.EntityFrameworkCore.Migrations;

namespace GroceryApp.Migrations
{
    public partial class RemoveCascadeCustoner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransactions_Customers_CustomerId",
                table: "CustomerTransactions");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransactions_Customers_CustomerId",
                table: "CustomerTransactions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransactions_Customers_CustomerId",
                table: "CustomerTransactions");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransactions_Customers_CustomerId",
                table: "CustomerTransactions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
