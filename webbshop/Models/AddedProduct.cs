using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.Models
{
    internal class AddedProduct
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int ProductId { get; set; }
        public int AdminId { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public int? ChangeAdminId { get; set; }
    }
}
