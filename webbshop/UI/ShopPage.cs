using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;

namespace webbshop.UI
{
    public class ShopPage : Page
    {
        public enum Buttons
        {
            Category,
            Search
        }
        public ShopPage(Product[]? products, string? searchInput = null)
        {
            var categoryPageW = new Window("(1)", 0, 0, new List<string> { "<- Gå tillbaka till kategori sidan" });
            Windows.Add(categoryPageW);

            var shopW = new Window("", 50, 0, new List<string> { "Shop sidan!" });
            Windows.Add(shopW);
            if (products == null)
            {
                var loadingW = new Window("", 50, 50, new List<string> { "Laddar..." });
                Windows.Add(loadingW);
            }
            else if (products.Length == 0)
            {
                var notFoundW = new Window("", 50, 50, new List<string> { "Inga produkter hittades :(" });
                Windows.Add(notFoundW);

            }
            else
            {
                int index = 1;
                int col = 0;
                int row = 0;

                foreach (var product in products)
                {
                    if (col % 3 == 0)
                    {
                        row++;
                        col = 0;
                    }
                    var productW = new Window($"({index + 2})", 20 + 30 * (col), 30 + 20 * row, new List<string> { product.Name, product.Price.ToString() + "kr", "Visa mer info ->" });
                    Windows.Add(productW);
                    index++;
                    col++;
                }
            }
            var searchField = new Window(searchInput == null ? "(2)" : "Sökt (2)", 50, 90, new List<string> { searchInput  == null ? "Sök bland alla produkter" : searchInput });
            Windows.Add(searchField);
            
        }
    }
}
