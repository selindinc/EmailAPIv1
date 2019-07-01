using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailApi.Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public IList<Mail> Mails { get; set; } = new List<Mail>();
    }
}
