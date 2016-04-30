﻿namespace DbLite
{
    using Query;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class DbLiteConnectionExtensions
    {
        /// <summary>
        /// Gets the current DialectProvider for this connection
        /// </summary>
        /// <param name="connection">Connection to get the dialect provider from</param>
        public static DbLiteDialectProvider GetDialectProvider(this IDbConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            return DbLiteDialectProviderFactory.GetProvider(connection);
        }

        #region Insert

        public static void Insert<TModel>(this IDbConnection connection, params TModel[] items)
        {
            DbLiteDialectProvider dialectProvider = DbLiteDialectProviderFactory.GetProvider(connection);
            DbLiteModelInfo modelInfo = DbLiteModelInfo<TModel>.Instance;

            if (dialectProvider.SupportsMultipleInsertValues)
            {
                using (var command = connection.CreateCommand())
                {
                    // Create the top part of our insert statement
                    command.CommandText = $"INSERT INTO {dialectProvider.EscapeTable(modelInfo.Name)} (";
                    command.CommandText += string.Join(", ", modelInfo.Columns.Select(x => dialectProvider.EscapeColumn(x.Key)));
                    command.CommandText += ") VALUES ";

                    // Create the value lines
                    var valueLines = items.Select(item =>
                    {
                        // Create a parameter for each column, the CreateParameterWithName also sets the correct value for the parameter
                        var parametersForLine = modelInfo.Columns.Values.Select(column => command.CreateParameterWithName(column.GetValue(item))).ToArray();

                        // Now build the line of "values"
                        return "(" + string.Join(", ", parametersForLine.Select(x => x.ParameterName)) + ")";
                    }).ToArray();

                    // Combine all the value lines and append it to the command
                    command.CommandText += string.Join(", ", valueLines);

                    // Command is all ready and loaded now, lets go ahead and call it
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                // Fallback if the provider don't support multiple values in a single insert
                foreach (var item in items)
                {
                    connection.Insert(item);
                }
            }
        }

        #endregion

        #region Select / Single

        public static List<TModel> Select<TModel>(this IDbConnection connection, string sql, object parameters)
            where TModel : class, new()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = DbLiteSQLBuilder.ToFullSelect<TModel>(connection.GetDialectProvider(), sql);

                if (parameters != null)
                {
                    command.CreateParametersFromObject(parameters);
                }

                using (var reader = command.ExecuteReader())
                {
                    return reader.ToList<TModel>();
                }
            }
        }

        public static TModel Single<TModel>(this IDbConnection connection, string sql, object parameters)
            where TModel : class, new()
        {
            return connection.Select<TModel>(sql, parameters).FirstOrDefault();
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates a record in the database
        /// </summary>
        /// <typeparam name="TModel">Type of the record</typeparam>
        /// <param name="connection">Database connection</param>
        /// <param name="item">Item to be updated in the database</param>
        /// <exception cref="InvalidOperationException">If no columns on the <see cref="item"/> has the <see cref="KeyAttribute"/></exception>
        public static void Update<TModel>(this IDbConnection connection, TModel item)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var dialectProvider = connection.GetDialectProvider();
            var modelInfo = DbLiteModelInfo<TModel>.Instance;

            // Ensure that the model has keys
            if (!modelInfo.Columns.Values.Any(column => column.Key))
                throw new InvalidOperationException("No column has the KeyAttribute, atleast one column must have a KeyAttribute to use the Update method");

            using (var command = connection.CreateCommand())
            {
                command.CreateParametersFromObject(item);

                command.CommandText = "UPDATE " + dialectProvider.EscapeColumn(modelInfo.Name);
                command.CommandText += " SET ";
                command.CommandText += string.Join(", ", modelInfo.Columns.Keys.Select(columnName => dialectProvider.EscapeColumn(columnName) + " = @" + columnName));
                command.CommandText += " WHERE " + string.Join(" AND ", modelInfo.Columns.Values.Where(column => column.Key).Select(column => dialectProvider.EscapeColumn(column.Name) + " = @" + column.Name));

                if (command.ExecuteNonQuery() == 0)
                    throw new NoRecordsAffectException();
            }
        }

        #endregion

        #region Delete

        public static void Delete<TModel>(this IDbConnection connection, TModel item)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var dialectProvider = connection.GetDialectProvider();
            var modelInfo = DbLiteModelInfo<TModel>.Instance;

            // Ensure that the model has keys
            if (!modelInfo.Columns.Values.Any(column => column.Key))
                throw new InvalidOperationException("No column has the KeyAttribute, atleast one column must have a KeyAttribute to use this method");

            using (var command = connection.CreateCommand())
            {
                command.CreateParametersFromObject(item);

                command.CommandText = "DELETE FROM " + dialectProvider.EscapeColumn(modelInfo.Name);
                command.CommandText += " WHERE " + string.Join(" AND ", modelInfo.Columns.Values.Where(column => column.Key).Select(column => dialectProvider.EscapeColumn(column.Name) + " = @" + column.Name));

                if (command.ExecuteNonQuery() == 0)
                    throw new NoRecordsAffectException();
            }
        }

        #endregion

        #region Query

        public static DbLiteQuery<TModel> Query<TModel>(this IDbConnection connection)
        {
            var dialectProvider = connection.GetDialectProvider();

            return new DbLiteQuery<TModel>(dialectProvider.CreateQueryProvider(connection));
        }

        #endregion

        #region Transaction

        /// <summary>
        /// Begins an tranasaction that DbLite automaticly uses.
        /// </summary>
        public static IDbTransaction OpenTransaction(this IDbConnection connection)
        {
            return connection.BeginTransaction();
        }

        /// <summary>
        /// Begins an tranasaction that DbLite automaticly uses.
        /// </summary>
        public static IDbTransaction OpenTransaction(this IDbConnection connection, IsolationLevel isolationLevel)
        {
            return connection.BeginTransaction(isolationLevel);
        }

        #endregion
    }
}