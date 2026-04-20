using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Microsoft.Extensions.DependencyInjection;

namespace ArBackup.Data.Extensions;

public static class AwsExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAws()
        {
            var chain = new CredentialProfileStoreChain();
            if (!chain.TryGetAWSCredentials("default", out var credentials)) throw new ArgumentException("Default AWS profile not found");

            services.AddScoped<IAmazonS3>(_ => new AmazonS3Client(credentials));

            return services;
        }
    }
    
    extension(IAmazonSecurityTokenService securityTokenService)
    {
        public async Task<string> GetCallerIdentity()
        {
            var response = await securityTokenService.GetCallerIdentityAsync(new GetCallerIdentityRequest());

            return response.Arn;
        }
    }
}