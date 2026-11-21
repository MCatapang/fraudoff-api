using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Business.Constants.Aws.Cognito;
using Business.Constants.General;
using Business.Constants.LoggingMessages;
using Business.Constants.ResultMessages;
using Business.Constants.ResultMessages.Services.Cognito;
using Business.Enums;
using Business.Helpers;
using Business.Models.Services.CognitoService;
using Microsoft.Extensions.Logging;
using Models.Wrappers;
using System.Net;

namespace Business.Services.Aws
{
    public class CognitoService : ICognitoService
    {
        private readonly ILogger<CognitoService> _logger;
        private readonly IAmazonCognitoIdentityProvider _cognitoProvider;

        public CognitoService(
            ILogger<CognitoService> logger,
            IAmazonCognitoIdentityProvider cognitoProvider)
        {
            _logger = logger;
            _cognitoProvider = cognitoProvider;
        }

        // TODO: Create registration endpoints

        /// <inheritdoc/>
        public async Task<MethodResult<AdditionalChallenge?>> AuthenticateLogin(
            string clientId, string secretHash, string userName, string password)
        {
            MethodResult<AdditionalChallenge?> result = new();

            // TODO: Create constants
            Dictionary<string, string> authParams = new()
            {
                { AuthParams.UserName, userName },
                { AuthParams.Password, password },
                { AuthParams.SecretHash, secretHash }
            };

            InitiateAuthRequest request = new()
            {
                ClientId = clientId,
                AuthParameters = authParams,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH
            };

            try
            {
                InitiateAuthResponse response = await _cognitoProvider.InitiateAuthAsync(request);

                if (response.AuthenticationResult is null)
                {
                    result.Message = CognitoResultMessage.AdditionalSteps;
                    result.Result = new AdditionalChallenge(
                        response.ChallengeName,
                        response.ChallengeParameters,
                        response.Session);
                    result.ResultType = ResultType.Unauthorized;
                }
                else
                {
                    result.Message = CognitoResultMessage.SuccessfulLoginAuth;
                    result.Result = default;
                    result.ResultType = ResultType.Success;
                }
            }
            catch (Exception ex)
            {
                string exceptionName = ex.GetType().Name;

                result.Message = exceptionName switch
                {
                    nameof(NotAuthorizedException) => CognitoResultMessage.InvalidLogin,
                    nameof(PasswordResetRequiredException) => CognitoResultMessage.PassResetRequired,
                    nameof(TooManyRequestsException) => ResultMessage.TooManyRequests,
                    nameof(UserNotConfirmedException) => CognitoResultMessage.UserNotConfirmed,
                    _ => ResultMessage.Error
                };
                result.Result = default;
                result.ResultType = exceptionName switch
                {
                    nameof(NotAuthorizedException) => ResultType.Unauthorized,
                    nameof(PasswordResetRequiredException) => ResultType.BadRequest,
                    nameof(TooManyRequestsException) => ResultType.TooManyRequests,
                    nameof(UserNotConfirmedException) => ResultType.Forbidden,
                    _ => ResultType.Error
                };

                _logger.LogError(ex, ex.Message);
            }

            return result;
        }

        public async Task<MethodResult<HttpStatusCode>> AttemptAuthChallenge(
            string challengeName,
            string clientId,
            string userPoolId,
            Dictionary<string, string> challengeResponses,
            string session)
        {
            _logger.LogInformation(LogTemplate.MethodStart, nameof(AttemptAuthChallenge));

            MethodResult<HttpStatusCode> result = new();

            RespondToAuthChallengeRequest request = new()
            {
                ChallengeName = AuthChallengeName.NewPasswordRequired,
                ChallengeResponses = challengeResponses,
                ClientId = clientId,
                Session = session
            };

            try
            {

            }
            catch (Exception ex)
            {


                _logger.LogError(ex, ex.Message);
            }

            _logger.LogInformation(LogTemplate.MethodEnd, nameof(AttemptAuthChallenge));

            return result;
        }

