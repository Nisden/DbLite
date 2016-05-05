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
        where TTable : class
    {
        /// <exception cref="InvalidReplicationOptionException">If one or more options are incorrect</exception>
        public TableReplication(ReplicationOptions options) : base(options)
        { }

        public override void Run()
        {

        }
    }
}
