namespace DbLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DbLiteExecutionEventArgs : EventArgs
    {
        public IDbConnection Connection { get; set; }

        public IDbCommand Command { get; set; }
    }
}
