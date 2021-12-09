using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112080902)]
    public class UpdatePaymentDate_202112080902 : Migration
    {
        public override void Up()
        {
            Execute.Sql($"UPDATE \"Payment\" SET \"Date\" = subquery.NewDate, \"BaseCost\" = subquery.Cost FROM (SELECT i.\"PaymentId\", MIN(i.\"Date\") AS NewDate, SUM(i.\"Cost\") AS Cost FROM \"Installment\" i GROUP BY i.\"PaymentId\") AS subquery WHERE \"Id\" = subquery.\"PaymentId\"");
        }

        public override void Down()
        {
        }
    }
}