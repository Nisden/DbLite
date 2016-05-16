namespace DbLite.Replication
{
    using System;
    using System.Collections.Generic;
    using System.Data;
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

        public abstract ReplicationResult Run(IDbConnection sourceConnection);
    }

    public sealed class TableReplication<TTable> : TableReplication
        where TTable : class, IReplicatedTable, new()
    {
        // TODO: Change this to some sort of "Shared" table
        private Dictionary<string, DateTime> lastReplicated = new Dictionary<string, DateTime>();

        /// <exception cref="InvalidReplicationOptionException">If one or more options are incorrect</exception>
        public TableReplication(ReplicationOptions options) : base(options)
        { }

        public override ReplicationResult Run(IDbConnection sourceConnection)
        {
            var result = new ReplicationResult();
            var modelInfo = DbLiteModelInfo<TTable>.Instance;

            // Open connection and get the dialect
            using (var connection = Options.DestinationConnection())
            {
                var dialectProvider = connection.GetDialectProvider();

                // Do we have a lastReplicated date?
                foreach (var instanceName in Options.InstanceNames)
                {
                    if (!lastReplicated.ContainsKey(instanceName))
                    {
                        // If not, lets get current highest
                        var record = connection.Single<TTable>($@"SELECT TOP 1 {dialectProvider.EscapeColumn(modelInfo.Columns[nameof(IReplicatedTable.LastUpdated)].Name)}
                                                                   FROM {dialectProvider.EscapeTable(modelInfo.Name)}
                                                                   WHERE {dialectProvider.EscapeColumn(modelInfo.Columns[nameof(IReplicatedTable.Source)].Name)} = @source
                                                                   ORDER BY {dialectProvider.EscapeColumn(modelInfo.Columns[nameof(IReplicatedTable.LastUpdated)].Name)} DESC",
                                                                   new { source = instanceName });

                        if (record == null)
                        {
                            // Never seen before
                            lastReplicated.Add(instanceName, new DateTime(1900, 1, 1)); // Prevents MSSQL Underflow if table is created using DateTime

                        }
                        else
                        {
                            lastReplicated.Add(instanceName, record.LastUpdated);
                        }
                    }
                }

                // Lets get the newest records that needs to be synchronized
                List<TTable> records;
                using (var command = sourceConnection.CreateCommand())
                {
                    // Generate a where statement similar to:
                    // (Source = DB1 AND LastUpdated > xx-yy-zzzz) OR (Source = DB2 AND LastUpdated > xx-yy-zzzz)
                    string whereSql = string.Join(" OR ", Options.InstanceNames.Select(instanceName => $"({dialectProvider.EscapeColumn(modelInfo.Columns[nameof(IReplicatedTable.Source)].Name)} = {command.CreateParameterWithName(instanceName).ParameterName} AND {dialectProvider.EscapeColumn(modelInfo.Columns[nameof(IReplicatedTable.LastUpdated)].Name)} > {command.CreateParameterWithName(lastReplicated[instanceName])})"));

                    // Gets the "oldsest" records that needs syncroniation
                    command.CommandText = $@"SELECT TOP {Options.BatchSize} {string.Join(", ", modelInfo.Columns.Values.Select(x => dialectProvider.EscapeColumn(x.Name)))}
                                             FROM {dialectProvider.EscapeTable(modelInfo.Name)}
                                             WHERE {whereSql}
                                             ORDER BY {dialectProvider.EscapeColumn(modelInfo.Columns[nameof(IReplicatedTable.LastUpdated)].Name)} DESC";

                    using (var reader = command.ExecuteReader())
                    {
                        records = reader.ToList<TTable>();
                    }
                }

                if (records.Count > 0)
                {
                    // Update records
                    foreach (var record in records)
                    {
                        try
                        {
                            connection.Update(record);
                            result.RecordsUpdated++;
                            lastReplicated[record.Source] = record.LastUpdated;
                        }
                        catch (NoRecordsAffectException)
                        {
                            connection.Insert(record);
                            result.RecordsAdded++;
                        }
                    }
                }
            }

            return result;
        }
    }
}
