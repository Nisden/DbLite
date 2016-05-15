namespace DbLite.Replication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class ReplicationResult
    {
        public int RecordsAdded { get; set; }

        public int RecordsUpdated { get; set; }
    }
}
