namespace DbLite.Replication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class TableReplication
    {
        public ReplicationOptions Options { get; private set; }

        /// <exception cref="InvalidReplicationOptionException">If one or more options are incorrect</exception>
        public TableReplication(ReplicationOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Validate();

            Options = options;
        }

        public abstract void Run();
    }

    public sealed class TableReplication<TTable> : TableReplication
        where TTable : class, IReplicatedTable, new()
    {
        private Dictionary<string, DateTime> lastReplicated = new Dictionary<string, DateTime>();

        /// <exception cref="InvalidReplicationOptionException">If one or more options are incorrect</exception>
        public TableReplication(ReplicationOptions options) : base(options)
        { }

        public override void Run()
        {
            var modelInfo = DbLiteModelInfo<TTable>.Instance;

            // Loop instances
            foreach (var instanceName in Options.InstanceNames)
            {
                // Open connection and get the dialect
                using (var connection = Options.OpenConnection())
                {
                    var dialectProvider = connection.GetDialectProvider();

                    // Do we have a lastReplicated date?
                    if (!lastReplicated.ContainsKey(instanceName))
                    {
                        // If not, lets get current highest
                        var records = connection.Select<TTable>($"WHERE {modelInfo.Columns[nameof(IReplicatedTable.Source)].Name} == @source", new { source = instanceName });
                    }
                }
            }
        }
    }
}
