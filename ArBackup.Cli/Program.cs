using ArBackup.Application;
using ArBackup.Application.BusinessLogic.Commands;
using ArBackup.Data.Extensions;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ArBackup.Application.Mediator;
using Ardalis.Result;
using Microsoft.Extensions.Logging;

namespace ArBackup;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Logging.AddSimpleConsole(c => c.SingleLine = true);

        builder.Services.AddApplicationLayer();
        builder.Services.AddDataLayer();

        using var host = builder.Build();
        var dispatcher = builder.Services.BuildServiceProvider().GetRequiredService<IMediator>();

        var results = Parser.Default.ParseArguments<UploadOptions, RestoreOptions>(args);

        await results.WithParsedAsync<UploadOptions>(async options => await UploadFileAsync(options, dispatcher, new CancellationTokenSource().Token));
        await results.WithParsedAsync<RestoreOptions>(async options => await RestoreFileAsync(options, dispatcher, new CancellationTokenSource().Token));
    }

    private static async Task RestoreFileAsync(RestoreOptions options, IMediator dispatcher, CancellationToken cancellationToken) => 
        await dispatcher.Send<RestoreFile.Command, Result>(new RestoreFile.Command(options.File), cancellationToken);

    private static async Task UploadFileAsync(UploadOptions uploadOptions, IMediator dispatcher, CancellationToken cancellationToken) => 
        await dispatcher.Send<UploadFile.Command, Result<UploadFile.Response>>(new UploadFile.Command(uploadOptions.Path), cancellationToken);
}