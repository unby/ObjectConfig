using System;
using System.Runtime.Serialization;

namespace ObjectConfig.Exceptions
{
    [Serializable]
    public class OperationException : ObjectConfigBaseException
    {
        public OperationException()
        {
        }

        public OperationException(string message) : base(message)
        {
        }

        public OperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OperationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
