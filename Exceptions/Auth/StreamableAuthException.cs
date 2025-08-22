using StreamableNet.Exceptions.Base;

namespace StreamableNet.Exceptions.Auth
{
    [Serializable]
    public class StreamableAuthException : StreamableException
    {
        public StreamableAuthException(string message, ErrorCode errorCode) : base(message, errorCode)
        {
        }

        public StreamableAuthException(string message, ErrorCode errorCode, Exception innerException)
            : base(message, errorCode, innerException)
        {
        }

    }
}
