namespace Business.Services.Aws
{
    public interface ISecretsManagerService
    {
        Task<string?> GetSecretValue(string secretName);
    }
}
