using StreamableNet.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamableNet.Exceptions.Auth
{
    [Serializable]
    public class StreamableAuthException :StreamableException
    {
        protected StreamableAuthException(string message, ErrorCode errorCode) :base(message, errorCode)
        {
        }

        protected StreamableAuthException(string message, ErrorCode errorCode, Exception innerException)
            : base(message, errorCode, innerException)
        {
        }

    }
}
