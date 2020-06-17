using Microsoft.AspNetCore.Mvc.Testing;

namespace PivotalServices.WebApiTemplate.CSharp.IntegrationTests
{
    public class TestingWebApplicationFactory : WebApplicationFactory<Startup>
    {
        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    base.ConfigureWebHost(builder);
        //    builder.ConfigureAppConfiguration((builderContext, configBuilder) =>
        //    {
        //        configBuilder.SetBasePath(Directory.GetCurrentDirectory());
        //        configBuilder.AddYamlFile("appsettings.Integration.yaml", false);
        //    });
        //}
    }
}
