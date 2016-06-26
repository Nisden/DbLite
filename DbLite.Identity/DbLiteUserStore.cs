namespace DbLite.Identity
{
    using DbLite.Identity.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DbLiteUserStore : DbLiteUserStore<User, Guid>
    {
        public DbLiteUserStore(DbConnection connection) : base(connection)
        { }
    }

    public class DbLiteUserStore<TUser, TKey> : IUserStore<TUser, TKey>, IUserRoleStore<TUser, TKey>, IDisposable
        where TUser : User<TKey>, new()
    {
        private readonly DbConnection connection;
        public DbConnection Connection
        {
            get
            {
                if (disposedValue)
                    throw new ObjectDisposedException(nameof(DbLiteUserStore));

                return connection;
            }
        }

        public bool DisposeConnection { get; set; } = true;

        public DbLiteUserStore(DbConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            this.connection = connection;
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public Task CreateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (EqualityComparer<TKey>.Default.Equals(user.Id))
                throw new ArgumentNullException(nameof(user.Id), "Id can not be null or default");

            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new ArgumentException("UserName must have a value", nameof(user.UserName));

            return Task.FromResult(Connection.Insert(user));
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public Task DeleteAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (EqualityComparer<TKey>.Default.Equals(user.Id))
                throw new ArgumentNullException(nameof(user.Id), "Id can not be null or default");

            try
            {
                Connection.Delete(user);
            }
            catch (NoRecordsAffectException ex)
            {
                throw new UserNotFoundException($"User with the Id {user.Id} was not found", ex);
            }

            return Task.Delay(0);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NoKeyException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public Task<TUser> FindByIdAsync(TKey userId)
        {
            if (EqualityComparer<TKey>.Default.Equals(userId))
                throw new ArgumentNullException(nameof(userId), "Id can not be null or default");

            var keyColumn = DbLiteModelInfo<TUser>.Instance.Columns.Values.Where(x => x.Key).FirstOrDefault();
            if (keyColumn == null)
                throw new NoKeyException("The model does not have a column with the Key Attribute");

            var user = Connection.Single<TUser>($"WHERE {keyColumn.Name} = @userId", new { userId = userId });
            if (user == null)
                throw new UserNotFoundException($"User with the Id {userId} was not found");

            return Task.FromResult(user);
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    Connection.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DbLiteUserStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
