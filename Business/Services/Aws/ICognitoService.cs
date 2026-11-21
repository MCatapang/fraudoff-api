using Amazon.CognitoIdentityProvider.Model;
using Business.Enums;
using Business.Models.Services.CognitoService;
using Models.Wrappers;
using System.Net;

namespace Business.Services.Aws
{
    public interface ICognitoService
    {
        /// <summary>
        ///     Authenticates a user that's trying to log in - ensuring that the user has the
        ///     correct user name and password combination, if the combination does exist.
        /// </summary>
        /// <param name="clientId">
        ///     The client through which a user is requesting to reset their password (e.g.,
        ///     fraudoff-api, fraudoff-ui, etc.).
        /// </param>
        /// <param name="clientSecret">
        ///     The secret associated with the client specified through the Client ID. 
        /// </param>
        /// <param name="emailAddress">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>
        ///     Additional challenges - if any - that need to be completed by the user in order to 
        ///     authenticate their log-in attempt.
        /// </returns>
        Task<MethodResult<AdditionalChallenge?>> AuthenticateLogin(
            string clientId, string clientSecret, string emailAddress, string password);


        Task<MethodResult<HttpStatusCode>> AttemptAuthChallenge(
            string challengeName,
            string clientId,
            string userPoolId,
            Dictionary<string, string> challengeResponses,
            string session);

        /// <summary>
        ///     Initiates the password reset process between a user and Cognito.
        ///     Successful initiation results in a code being sent to a user's email address
        ///     and/or phone number.
        /// </summary>
        /// <param name="userName">The user who's requesting for their password to be reset.</param>
        /// <param name="userPoolId">The Cognito user pool that the user belongs to.</param>
        /// <returns>The HTTP status code for the request to Cognito for a password reset.</returns>
        Task<MethodResult<HttpStatusCode>> RequestPasswordReset(
            string userName, string userPoolId);

        /// <summary>
        ///     Attempts to complete the password reset process between a user and Cognito.
        /// </summary>
        /// <param name="clientId">
        ///     The client through which a user is attempting to 
        ///     complete the password reset process.
        /// </param>
        /// <param name="userName">The user who's requesting for their password to be reset.</param>
        /// <param name="confirmationCode">
        ///     The secure code that was sent to the 
        ///     user's email address in order to confirm that the user does indeed want
        ///     to reset their password.
        /// </param>
        /// <param name="newPassword">
        ///     The user's brand new password that the user will be using instead
        ///     of their old password hereafter - in the case that a successful reset is attained.
        /// </param>
        /// <returns>
        ///     In the case of successful password resets, <see cref="MethodResult{T}.Result"/> 
        ///     will return a <see cref="HttpStatusCode.OK"/> and 
        ///     <see cref="MethodResult{T}.ResultType"/> will return a 
        ///     <see cref="ResultType.Success"/>.
        /// </returns>
        Task<MethodResult<HttpStatusCode>> AttemptPasswordReset(
            string clientId, string userName, string confirmationCode, string newPassword);
    }
}
