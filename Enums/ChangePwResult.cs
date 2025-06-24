namespace API_Project.Enums
{
    public enum ChangePasswordResult
    {
        Success,
        MissingInput,
        InvalidToken,
        WrongOldPassword,
        InvalidNewPassword
    }
    public enum ChangeEmailResult
    {
        SuccessSendEmail,
        OtpWrong,
        InvalidToken,
        Empty,
        InvalidFormatEmail,
        SameEmail,
        EmailAlreadyUsed,
        SuccessConfirm,
        OtpInvalid,
        OtpExpired
    }

}
