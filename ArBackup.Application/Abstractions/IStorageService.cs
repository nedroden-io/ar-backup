using Ardalis.Result;

namespace ArBackup.Application.Abstractions;

public interface IStorageService
{
    Task<Result> CreateContainerIfNotExistsAsync(string containerName, CancellationToken cancellationToken);
    
    Task<Result<Guid>> UploadFileAsync(string containerName, string path, CancellationToken cancellationToken);

    Task<Result> DownloadFileAsync(string containerName, Guid guid, string target,
        CancellationToken cancellationToken);
}