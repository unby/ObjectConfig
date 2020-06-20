using System;
using System.Runtime.Serialization;

namespace ObjectConfig.Exceptions
{
    [Serializable]
    public class EntityException
        : ObjectConfigBaseException
    {
        public EntityException()
        {
        }

        public EntityException(string message)
            : base(message)
        {
        }

        public EntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EntityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
