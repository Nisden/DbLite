namespace DbLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class DbLiteTransaction : DbTransaction, IDbTransaction
    {
        public override IsolationLevel IsolationLevel
        {
            get
            {
                return transaction.IsolationLevel;
            }
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return (DbConnection)transaction.Connection;
            }
        }

        public event EventHandler Finished;
        private void OnFinished()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private readonly IDbTransaction transaction;
        public IDbTransaction InnerTransaction
        {
            get
            {
                return transaction;
            }
        }

        public DbLiteTransaction(IDbTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            this.transaction = transaction;
        }

        public override void Commit()
        {
            OnFinished();
            transaction.Commit();
        }

        public override void Rollback()
        {
            OnFinished();
            transaction.Rollback();
        }

        protected override void Dispose(bool disposing)
        {
            OnFinished();
            transaction.Dispose();

            base.Dispose(disposing);
        }
    }
}
