namespace DbLite.Test.Tables
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SimpleTable
    {
        public string String1 { get; set; }

        public string String2 { get; set; }

        public bool Boolean1 { get; set; }

        [Key]
        public int Interger1 { get; set; }

        public Decimal Decimal1 { get; set; }

    }
}
