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
        private readonly Lazy<TConnection> db;
        public TConnection Db
        {
            get
            {
                return db.Value;
            }
        }

        public DatabaseFixture()
        {
            db = new Lazy<TConnection>(Open);
            SetupDatabase();
        }

        protected abstract void SetupDatabase();

        protected abstract TConnection Open();

        public void Dispose()
        {
            if (db.IsValueCreated)
                db.Value.Dispose();
        }
    }
}
