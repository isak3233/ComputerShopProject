using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.Models
{
    public class CartProducts
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
