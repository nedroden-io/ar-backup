using ArBackup.Application.Abstractions;
using Ardalis.Result;
using Microsoft.Extensions.Logging;

namespace ArBackup.Application.BusinessLogic.Commands;

public static class UploadFile
{
    private const string ContainerName = "ar-backup";
    
    public sealed record Command(string Path);

    public sealed record Response(Guid Guid);
    
    public class Handler : ICommandHandler<Command>
    {
        private readonly IStorageService _storageService;
        private readonly ILogger<Handler> _logger;
        public Handler(IStorageService storageService, ILogger<Handler> logger)
        {
            _storageService = storageService;
            _logger = logger;
        }
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var createContainer = await _storageService.CreateContainerIfNotExistsAsync(ContainerName, cancellationToken);
            if (createContainer.IsError()) return;

            var result = await _storageService.UploadFileAsync(ContainerName, request.Path, cancellationToken);
            // You may want to log or handle the result here
        }
    }
}