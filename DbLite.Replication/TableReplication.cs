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

        public int BatchSize { get; set; } = 5000;

        /// <exception cref="InvalidReplicationOptionException">If one or more options are incorrect</exception>
        public TableReplication(ReplicationOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Validate();

            Options = options;
        }

        public abstract ReplicationResult Run();
    }

    public sealed class TableReplication<TTable> : TableReplication
        where TTable : class, IReplicatedTable, new()
    {
        private Dictionary<string, DateTime> lastReplicated = new Dictionary<string, DateTime>();

        /// <exception cref="InvalidReplicationOptionException">If one or more options are incorrect</exception>
        public TableReplication(ReplicationOptions options) : base(options)
        { }

        public override ReplicationResult Run()
        {
            var result = new ReplicationResult();
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
                        var record = connection.Single<TTable>($@"SELECT TOP 1 {dialectProvider.EscapeColumn(modelInfo.Columns[nameof(IReplicatedTable.LastUpdated)].Name)}
                                                                   FROM {dialectProvider.EscapeTable(modelInfo.Name)}
                                                                   WHERE {dialectProvider.EscapeColumn(modelInfo.Columns[nameof(IReplicatedTable.Source)].Name)} == @source
                                                                   ORDER BY {dialectProvider.EscapeColumn(modelInfo.Columns[nameof(IReplicatedTable.LastUpdated)].Name)} DESC", 
                                                                   new { source = instanceName });

                        lastReplicated.Add(instanceName, record.LastUpdated);
                    }

                    // Lets get the newest records that needs to be synchronized
                    var records = connection.Select<TTable>($@"SELECT TOP {BatchSize} {string.Join(", ", modelInfo.Columns.Values.Select(x => dialectProvider.EscapeColumn(x.Name)))}
                                                               FROM {dialectProvider.EscapeTable(modelInfo.Name)}
                                                               WHERE {dialectProvider.EscapeColumn(modelInfo.Columns[nameof(IReplicatedTable.Source)].Name)} > @last
                                                               ORDER BY {dialectProvider.EscapeColumn(modelInfo.Columns[nameof(IReplicatedTable.LastUpdated)].Name)} DESC",
                                                               new { last = lastReplicated[instanceName] });

                    if (records.Count > 0)
                    {
                        // Update records
                        foreach (var record in records)
                        {
                            try
                            {
                                connection.Update(record);
                                result.RecordsUpdated++;
                            }
                            catch (NoRecordsAffectException)
                            {
                                connection.Insert(record);
                                result.RecordsAdded++;
                            }
                        }

                        lastReplicated[instanceName] = records.Last().LastUpdated;
                    }
                }
            }

            return result;
        }
    }
}
