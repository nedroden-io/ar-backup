using Microsoft.Extensions.DependencyInjection;
using ArBackup.Application.Mediator;
using ArBackup.Application.BusinessLogic.Commands;
using Ardalis.Result;

namespace ArBackup.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator.Mediator>();

        services.AddTransient<ICommandHandler<UploadFile.Command, Result<UploadFile.Response>>, UploadFile.Handler>();
        services.AddTransient<ICommandHandler<RestoreFile.Command, Result>, RestoreFile.Handler>();

        return services;
    }
}