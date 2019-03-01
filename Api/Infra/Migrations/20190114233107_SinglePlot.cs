using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflow.Api.Infra.Migrations
{
  /// Single plot
  public partial class SinglePlot : Migration
  {
    /// Up migration
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<bool>(
          name: "SinglePlot",
          table: "Payment",
          nullable: false,
          defaultValue: false);
    }

    /// Down migration
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "SinglePlot",
          table: "Payment");
    }
  }
}
