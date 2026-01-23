using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.Models
{
    public class PaymentHistory
    {
        public int Id { get; set; }
        public virtual PaymentOption PaymentOption { get; set; }
        public int PaymentOptionId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public string DeliveryStreet { get; set; }
        public virtual City DeliveryCity { get; set; }
        public int DeliveryCityId { get; set; }

        public virtual DeliveryOption DeliveryOption {  get; set; }
        public int DeliveryOptionId { get; set; }
        public DateTime PayedDate { get; set; }
        public DateTime SendDate { get; set; }


    }
}
