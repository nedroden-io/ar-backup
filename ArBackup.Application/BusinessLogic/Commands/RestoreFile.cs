using ArBackup.Application.Abstractions;
using ArBackup.Application.Mediator;
using Ardalis.Result;
using Microsoft.Extensions.Logging;

namespace ArBackup.Application.BusinessLogic.Commands;

public static class RestoreFile
{
    private const string ContainerName = "armonden-ar-backup";

    public sealed record Command(string ObjectName);

    public sealed class Handler(
        IStorageService storageService,
        ILogger<Handler> logger) : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await storageService.DownloadFileAsync(ContainerName, request.ObjectName, request.ObjectName, cancellationToken);

            if (result.IsError())
            {
                return Result.Error();
            }

            logger.LogInformation("Restored file {FileId} to {Path}", request.ObjectName, request.ObjectName);

            return Result.Success();
        }
    }
}