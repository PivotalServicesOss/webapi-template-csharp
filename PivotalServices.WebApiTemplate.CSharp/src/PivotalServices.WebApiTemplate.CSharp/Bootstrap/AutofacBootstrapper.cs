using Autofac;
using PivotalServices.WebApiTemplate.CSharp.IoC;

namespace PivotalServices.WebApiTemplate.CSharp.Bootstrap
{
    public class AutofacBootstrapper
    {
        public static IContainer Execute()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ApiModule>();
            return builder.Build();
        }
    }
}