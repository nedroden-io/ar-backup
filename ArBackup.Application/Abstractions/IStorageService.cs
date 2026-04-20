using Ardalis.Result;

namespace ArBackup.Application.Abstractions;

public interface IStorageService
{
    Task<Result> CreateContainerIfNotExistsAsync(string containerName, CancellationToken cancellationToken);
    
    Task<Result<string>> UploadFileAsync(string containerName, string path, CancellationToken cancellationToken);

    Task<Result> DownloadFileAsync(string containerName, string objectName, string target,
        CancellationToken cancellationToken);
}