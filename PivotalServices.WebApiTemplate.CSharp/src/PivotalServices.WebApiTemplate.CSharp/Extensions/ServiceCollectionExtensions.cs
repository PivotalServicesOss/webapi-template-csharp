using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using PivotalServices.WebApiTemplate.CSharp.Bootstrap;
using PivotalServices.WebApiTemplate.CSharp.Documentation;
using PivotalServices.WebApiTemplate.CSharp.Versioning;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.CloudFoundry;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System.Reflection;

namespace PivotalServices.WebApiTemplate.CSharp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudFoundryServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.HasCloudFoundryServiceConfigurations())
            {
                services.AddDiscoveryClient(configuration);
                services.AddCloudFoundryActuators(configuration);
            }

            return services;
        }
        
        public static IServiceCollection AddApiVersioningServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiInfoOptions>(configuration.GetSection("ApiInfo"));

            services.AddApiVersioning(options => {
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => {
                options.OperationFilter<SwaggerDefaultValues>();
                options.IncludeXmlComments(XmlCommentsFilePath);
                options.CustomSchemaIds(i => i.FullName);
            });

            return services;
        }

        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        public static IServiceCollection AddControllersWithFluentValidation(this IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<Startup>();
                });

            return services;
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
