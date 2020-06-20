using System;
using System.Runtime.Serialization;

namespace ObjectConfig.Exceptions
{
    [Serializable]
    public class ForbidenException
        : ObjectConfigBaseException
    {
        public ForbidenException()
        {
        }

        public ForbidenException(string message)
            : base(message)
        {
        }

        public ForbidenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ForbidenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
