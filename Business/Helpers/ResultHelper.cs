using Amazon.CognitoIdentityProvider.Model;
using System.Net;

namespace Business.Helpers
{
    // TODO: Convert the rest of the code to use the methods here
    public static class ResultHelper
    {
        public static HttpStatusCode ExceptionToHttpStatusCode(string exceptionName)
            => exceptionName switch
            {
                nameof(LimitExceededException) => HttpStatusCode.Forbidden,
                nameof(ResourceNotFoundException) => HttpStatusCode.NotFound,
                nameof(TooManyFailedAttemptsException) => HttpStatusCode.TooManyRequests,
                nameof(UserNotFoundException) => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };
    }
}
