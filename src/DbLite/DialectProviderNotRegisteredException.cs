namespace DbLite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DialectProviderNotRegisteredException : Exception
    {
        public DialectProviderNotRegisteredException() { }
        public DialectProviderNotRegisteredException(string message) : base(message) { }
        public DialectProviderNotRegisteredException(string message, Exception inner) : base(message, inner) { }
    }
}
