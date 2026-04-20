using ArBackup.Application.Abstractions;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArBackup.Application.BusinessLogic.Commands;

public static class UploadFile
{
    private const string ContainerName = "ar-backup";
    
    public sealed record Command(string Path) : IRequest<Result<Response>>;

    public sealed record Response(Guid Guid);
    
    public class Handler(
        IStorageService storageService,
        ILogger<Handler> logger) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var createContainer = await storageService.CreateContainerIfNotExistsAsync(ContainerName, cancellationToken);
            if (createContainer.IsError()) return createContainer;

            var result = await storageService.UploadFileAsync(ContainerName, request.Path, cancellationToken);
            
            return result.IsSuccess ? Result.Success(new Response(result.Value)) : Result.Error();
        }
    }
}