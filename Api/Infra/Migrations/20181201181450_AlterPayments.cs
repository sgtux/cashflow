using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflow.Api.Infra.Migrations
{
  /// Alter payments
  public partial class AlterPayments : Migration
  {
    /// Up migration
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Payment_CreditCard_CarditCardId1",
          table: "Payment");

      migrationBuilder.DropForeignKey(
          name: "FK_Payment_CreditCard_CarditCardIdId",
          table: "Payment");

      migrationBuilder.DropIndex(
          name: "IX_Payment_CarditCardId1",
          table: "Payment");

      migrationBuilder.DropColumn(
          name: "CarditCardId1",
          table: "Payment");

      migrationBuilder.RenameColumn(
          name: "CarditCardIdId",
          table: "Payment",
          newName: "CreditCardId");

      migrationBuilder.RenameIndex(
          name: "IX_Payment_CarditCardIdId",
          table: "Payment",
          newName: "IX_Payment_CreditCardId");

      migrationBuilder.AddForeignKey(
          name: "FK_Payment_CreditCard_CreditCardId",
          table: "Payment",
          column: "CreditCardId",
          principalTable: "CreditCard",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }

    /// Down migration
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Payment_CreditCard_CreditCardId",
          table: "Payment");

      migrationBuilder.RenameColumn(
          name: "CreditCardId",
          table: "Payment",
          newName: "CarditCardIdId");

      migrationBuilder.RenameIndex(
          name: "IX_Payment_CreditCardId",
          table: "Payment",
          newName: "IX_Payment_CarditCardIdId");

      migrationBuilder.AddColumn<int>(
          name: "CarditCardId1",
          table: "Payment",
          nullable: true);

      migrationBuilder.CreateIndex(
          name: "IX_Payment_CarditCardId1",
          table: "Payment",
          column: "CarditCardId1");

      migrationBuilder.AddForeignKey(
          name: "FK_Payment_CreditCard_CarditCardId1",
          table: "Payment",
          column: "CarditCardId1",
          principalTable: "CreditCard",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);

      migrationBuilder.AddForeignKey(
          name: "FK_Payment_CreditCard_CarditCardIdId",
          table: "Payment",
          column: "CarditCardIdId",
          principalTable: "CreditCard",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
  }
}
