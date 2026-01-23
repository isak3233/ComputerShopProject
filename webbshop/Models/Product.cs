using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
        public string Details { get; set; }
        public virtual Supplier Supplier { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
        public int InventoryBalance { get; set; }
        
        public bool IsSelected { get; set; }

        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();

        public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    }
}
