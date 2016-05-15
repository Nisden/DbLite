namespace DbLite.Test.Tables
{
    using DbLite.Replication;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ReplicationTestTable : IReplicatedTable
    {
        [Key]
        public Guid Id { get; set; }

        public string Value1 { get; set; }

        public string Value2 { get; set; }

        public string Value3 { get; set; }

        public bool Deleted { get; set; }

        public DateTime LastUpdated { get; set; }

        public string Source { get; set; }
    }
}
