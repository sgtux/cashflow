using Microsoft.EntityFrameworkCore.Migrations;

namespace FinanceApi.Migrations
{
    public partial class SinglePlot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SinglePlot",
                table: "Payment",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SinglePlot",
                table: "Payment");
        }
    }
}
