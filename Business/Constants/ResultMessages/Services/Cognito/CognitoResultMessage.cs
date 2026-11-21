namespace Business.Constants.ResultMessages.Services.Cognito
{
    public static class CognitoResultMessage
    {
        public const string AdditionalSteps = "Additional steps must be taken to authenticate " +
            "log-in.";
        public const string SuccessfulLoginAuth = "Log-in was successful.";
        public const string InvalidLogin = "The user name and password combination is invalid.";
        public const string PassResetRequired = "The password must be reset.";
        public const string UserNotConfirmed = "The newly-registered account must first be confirmed.";
        public const string ConfirmationSent = "A confirmation code has been sent to the user's " +
            "email address associated with their account.";
        public const string PasswordResetProhibited = "Password reset is forbidden.";
        public const string PasswordResetSuccess = "Successfully reset password.";
        public const string PasswordResetFail = "Failed to reset password.";
        public const string CodeMismatch = "The confirmation code provided was incorrect.";
        public const string CodeExpired = "The confirmation code provided has expired.";
        public const string InvalidNewPassword = "The new password provided is invalid.";
        public const string UserNotFound = "The user can't be found.";
    }
}
