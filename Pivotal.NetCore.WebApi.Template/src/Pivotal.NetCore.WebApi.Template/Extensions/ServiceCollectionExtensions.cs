using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Management.CloudFoundry;

namespace Pivotal.NetCore.WebApi.Template.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddActuatorsAndHealthContributors(this IServiceCollection services, IConfiguration configuration)
        {
            //Add additional Health Contributors here
            services.AddCloudFoundryActuators(configuration);
        }
    }
}
