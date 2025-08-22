namespace StreamableNet.Exceptions.Base
{
    public enum ErrorCode
    {
        // General
        Unknown = 0,
        EmptyResponse = 1001,
        InvalidResponse = 1002,
        NetworkError = 1003,

        // Authentication
        AuthenticationFailed = 2001,
        InvalidCredentials = 2002,
    }
}
