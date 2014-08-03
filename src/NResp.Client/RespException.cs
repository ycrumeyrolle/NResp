namespace NResp.Client
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Default ctor is not used.")]
    [Serializable]
    public class RespException : Exception
    {
        public RespException(string message)
            : base(message)
        {
        }

        public RespException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RespException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
