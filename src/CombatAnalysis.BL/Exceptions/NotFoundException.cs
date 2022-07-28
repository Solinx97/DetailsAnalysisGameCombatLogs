using System;
using System.Runtime.Serialization;

namespace CombatAnalysis.BL.Exceptions
{
    [Serializable]
    internal class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, string paramName)
            : base(message)
        {
            ParamName = paramName ?? throw new ArgumentNullException(paramName);
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public string ParamName { get; set; }
    }
}
