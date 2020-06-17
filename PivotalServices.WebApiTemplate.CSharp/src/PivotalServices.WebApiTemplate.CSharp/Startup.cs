using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PivotalServices.WebApiTemplate.CSharp.Extensions;

namespace PivotalServices.WebApiTemplate.CSharp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddCloudFoundryServices(Configuration);
            services.AddMediatRServices();
            services.AddApiVersioningServices(Configuration);
            services.AddSwaggerServices();
            services.AddControllers();
            services.AddValidatorsFromAssemblyContaining<Startup>();
        }


        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(AutofacChildLifetimeScopeConfigurationAdapter config)
        {
            // Register your own things directly with Autofac
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.ConfigureExceptionMiddleware();
            app.ConfigureSwagger(provider);
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.ConfigureCloudFoundryServices(configuration);
        }
    }
}