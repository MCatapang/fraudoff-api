using Amazon.CognitoIdentityProvider;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Business.Services;
using Business.Services.Aws;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Services: Basics
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Services: AWS
        var awsOptions = builder.Configuration.GetAWSOptions();
        builder.Services.AddDefaultAWSOptions(awsOptions);
        builder.Services.AddTransient<IAmazonCognitoIdentityProvider, AmazonCognitoIdentityProviderClient>();
        builder.Services.AddSingleton<IAmazonSecretsManager, AmazonSecretsManagerClient>();
        builder.Services.AddSingleton<ISecretsManagerCache, SecretsManagerCache>();

        // Services: Custom - AWS-Related
        builder.Services.AddTransient<ICognitoService, CognitoService>();
        builder.Services.AddScoped<ISecretsManagerService, SecretsManagerService>();

        // Services: Custom - General
        builder.Services.AddTransient<IUserAuthService, UserAuthService>();

        var app = builder.Build();

        // HTTP Request Pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}