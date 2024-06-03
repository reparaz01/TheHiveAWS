using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Amazon;

namespace TheHiveAWS.Helpers
{
    public static class HelperSecretManager
    {
        public static async Task<string> GetSecretsAsync()
        {
            string secretName = "secretothehive";
            string region = "us-east-1";
            IAmazonSecretsManager client =
                new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT",
            };
            GetSecretValueResponse response;
            response = await client.GetSecretValueAsync(request);
            string secret = response.SecretString;
            return secret;
        }
    }
}