using ArBackup.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ArBackup.Data.Extensions;

public static class ServiceCollectionExtensions
{
    extension (IServiceCollection services)
    {
        public IServiceCollection AddDataLayer()
        {
            services.AddAws();
            services.AddScoped<IStorageService, AwsStorageService>();
            
            return services;
        }
    }
}