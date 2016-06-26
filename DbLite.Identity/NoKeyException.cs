namespace DbLite.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    public class NoKeyException : Exception
    {
        public NoKeyException() { }
        public NoKeyException(string message) : base(message) { }
        public NoKeyException(string message, Exception inner) : base(message, inner) { }
        protected NoKeyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
