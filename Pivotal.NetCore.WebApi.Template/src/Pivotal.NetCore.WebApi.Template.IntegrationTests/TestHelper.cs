using Pivotal.NetCore.WebApi.Template;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;

namespace Pivotal.NetCore.WebApi.Template.Integration.Tests
{
    public class TestHelper
    {
        // Creates a new instance of TestServer using the application startup
        public static TestServer GetTestServer()
        {
            return new TestServer(
                new WebHostBuilder().UseConfiguration(GetConfiguration(new string[] { }))
                    .UseContentRoot(AppContext.BaseDirectory).UseStartup<Startup>());
        }

        private static IConfiguration GetConfiguration(string[] args)
        {
            return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Integration.json", true, false).Build();
        }
    }
}