        /// <inheritdoc/>
        public async Task<MethodResult<HttpStatusCode>> RequestPasswordReset(
            string userName, string userPoolId)
        {
            _logger.LogInformation(LogTemplate.MethodStart, nameof(RequestPasswordReset));

            MethodResult<HttpStatusCode> result = new();

            try
            {
                AdminResetUserPasswordRequest request = new()
                {
                    Username = userName,
                    UserPoolId = userPoolId
                };
                AdminResetUserPasswordResponse response = await _cognitoProvider
                    .AdminResetUserPasswordAsync(request);

                result.Message = response.HttpStatusCode switch
                {
                    HttpStatusCode.OK => CognitoResultMessage.ConfirmationSent,
                    HttpStatusCode.Accepted => CognitoResultMessage.ConfirmationSent,
                    HttpStatusCode.NoContent => CognitoResultMessage.ConfirmationSent,
                    HttpStatusCode.Unauthorized => ResultMessage.Error,
                    HttpStatusCode.Forbidden => CognitoResultMessage.PasswordResetProhibited,
                    _ => ResultMessage.Error
                };
                result.Result = response.HttpStatusCode;
                result.ResultType = response.HttpStatusCode switch
                {
                    HttpStatusCode.OK => ResultType.Success,
                    HttpStatusCode.Accepted => ResultType.Success,
                    HttpStatusCode.NoContent => ResultType.Success,
                    HttpStatusCode.Unauthorized => ResultType.Unauthorized,
                    HttpStatusCode.Forbidden => ResultType.Forbidden,
                    _ => ResultType.Error
                };
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Result = ResultHelper.ExceptionToHttpStatusCode(ex.GetType().Name);
                result.ResultType = ResultType.Error;

                _logger.LogError(ex, ex.Message);
            }

            _logger.LogInformation(LogTemplate.MethodEnd, nameof(RequestPasswordReset));

            return result;
        }

        /// <inheritdoc/>
        public async Task<MethodResult<HttpStatusCode>> AttemptPasswordReset(
            string clientid, string userName, string confirmationCode, string newPassword)
        {
            _logger.LogInformation(LogTemplate.MethodStart, nameof(AttemptPasswordReset));

            MethodResult<HttpStatusCode> result = new();

            try
            {
                ConfirmForgotPasswordRequest request = new()
                {
                    ClientId = clientid,
                    Username = userName,
                    ConfirmationCode = confirmationCode,
                    Password = newPassword
                };

                ConfirmForgotPasswordResponse response = await _cognitoProvider
                    .ConfirmForgotPasswordAsync(request);

                result.Message = response.HttpStatusCode is HttpStatusCode.OK
                    ? CognitoResultMessage.PasswordResetSuccess
                    : CognitoResultMessage.PasswordResetFail;
                result.Result = response.HttpStatusCode;
                result.ResultType = response.HttpStatusCode switch
                {
                    // TODO: Modularize these repeating logic in a helper method
                    HttpStatusCode.OK => ResultType.Success,
                    HttpStatusCode.BadRequest => ResultType.BadRequest,
                    HttpStatusCode.Unauthorized => ResultType.Unauthorized,
                    HttpStatusCode.TooManyRequests => ResultType.TooManyRequests,
                    _ => ResultType.Error
                };

                _logger.LogInformation(LogTemplate.ReceivedHttpStatusCode, response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                string exceptionName = ex.GetType().Name;

                result.Message = exceptionName switch
                {
                    nameof(CodeMismatchException) => CognitoResultMessage.CodeMismatch,
                    nameof(ExpiredCodeException) => CognitoResultMessage.CodeExpired,
                    nameof(InvalidPasswordException) => CognitoResultMessage.InvalidNewPassword,
                    nameof(LimitExceededException) => ResultMessage.TooManyRequests,
                    nameof(TooManyFailedAttemptsException) => ResultMessage.TooManyRequests,
                    nameof(TooManyRequestsException) => ResultMessage.TooManyRequests,
                    nameof(UserNotConfirmedException) => CognitoResultMessage.UserNotConfirmed,
                    nameof(UserNotFoundException) => CognitoResultMessage.UserNotFound,
                    _ => ResultMessage.Error
                };
                result.Result = exceptionName switch
                {
                    nameof(CodeMismatchException) => HttpStatusCode.BadRequest,
                    nameof(ExpiredCodeException) => HttpStatusCode.BadRequest,
                    nameof(InvalidPasswordException) => HttpStatusCode.BadRequest,
                    nameof(LimitExceededException) => HttpStatusCode.TooManyRequests,
                    nameof(TooManyFailedAttemptsException) => HttpStatusCode.TooManyRequests,
                    nameof(TooManyRequestsException) => HttpStatusCode.TooManyRequests,
                    nameof(UserNotConfirmedException) => HttpStatusCode.BadRequest,
                    nameof(UserNotFoundException) => HttpStatusCode.BadRequest,
                    _ => HttpStatusCode.InternalServerError
                };
                result.ResultType = exceptionName switch
                {
                    nameof(CodeMismatchException) => ResultType.BadRequest,
                    nameof(ExpiredCodeException) => ResultType.BadRequest,
                    nameof(InvalidPasswordException) => ResultType.BadRequest,
                    nameof(LimitExceededException) => ResultType.TooManyRequests,
                    nameof(TooManyFailedAttemptsException) => ResultType.TooManyRequests,
                    nameof(TooManyRequestsException) => ResultType.TooManyRequests,
                    nameof(UserNotConfirmedException) => ResultType.BadRequest,
                    nameof(UserNotFoundException) => ResultType.BadRequest,
                    _ => ResultType.Error
                };

                _logger.LogError(ex, ex.Message);
            }

            _logger.LogInformation(LogTemplate.MethodEnd, nameof(AttemptPasswordReset));

            return result;
        }
    }
}
