using System.IO;
using Cashflow.Migrations;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests.Config
{
    [TestClass]
    public class TestInitializer
    {
        public static int Value;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            if (File.Exists("test.db"))
                File.Delete("test.db");
            var conn = new TestAppConfig().DatabaseConnectionString;
            var runner = new MigrationRunner(DatabaseType.Sqlite, conn, false, typeof(TestInitializer).Assembly);
            runner.Run();
            BaseControllerTest.Init();
        }
    }
}