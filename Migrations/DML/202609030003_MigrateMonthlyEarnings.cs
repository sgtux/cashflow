using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202609030003)]
    public class MigrateMonthlyEarnings_202609030003 : Migration
    {
        // Moves every recurring ("Monthy", Type = 1) Earning into the new
        // RecurringEarning / RecurringEarningHistory structure.
        //
        // Assumption: the pair (UserId, Description) uniquely identifies a
        // recurring earning. Distinct recurring earnings sharing the same
        // description for the same user would be merged into one (rare, acceptable).
        public override void Up()
        {
            Execute.Sql(@"
                INSERT INTO RecurringEarning (Description, Value, UserId)
                SELECT g.Description, latest.Value, g.UserId
                FROM (SELECT DISTINCT UserId, Description FROM Earning WHERE Type = 1) g
                CROSS APPLY (
                    SELECT TOP 1 e.Value
                    FROM Earning e
                    WHERE e.UserId = g.UserId AND e.Description = g.Description AND e.Type = 1
                    ORDER BY e.Date DESC
                ) latest;

                INSERT INTO RecurringEarningHistory (Value, Date, RecurringEarningId)
                SELECT e.Value, e.Date, re.Id
                FROM Earning e
                JOIN RecurringEarning re ON re.UserId = e.UserId AND re.Description = e.Description
                WHERE e.Type = 1;

                DELETE FROM Earning WHERE Type = 1;
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
                INSERT INTO Earning (Description, Value, Date, UserId, Type)
                SELECT re.Description, reh.Value, reh.Date, re.UserId, 1
                FROM RecurringEarningHistory reh
                JOIN RecurringEarning re ON re.Id = reh.RecurringEarningId;

                DELETE FROM RecurringEarningHistory;
                DELETE FROM RecurringEarning;
            ");
        }
    }
}
