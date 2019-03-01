using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflow.Api.Infra.Migrations
{
  /// Bind user with credit card
  public partial class BindUserCreditCard : Migration
  {
    /// Up migration
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Payment_CreditCard_CardId",
          table: "Payment");

      migrationBuilder.RenameColumn(
          name: "CardId",
          table: "Payment",
          newName: "CarditCardIdId");

      migrationBuilder.RenameIndex(
          name: "IX_Payment_CardId",
          table: "Payment",
          newName: "IX_Payment_CarditCardIdId");

      migrationBuilder.AddColumn<int>(
          name: "CarditCardId1",
          table: "Payment",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "UserId",
          table: "CreditCard",
          nullable: false,
          defaultValue: 0);

      migrationBuilder.CreateIndex(
          name: "IX_Payment_CarditCardId1",
          table: "Payment",
          column: "CarditCardId1");

      migrationBuilder.CreateIndex(
          name: "IX_CreditCard_UserId",
          table: "CreditCard",
          column: "UserId");

      migrationBuilder.AddForeignKey(
          name: "FK_CreditCard_User_UserId",
          table: "CreditCard",
          column: "UserId",
          principalTable: "User",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

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

    /// Down migration
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_CreditCard_User_UserId",
          table: "CreditCard");

      migrationBuilder.DropForeignKey(
          name: "FK_Payment_CreditCard_CarditCardId1",
          table: "Payment");

      migrationBuilder.DropForeignKey(
          name: "FK_Payment_CreditCard_CarditCardIdId",
          table: "Payment");

      migrationBuilder.DropIndex(
          name: "IX_Payment_CarditCardId1",
          table: "Payment");

      migrationBuilder.DropIndex(
          name: "IX_CreditCard_UserId",
          table: "CreditCard");

      migrationBuilder.DropColumn(
          name: "CarditCardId1",
          table: "Payment");

      migrationBuilder.DropColumn(
          name: "UserId",
          table: "CreditCard");

      migrationBuilder.RenameColumn(
          name: "CarditCardIdId",
          table: "Payment",
          newName: "CardId");

      migrationBuilder.RenameIndex(
          name: "IX_Payment_CarditCardIdId",
          table: "Payment",
          newName: "IX_Payment_CardId");

      migrationBuilder.AddForeignKey(
          name: "FK_Payment_CreditCard_CardId",
          table: "Payment",
          column: "CardId",
          principalTable: "CreditCard",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
  }
}
