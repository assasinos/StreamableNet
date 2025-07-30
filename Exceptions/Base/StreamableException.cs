using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamableNet.Exceptions.Base
{
    [Serializable]
    public class StreamableException : Exception
    {
        public ErrorCode ErrorCode { get; }
        public string RequestId { get; set; }

        public StreamableException(string message, ErrorCode errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public StreamableException(string message, ErrorCode errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
