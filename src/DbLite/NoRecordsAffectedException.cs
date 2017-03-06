using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLite
{
    public class NoRecordsAffectException : Exception
    {
        public NoRecordsAffectException() { }
        public NoRecordsAffectException(string message) : base(message) { }
        public NoRecordsAffectException(string message, Exception inner) : base(message, inner) { }
    }
}
