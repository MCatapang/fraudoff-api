using Business.Constants.Aws.Cognito;
using Business.Constants.Aws.SecretsManager;
using Business.Constants.ResultMessages;
using Business.Enums;
using Business.Enums.Users;
using Business.Helpers;
using Business.Models.Services.CognitoService;
using Business.Models.Users;
using Business.Services.Aws;
using Models.Wrappers;
using System.Net;

namespace Business.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly ISecretsManagerService _secretsManagerService;
        private readonly ICognitoService _cognitoService;

        public UserAuthService(
            ISecretsManagerService secretManagerService, ICognitoService cognitoService)
        {
            _secretsManagerService = secretManagerService;
            _cognitoService = cognitoService;
        }

        public async Task<LoginAuthResult> AuthenticateLogin(string userName, string password)
        {
            LoginAuthResult result = new(false, ResultMessage.Error);

            string? clientId = await _secretsManagerService
                .GetSecretValue(SecretName.FraudOffApiClientId);
            string? clientSecret = await _secretsManagerService
                .GetSecretValue(SecretName.FraudOffApiClientSecret);

            if (new[] {clientId, clientSecret}.All(s => !string.IsNullOrWhiteSpace(s)))
            {
                string? secretHash = SecurityHelper
                    .CalculateSecretHash(clientSecret!, userName, clientId!);

                if (!string.IsNullOrWhiteSpace(secretHash))
                {
                    MethodResult<AdditionalChallenge?> authResult = await _cognitoService
                        .AuthenticateLogin(clientId!, secretHash, userName, password);
                    result = new(
                        authResult.ResultType is ResultType.Success,
                        authResult.Message,
                        authResult.Result);
                }
            }

            return result;
        }

        public async Task<AuthChallengeResult> RespondToAuthChallenge(AuthChallengeInput input)
        {
            AuthChallengeResult result = new(input.ChallengeName);

            bool canContinue = true;
            string? clientId = null;
            string? userPoolId = null;

            if (canContinue)
            {
                canContinue = !string.IsNullOrWhiteSpace(input.ChallengeName)
                    && typeof(AuthChallengeName).GetFields().Any(
                        f => (string?)f.GetRawConstantValue() == input.ChallengeName);
            }

            if (canContinue)
            {
                clientId = await _secretsManagerService
                        .GetSecretValue(SecretName.FraudOffApiClientId);
                userPoolId = await _secretsManagerService
                        .GetSecretValue(SecretName.DefaultUserPoolId);

                canContinue = new[] { clientId, userPoolId }.All(s => !string.IsNullOrWhiteSpace(s));
            }

            if (canContinue)
            {
                MethodResult<HttpStatusCode>? attemptResult = await _cognitoService
                    .AttemptAuthChallenge(
                        input.ChallengeName,
                        clientId!,
                        userPoolId!,
                        input.ChallengeResponses,
                        input.Session);

                if (attemptResult is not null)
                {
                    result.IsAuthChallengeSatisfied = attemptResult is not null
                            && attemptResult.ResultType is ResultType.Success;
                    result.Details = attemptResult!.Message;
                }
            }

            return result;
        }

        public async Task<PasswordResetResult> RequestPasswordReset(string userName)
        {
            PasswordResetResult result = new(
                PasswordResetStage.Request, ResultMessage.Error);

            string? userPoolId = await _secretsManagerService
                .GetSecretValue(SecretName.DefaultUserPoolId);

            if (!string.IsNullOrWhiteSpace(userPoolId))
            {
                MethodResult<HttpStatusCode> resetRequestResult = await _cognitoService
                    .RequestPasswordReset(userName, userPoolId);

                result.Message = resetRequestResult.Message;
            }

            return result;
        }

        // TODO: Add documentation for all of the methods in this class
        public async Task<PasswordResetResult> AttemptPasswordReset(
            string confirmationCode, string userName, string newPassword)
        {
            PasswordResetResult result = new(
                PasswordResetStage.Attempt, ResultMessage.Error);

            string? clientId = await _secretsManagerService
                    .GetSecretValue(SecretName.FraudOffApiClientId);

            if (!string.IsNullOrWhiteSpace(clientId))
            {
                MethodResult<HttpStatusCode> attemptResult = await _cognitoService
                    .AttemptPasswordReset(clientId, userName, confirmationCode, newPassword);

                result.Message = attemptResult.Message;
            }

            return result;
        }
    }
}
