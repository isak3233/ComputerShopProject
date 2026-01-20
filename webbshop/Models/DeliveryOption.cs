using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.Models
{
    public class DeliveryOption
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();
    }
}
