namespace API_Project.Enums
{
    public enum PasswordCheckResult
    {
        Valid,
        TooShort,
        TooLong,
        MissingUppercase,
        MissingLowercase,
        MissingDigit,
        MissingSpecialChar,
        ContainsInvalidChar
    }

    public enum EmailCheckResult
    {
        Valid,
        Empty,
        InvalidFormat
    }

    public enum PhoneCheckResult
    {
        Valid,
        NotStartWithZero,
        InvalidLength
    }

    public enum AuthResult
    {
        Success,
        InvalidCredentials,
        UserNotFound,
        EmailInvalid,
        OtpInvalid,
        OtpExpired,
        SendOtpFailed,
        ChangePasswordFailed,
        UnknownError
    }
    public enum RegisterResult
    {
        Success,
        Underage,
        InvalidEmail,
        InvalidPhone,
        InvalidPassword,
        AccountExists
    }

}
