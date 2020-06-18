using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PivotalServices.WebApiTemplate.CSharp.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace PivotalServices.WebApiTemplate.CSharp.Documentation
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        private readonly ApiInfoOptions apiOptions;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IOptionsMonitor<ApiInfoOptions> apiOptions)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.apiOptions = (apiOptions ?? throw new ArgumentNullException(nameof(apiOptions))).CurrentValue;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Version = description.ApiVersion.ToString(),
                Title = apiOptions.Title,
                Description = apiOptions.Description,
                Contact = new OpenApiContact() { Name = apiOptions.Contact.Name, Email = apiOptions.Contact.Email },
            };

            if (description.IsDeprecated)
                info.Description += apiOptions.DeprecationMessage;

            return info;
        }
    }
}
