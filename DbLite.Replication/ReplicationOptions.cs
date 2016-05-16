namespace DbLite.Replication
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class ReplicationOptions
    {
        public string[] InstanceNames { get; set; }

        public Func<IDbConnection> DestinationConnection { get; set; }

        public int BatchSize { get; set; } = 5000;

        /// <summary>
        /// Validates the options defined.
        /// </summary>
        /// <exception cref="InvalidReplicationOptionException">If one or more options are incorrect</exception>
        public void Validate()
        {
            if (InstanceNames == null || InstanceNames.Length == 0)
                throw new InvalidReplicationOptionException("Replication Options must have one or more InstanceNames defined");

            if (DestinationConnection == null)
                throw new InvalidReplicationOptionException("DestinationConnection must be defined");

            if (BatchSize <= 0)
                throw new InvalidReplicationOptionException("BatchSize must be larger then 0");
        }
    }
}
