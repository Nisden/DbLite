namespace DbLite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    public class DialectProviderNotRegisteredException : Exception
    {
        public DialectProviderNotRegisteredException() { }
        public DialectProviderNotRegisteredException(string message) : base(message) { }
        public DialectProviderNotRegisteredException(string message, Exception inner) : base(message, inner) { }
        protected DialectProviderNotRegisteredException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
