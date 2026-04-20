using ArBackup.Application.Abstractions;
using ArBackup.Application.Mediator;
using Ardalis.Result;
using Microsoft.Extensions.Logging;

namespace ArBackup.Application.BusinessLogic.Commands;

public static class RestoreFile
{
    private const string ContainerName = "armonden-ar-backup";

    public sealed record Command(Guid FileId);

    public sealed class Handler(
        IStorageService storageService,
        ILogger<Handler> logger) : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await storageService.DownloadFileAsync(ContainerName, request.FileId, "restored.txt", cancellationToken);
            
            return result.IsError() ? Result.Error() : Result.Success();
        }
    }
}