using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ArBackup.Application;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationLayer()
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            });
            
            return services;
        }
    }
}