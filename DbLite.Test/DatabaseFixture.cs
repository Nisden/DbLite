namespace DbLite.Test
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public abstract class DatabaseFixture<TConnection> : IDisposable
        where TConnection : IDbConnection
    {
        public DatabaseFixture()
        {
            using (Open())
            { }
        }

        protected abstract void SetupDatabase(string databaseName);

        public abstract TConnection Open(string namedDatabase = "Default");

        public abstract void Dispose();
    }
}
