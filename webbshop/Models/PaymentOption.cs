using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.Models
{
    public class PaymentOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();   
    }
}
