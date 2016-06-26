namespace DbLite.Identity.Models
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class User : User<Guid>
    { }

    public class User<TKey> : IUser<TKey>
    {
        [Key]
        public TKey Id { get; set; }

        public string UserName { get; set; }
    }
}
