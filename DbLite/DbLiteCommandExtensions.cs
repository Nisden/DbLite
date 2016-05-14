namespace DbLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public static class DbLiteCommandExtensions
    {
        /// <summary>
        /// Creates parameters corresponding to the properties of <see cref="parameters"/>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters">An object with properties corresponding to the parameters</param>
        /// <exception cref="ArgumentNullException">If command or parameters are null</exception>
        public static void CreateParametersFromObject(this IDbCommand command, object parameters)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var modelInfo = new DbLiteModelInfo(parameters.GetType());

            foreach (var property in modelInfo.Columns.Values)
            {
                command.CreateParameter(property.Name, property.GetValue(parameters));
            }
        }

        /// <summary>
        /// Creates a new parameter thats initialized with a name
        /// </summary>
        /// <param name="connection"></param>
        public static IDbDataParameter CreateParameterWithName(this IDbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            return CreateParameterWithName(command, null);
        }

        /// <summary>
        /// Creates a new parameter thats initialized with a name and value
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IDbDataParameter CreateParameterWithName(this IDbCommand command, object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            string parameterName = "@p" + (command.Parameters.Count + 1);
            return command.CreateParameter(parameterName, value);
        }

        /// <summary>
        /// Creates a new database parameter
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IDbDataParameter CreateParameter(this IDbCommand command, string name, object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var dialectProvider = command.Connection.GetDialectProvider();
            var parameter = command.CreateParameter();

            // Set name for the parameter
            parameter.ParameterName = name.StartsWith("@") ? name : ("@" + name);
            command.Parameters.Add(parameter);

            // Set the value of our parameter
            if (value != null)
            {
                dialectProvider.SetParameterDataType(parameter, value);
                parameter.Value = value;
            }
            else
            {
                parameter.Value = DBNull.Value;
            }

            return parameter;
        }
    }
}
