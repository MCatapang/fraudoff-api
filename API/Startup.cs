using Amazon.CognitoIdentityProvider;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Business.Services;
using Business.Services.Aws;

namespace API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            
            ConfigureAws(ref services);
            ConfigureCustomServices(ref services);
        }

        private void ConfigureBaseline(ref IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddConsole());
        }

        private void ConfigureAws(ref IServiceCollection services)
        {
            services.AddTransient<IAmazonCognitoIdentityProvider, AmazonCognitoIdentityProviderClient>();
            services.AddScoped<IAmazonSecretsManager, AmazonSecretsManagerClient>();
            services.AddSingleton<ISecretsManagerCache, SecretsManagerCache>();
        }

        private void ConfigureCustomServices(ref IServiceCollection services)
        {
            // AWS-Related
            services.AddTransient<ICognitoService, CognitoService>();
            services.AddScoped<ISecretsManagerService, SecretsManagerService>();

            // General
            services.AddScoped<IUserAuthService, UserAuthService>();
        }
    }
}
