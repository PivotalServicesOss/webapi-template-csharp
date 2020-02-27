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
using Microsoft.Extensions.DependencyInjection;

namespace PivotalServices.WebApiTemplate.CSharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureHostConfiguration(ConfigureHost(args))
                .ConfigureAppConfiguration(ConfigureApplicationConfig())
                .UseDefaultServiceProvider(ConfigureServiceProvider())
                .ConfigureWebHostDefaults(ConfigureWebHost())
                .ConfigureLogging(ConfigureLogging());
        }

        private static Action<HostBuilderContext, ServiceProviderOptions> ConfigureServiceProvider()
        {
            return (hostingContext, options) =>
            {
                var isDevelopment = hostingContext.HostingEnvironment.IsDevelopment();
                options.ValidateScopes = isDevelopment;
                options.ValidateOnBuild = isDevelopment;
            };
        }

        private static Action<IWebHostBuilder> ConfigureWebHost()
        {
            return webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseCloudHosting(5000, 5001);
            };
        }

        private static Action<HostBuilderContext, ILoggingBuilder> ConfigureLogging()
        {
            return (hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole(options =>
                {
                    options.IncludeScopes = Convert.ToBoolean(
                    hostingContext.Configuration["Logging:IncludeScopes"]);
                });
                loggingBuilder.AddDebug();
                loggingBuilder.AddEventSourceLogger();
            };
        }

        private static Action<HostBuilderContext, IConfigurationBuilder> ConfigureApplicationConfig()
        {
            return (hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

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
                    .AddEnvironmentVariables()
                    .AddConfigServer()
                    .AddPlaceholderResolver();
            };
        }

        private static Action<IConfigurationBuilder> ConfigureHost(string[] args)
        {
            return config =>
            {
                config.AddEnvironmentVariables(prefix: "DOTNET_");
                if (args != null)
                    config.AddCommandLine(args);
            };
        }
    }
}