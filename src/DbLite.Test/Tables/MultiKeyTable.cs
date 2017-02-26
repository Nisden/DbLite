namespace DbLite.Test.Tables
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MultiKeyTable
    {
        [Key]
        public int Id1 { get; set; }

        [Key]
        public string Id2 { get; set; }

        public string Value { get; set; }
    }
}
