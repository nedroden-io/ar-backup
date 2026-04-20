using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using ArBackup.Application.Abstractions;
using ArBackup.Data.Extensions;
using Ardalis.Result;
using Microsoft.Extensions.Logging;

namespace ArBackup.Data;

public class AwsStorageService(
    IAmazonS3 amazonS3,
    ILogger<AwsStorageService> logger) : IStorageService
{
    public async Task<Result> CreateContainerIfNotExistsAsync(string containerName, CancellationToken cancellationToken)
    {
        // TODO: This throws an exception if the bucket does not exist
        var exists = await amazonS3.HeadBucketAsync(new HeadBucketRequest { BucketName = containerName }, cancellationToken);
        
        if (exists is { HttpStatusCode: HttpStatusCode.OK }) return Result.Success();
        
        var response = await amazonS3.PutBucketAsync(new PutBucketRequest
        {
            BucketName = containerName,
            UseClientRegion = true,
            PutBucketConfiguration = new PutBucketConfiguration
            {
                BucketInfo = new BucketInfo
                {
                    Type = BucketType.Directory,
                }
            }
        }, cancellationToken);
        
        return response is { HttpStatusCode: HttpStatusCode.OK } ? Result.Success() : Result.Error();
    }

    public async Task<Result<string>> UploadFileAsync(string containerName, string path, CancellationToken cancellationToken)
    {
        var request = new PutObjectRequest()
        {
            BucketName = containerName,
            Key = path.ToBase64(),
            FilePath = path
        };
        
        var response = await amazonS3.PutObjectAsync(request, cancellationToken);
        
        if (response is null || response.HttpStatusCode != HttpStatusCode.OK) return Result.Error();
        
        logger.LogInformation("Uploaded file {Path}", path);

        return path;
    }
    
    public async Task<Result> DownloadFileAsync(string containerName, string objectName, string target, CancellationToken cancellationToken)
    {
        var request = new GetObjectRequest
        {
            BucketName = containerName,
            Key = objectName.ToBase64()
        };

        using var response = await amazonS3.GetObjectAsync(request, cancellationToken);
        using var reader = new StreamReader(response.ResponseStream);
        var content = await reader.ReadToEndAsync(cancellationToken);
        
        await File.WriteAllTextAsync(target, content, cancellationToken);

        return Result.Success();
    }
}