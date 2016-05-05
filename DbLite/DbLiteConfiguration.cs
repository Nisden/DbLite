namespace DbLite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class DbLiteConfiguration
    {
        public static event EventHandler<DbLiteExecutionEventArgs> BeforeSelect;
        internal static void OnBeforeSelect(object sender, DbLiteExecutionEventArgs args)
        {
            if (BeforeSelect != null)
                BeforeSelect(sender, args);
        }

        public static event EventHandler<DbLiteExecutionEventArgs> BeforeUpdate;
        internal static void OnBeforeUpdate(object sender, DbLiteExecutionEventArgs args)
        {
            if (BeforeUpdate != null)
                BeforeUpdate(sender, args);
        }

        public static event EventHandler<DbLiteExecutionEventArgs> BeforeDelete;
        internal static void OnBeforeDelete(object sender, DbLiteExecutionEventArgs args)
        {
            if (BeforeDelete != null)
                BeforeDelete(sender, args);
        }
    }
}
