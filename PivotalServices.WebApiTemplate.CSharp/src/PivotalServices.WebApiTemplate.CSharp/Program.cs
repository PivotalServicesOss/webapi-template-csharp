using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Hosting;
using System;
using System.IO;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Logging;
using Steeltoe.Extensions.Configuration.Placeholder;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace PivotalServices.WebApiTemplate.CSharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new HostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureHostConfiguration(config =>
            {
                config.AddEnvironmentVariables(prefix: "DOTNET_");
                if (args != null)
                    config.AddCommandLine(args);
            })
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                var clientSettings = new ConfigServerClientSettings { Environment = env.EnvironmentName };

                if (env.IsDevelopment() && !string.IsNullOrEmpty(env.ApplicationName))
                {
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                    if (appAssembly != null)
                        config.AddUserSecrets(appAssembly, optional: true);
                }

                config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, false)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, false)
                    .AddYamlFile("appsettings.yaml", true, false)
                    .AddYamlFile($"appsettings.{env.EnvironmentName}.yaml", true, false)
                    .AddPlaceholderResolver()
                    .AddEnvironmentVariables()
                    .AddConfigServer(clientSettings);
            })
            .UseDefaultServiceProvider((hostingContext, options) =>
            {
                var isDevelopment = hostingContext.HostingEnvironment.IsDevelopment();
                options.ValidateScopes = isDevelopment;
                options.ValidateOnBuild = isDevelopment;
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseCloudHosting(5000, 5001);
            })
            .ConfigureLogging((hostingContext, loggingBuilder) => 
            {
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole(
                    options =>
                    {
                        options.IncludeScopes = Convert.ToBoolean(
                                hostingContext.Configuration["Logging:IncludeScopes"]);
                    });
                loggingBuilder.AddDebug();
                loggingBuilder.AddDynamicConsole();
                loggingBuilder.AddEventSourceLogger();
            })
            .Build()
            .Run();
        }
    }
}