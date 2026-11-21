using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services.Aws
{
    public class SecretsManagerService : ISecretsManagerService
    {
        private readonly ILogger<SecretsManagerService> _logger;
        private readonly ISecretsManagerCache _secretsManagerCache;

        public SecretsManagerService(
            ILogger<SecretsManagerService> logger,
            ISecretsManagerCache secretsManagerCache)
        {
            _logger = logger;
            _secretsManagerCache = secretsManagerCache;
        }

        public async Task<string?> GetSecretValue(string secretName)
        {
            string? secretValue = null;

            try
            {
                SecretCacheItem cachedSecret = _secretsManagerCache.GetCachedSecret(secretName);
                // TODO: Create const and standardize this cancellation token delay
                CancellationTokenSource cts = new(TimeSpan.FromSeconds(5));
                GetSecretValueResponse secret = await cachedSecret.GetSecretValue(cts.Token);

                secretValue = secret.SecretString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return secretValue;
        }
    }
}
