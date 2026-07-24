using System.Collections.Generic;
using System.IO;
using Cashflow.Tests.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Cashflow.Tests.Mocks
{
    public class WebApplicationTest : WebApplicationFactory<TestStartup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(config => config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["hostBuilder:reloadConfigOnChange"] = "false"
                }))
                .ConfigureWebHostDefaults(builder =>
                    builder.UseStartup<TestStartup>());
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            return base.CreateHost(builder);
        }
    }
}