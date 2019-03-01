using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflow.Api.Infra.Migrations
{
  /// Add fixed payment column
  public partial class Payment_FixedPayment : Migration
  {
    /// Up migration
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<bool>(
          name: "FixedPayment",
          table: "Payment",
          nullable: false,
          defaultValue: false);
    }

    /// Down migration
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "FixedPayment",
          table: "Payment");
    }
  }
}
