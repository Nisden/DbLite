namespace DbLite.Test
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public abstract class DatabaseFixture<TConnection>
        where TConnection : IDbConnection
    {
        public DatabaseFixture()
        {
            SetupDatabase();
        }

        protected abstract void SetupDatabase();

        public abstract TConnection Open();
    }
}
