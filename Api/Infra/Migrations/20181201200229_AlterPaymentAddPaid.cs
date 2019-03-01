using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflow.Api.Infra.Migrations
{
  /// Add paid column
  public partial class AlterPaymentAddPaid : Migration
  {
    /// Up migration
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
          name: "PlotsPaid",
          table: "Payment",
          nullable: false,
          defaultValue: 0);
    }

    /// Down migration
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "PlotsPaid",
          table: "Payment");
    }
  }
}
