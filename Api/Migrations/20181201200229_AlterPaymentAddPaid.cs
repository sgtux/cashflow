using Microsoft.EntityFrameworkCore.Migrations;

namespace FinanceApi.Migrations
{
    public partial class AlterPaymentAddPaid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlotsPaid",
                table: "Payment",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlotsPaid",
                table: "Payment");
        }
    }
}
