using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Country Country { get; set; }
        public int CountryId { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();
    }
}
