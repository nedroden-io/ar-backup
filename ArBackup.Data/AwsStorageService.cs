using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using ArBackup.Application.Abstractions;
using Ardalis.Result;

namespace ArBackup.Data;

public class AwsStorageService(IAmazonS3 amazonS3) : IStorageService
{
    public async Task<Result> CreateContainerIfNotExistsAsync(string containerName, CancellationToken cancellationToken)
    {
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
                    Type = BucketType.Directory
                }
            }
        }, cancellationToken);
        
        return response is { HttpStatusCode: HttpStatusCode.OK } ? Result.Success() : Result.Error();
    }

    public async Task<Result<Guid>> UploadFileAsync(string containerName, string path, CancellationToken cancellationToken)
    {
        var objectName = Guid.NewGuid();
        var request = new PutObjectRequest()
        {
            BucketName = containerName,
            Key = objectName.ToString(),
            FilePath = path
        };
        
        var response = await amazonS3.PutObjectAsync(request, cancellationToken);
        
        if (response is null || response.HttpStatusCode != HttpStatusCode.OK) return Result.Error();

        return Result.Success(objectName);
    }
}