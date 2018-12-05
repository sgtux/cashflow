using Microsoft.EntityFrameworkCore.Migrations;

namespace FinanceApi.Migrations
{
    public partial class Payment_FixedPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FixedPayment",
                table: "Payment",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedPayment",
                table: "Payment");
        }
    }
}
