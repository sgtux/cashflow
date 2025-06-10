using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202505291200)]
    public class UpdatePaymentType_202505291200 : Migration
    {
        public override void Up()
        {
            Update.Table("Payment").Set(new { Type = 20 }).Where(new { Type = 1 });
            Update.Table("Payment").Set(new { Type = 24 }).Where(new { Type = 2 });
            Update.Table("Payment").Set(new { Type = 5 }).Where(new { Type = 3 });
            Update.Table("Payment").Set(new { Type = 23 }).Where(new { Type = 4 });
            Update.Table("Payment").Set(new { Type = 21 }).Where(new { Type = 5 });
        }

        public override void Down()
        {
        }
    }
}