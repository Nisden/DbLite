namespace DbLite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class DbLiteSQLBuilder
    {
        /// <summary>
        /// Takes a partial select statement and transforms it into a Full select statement for the <see cref="TModel"/> type.
        /// </summary>
        /// <typeparam name="TModel">Type of the table that the select statement applies to</typeparam>
        /// <param name="dialectProvider">The dialect provider used to create the statement</param>
        /// <param name="sql">The partial or full select statement</param>
        /// <returns>A full select statement based on <see cref="sql"/></returns>
        public static string ToFullSelect<TModel>(DbLiteDialectProvider dialectProvider, string sql)
        {
            if (dialectProvider == null)
                throw new ArgumentNullException(nameof(dialectProvider));

            var modelInfo = DbLiteModelInfo<TModel>.Instance;

            if (sql == null)
                sql = string.Empty; // start from scratch

            // Is it already a full statement?
            if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                return sql;

            // Is its for a procedure
            if (sql.StartsWith("EXEC", StringComparison.OrdinalIgnoreCase))
                return sql;

            // Add missing WHERE if required
            if (!sql.StartsWith("WHERE", StringComparison.OrdinalIgnoreCase) && !sql.StartsWith("FROM", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    sql = "WHERE " + sql;
                }
            }

            // Add missing FROM if required
            if (string.IsNullOrWhiteSpace(sql) || sql.StartsWith("WHERE", StringComparison.OrdinalIgnoreCase))
            {
                sql = $"FROM {dialectProvider.EscapeTable(modelInfo.Name)}" + Environment.NewLine + sql;
            }
            
            // Add missing select if required
            if (sql.StartsWith("FROM", StringComparison.OrdinalIgnoreCase))
            {
                sql = "SELECT " + string.Join(", ", modelInfo.Columns.Select(x => dialectProvider.EscapeColumn(x.Key))) + Environment.NewLine + sql;
            }

            return sql;
        }
    }
}
