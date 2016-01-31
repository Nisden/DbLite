namespace DbLite
{
    using System;
    using System.Data;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public static class DbLiteDialectProviderFactory
    {
        private static ConcurrentDictionary<string, DbLiteDialectProvider> dialectProviders = new ConcurrentDictionary<string, DbLiteDialectProvider>();

        static DbLiteDialectProviderFactory()
        {
            // A list of the Official DbLite Providers
            KeyValuePair<string, string>[] officialProviders = new KeyValuePair<string, string>[] 
            {
                new KeyValuePair<string, string>("System.Data.SQLite.SQLiteConnection", "DbLite.SQLite.DbLiteSQLiteDialectProvider, DbLite.SQLite")
            };

            // Auto Registration of Provider Types
            foreach (var item in officialProviders)
            {
                var providerType = Type.GetType(item.Value);
                if (providerType != null)
                {
                    RegisterProvider(item.Key, (DbLiteDialectProvider)Activator.CreateInstance(providerType));
                }
            }
        }

        /// <summary>
        /// Gets a dialect provider based on the <see cref="connection"/>
        /// </summary>
        /// <param name="connection">Connection to get a dialect provider from</param>
        public static DbLiteDialectProvider GetProvider(IDbConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            var connectionType = connection.GetType();

            DbLiteDialectProvider provider;
            if (!dialectProviders.TryGetValue(connectionType.FullName, out provider))
            {
                throw new DialectProviderNotRegisteredException($"No dialect provider with the name \"{connectionType.FullName}\" are registered.");
            }

            return provider;
        }

        /// <summary>
        /// Registeres a new Dialect Provider.
        /// </summary>
        /// <param name="name">The full name of the Connection Type, could be "System.Data.SqlClient.SqlConnection"</param>
        /// <param name="provider">The dialect provider for this instance</param>
        public static void RegisterProvider(string name, DbLiteDialectProvider provider)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            // Add or replace the existing provider
            dialectProviders.AddOrUpdate(name, provider, (x, old) =>
            {
                return provider;
            });
        }
    }
}
