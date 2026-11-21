namespace Business.Constants.Aws.Cognito
{
    public static class AuthChallengeName
    {
        public const string SmsMfa = "SMS_MFA";
        public const string PasswordVerifier = "PASSWORD_VERIFIER";
        public const string NewPasswordRequired = "NEW_PASSWORD_REQUIRED";
        public const string SoftwareTokenMfa = "SOFTWARE_TOKEN_MFA";
        public const string DeviceSrpAuth = "DEVICE_SRP_AUTH";
        public const string MfaSetup = "MFA_SETUP";
    }
}
