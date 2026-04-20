using Microsoft.Extensions.DependencyInjection;
using ArBackup.Application.BusinessLogic.Commands;

namespace ArBackup.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator>();

        services.AddTransient<ICommandHandler<UploadFile.Command>, UploadFile.Handler>();

        return services;
    }
}