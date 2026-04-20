using ArBackup.Application;
using ArBackup.Application.BusinessLogic.Commands;
using ArBackup.Data.Extensions;
using CommandLine;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArBackup;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddApplicationLayer();
        builder.Services.AddDataLayer();

        using var host = builder.Build();
        
        Console.WriteLine("Application started");
        await Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(async options => await Dispatch(options, builder.Services.BuildServiceProvider().GetRequiredService<IMediator>(), new CancellationTokenSource().Token));
    }

    private static async Task Dispatch(Options options, IMediator mediator, CancellationToken cancellationToken)
    {
        switch (options.Action.ToLower())
        {
            case "upload": 
                await mediator.Publish(new UploadFile.Command(options.Path), cancellationToken);
                break;

            default: throw new InvalidOperationException();
        }
    }
}