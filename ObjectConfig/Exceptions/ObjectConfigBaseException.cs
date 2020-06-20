using System;
using System.Runtime.Serialization;

namespace ObjectConfig.Exceptions
{
    [Serializable]
    public abstract class ObjectConfigBaseException
        : Exception
    {
        protected ObjectConfigBaseException()
        {
        }

        protected ObjectConfigBaseException(string message)
            : base(message)
        {
        }

        protected ObjectConfigBaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ObjectConfigBaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
