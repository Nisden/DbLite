namespace DbLite.Replication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    public class InvalidReplicationOptionException : Exception
    {
        public InvalidReplicationOptionException() { }
        public InvalidReplicationOptionException(string message) : base(message) { }
        public InvalidReplicationOptionException(string message, Exception inner) : base(message, inner) { }
        protected InvalidReplicationOptionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
