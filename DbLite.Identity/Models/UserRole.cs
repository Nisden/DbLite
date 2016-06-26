namespace DbLite.Identity.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserRole<TKey>
    {
        [Key]
        public TKey UserId { get; set; }

        [Key]
        public string Name { get; set; }
    }
}
