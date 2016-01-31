namespace DbLite.Query
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public class DbLiteQueryProviderCommon : DbLiteQueryProvider
    {
        public DbLiteQueryProviderCommon(IDbConnection connection) : base(connection)
        { }

        public override object Execute(Expression expression)
        {
            string queryText = GetQueryText(expression);

            throw new NotImplementedException();
        }

        public override string GetQueryText(Expression expression)
        {
            var methodExpression = (MethodCallExpression)expression;
            
            throw new NotImplementedException();
        }
    }
}
