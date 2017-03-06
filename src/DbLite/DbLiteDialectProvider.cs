namespace DbLite
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class DbLiteDialectProvider
    {
        public abstract bool SupportsMultipleInsertValues
        {
            get;
        }

        public abstract string RetriveLastIdentity
        {
            get;
        }

        private readonly ConcurrentDictionary<IDbConnection, DbLiteTransaction> openTransactions = new ConcurrentDictionary<IDbConnection, DbLiteTransaction>();

        public DbLiteDialectProvider()
        {

        }

        public abstract string EscapeTable(string tableName);

        public abstract string EscapeColumn(string columnName);

        public virtual void SetParameterDataType(IDbDataParameter parameter, object value)
        {
            if (value == null)
            {
                parameter.DbType = DbType.Object;
            }
            if (value is string)
            {
                parameter.DbType = DbType.String;
            }
            else if (value is Boolean)
            {
                parameter.DbType = DbType.Boolean;
            }
            else if (value is DateTime)
            {
                parameter.DbType = DbType.DateTime;
            }
            else if (value is Int16)
            {
                parameter.DbType = DbType.Int16;
            }
            else if (value is Int32)
            {
                parameter.DbType = DbType.Int32;
            }
            else if (value is Int64)
            {
                parameter.DbType = DbType.Int64;
            }
            else if (value is Decimal)
            {
                parameter.DbType = DbType.Decimal;
            }
            else
            {
                throw new NotImplementedException($"DataType for {value.GetType().FullName} is not set");
            }
        }

        public virtual DbLiteTransaction OpenTransaction(IDbConnection connection)
        {
            var transaction = new DbLiteTransaction(connection.BeginTransaction());
            transaction.Finished += TransactionFinished;

            if (!openTransactions.TryAdd(connection, transaction))
                throw new InvalidOperationException("You cannot open an transaction on the same connection twice");

            return transaction;
        }

        public virtual DbLiteTransaction OpenTransaction(IDbConnection connection, IsolationLevel isolationLevel)
        {
            var transaction = new DbLiteTransaction(connection.BeginTransaction(isolationLevel));
            transaction.Finished += TransactionFinished;

            if (!openTransactions.TryAdd(connection, transaction))
                throw new InvalidOperationException("You cannot open an transaction on the same connection twice");

            return transaction;
        }

        private void TransactionFinished(object sender, EventArgs args)
        {
            DbLiteTransaction currentTransaction = (DbLiteTransaction)sender;

            // Remove from list something
            try
            {
                DbLiteTransaction removedTransaction;
                openTransactions.TryRemove(currentTransaction.Connection, out removedTransaction);
            }
            finally
            {
                currentTransaction.Finished -= TransactionFinished;
            }
        }

        public virtual DbLiteTransaction GetOpenTransaction(IDbConnection connection)
        {
            DbLiteTransaction openTransaction;
            openTransactions.TryGetValue(connection, out openTransaction);

            return openTransaction;
        }
    }
}
