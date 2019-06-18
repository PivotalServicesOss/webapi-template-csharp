using System;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Discovery.Client;
using Pivotal.NetCore.WebApi.Template.Bootstrap;
using Pivotal.NetCore.WebApi.Template.Extensions;
using Pivotal.NetCore.WebApi.Template.Features.Values;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.Management.CloudFoundry;
using Swashbuckle.AspNetCore.Swagger;

namespace Pivotal.NetCore.WebApi.Template
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            if (Configuration.HasCloudFoundryServiceConfigurations())
            {
                services.AddDiscoveryClient(Configuration);
            }

            services.AddMediatR(typeof(Startup).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddActuatorsAndHealthContributors(Configuration);
            services.AddMvc().AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<GetValues>(); });

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "Values API", Version = "v1"}); });

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Form API V1");
                c.RoutePrefix = "swagger";
            });

            app.UseMiddleware<ValidationExceptionMiddleware>();
            app.UseMvc();

            if (configuration.HasCloudFoundryServiceConfigurations())
            {
                app.UseDiscoveryClient();
                app.UseCloudFoundryActuators();
            }
        }
    }
}