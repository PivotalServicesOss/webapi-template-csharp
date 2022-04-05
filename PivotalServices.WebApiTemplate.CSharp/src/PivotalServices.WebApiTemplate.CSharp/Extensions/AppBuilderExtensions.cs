﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using PivotalServices.WebApiTemplate.CSharp.Bootstrap;
using Steeltoe.Connector;
using Steeltoe.Discovery.Client;

namespace PivotalServices.WebApiTemplate.CSharp.Extensions
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder ConfigureCloudFoundryServices(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (configuration.HasCloudFoundryServiceConfigurations())
            {
                app.UseDiscoveryClient();
            }

            return app;
        }

        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

                    options.RoutePrefix = string.Empty;
                });

            return app;
        }

        public static IApplicationBuilder ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ValidationExceptionMiddleware>();

            return app;
        }
    }
}
