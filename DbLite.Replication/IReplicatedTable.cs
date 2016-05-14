namespace DbLite.Replication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IReplicatedTable
    {
        bool Deleted { get; set; }

        string Source { get; set; }

        DateTime LastUpdated { get; set; }
    }
}
