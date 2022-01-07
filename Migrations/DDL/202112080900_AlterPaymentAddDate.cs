using System;
using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112080900)]
    public class AlterPaymentAddDate_202112080900 : Migration
    {
        public override void Up()
        {
            Alter.Table("Payment").AddColumn("Date").AsDateTime().WithDefaultValue(DateTime.Now);
        }

        public override void Down()
        {
            Delete.Column("Date").FromTable("Payment");
        }
    }
}