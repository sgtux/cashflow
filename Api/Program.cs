using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Cashflow.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}