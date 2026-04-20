using ArBackup.Application.Abstractions;
using ArBackup.Application.Mediator;
using Ardalis.Result;
using Microsoft.Extensions.Logging;

namespace ArBackup.Application.BusinessLogic.Commands;

public static class UploadFile
{
    private const string ContainerName = "armonden-ar-backup";
    
    public sealed record Command(string Path);

    public sealed record Response(string ObjectName);
    
    public sealed class Handler(IStorageService storageService, ILogger<Handler> logger)
        : ICommandHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var createContainer = await storageService.CreateContainerIfNotExistsAsync(ContainerName, cancellationToken);
            if (createContainer.IsError()) return createContainer;
            
            var path = Path.GetFullPath(request.Path);
            var result = await storageService.UploadFileAsync(ContainerName, path, cancellationToken);

            if (result.IsError()) return Result.Error();
            
            Console.WriteLine(result.Value);

            return Result.Success(new Response(result.Value));
        }
    }
}