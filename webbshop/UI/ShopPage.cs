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
            Search,
            ShowMore = 11,
            ShowLess = 12,
        }
        public ShopPage()
        {
            Update();
        }
        public void Update(Product[]? products = null, string? searchInput = null, int indexOn = 0)
        {
            Windows = new List<Window>();
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
                var showMoreW = new Window("(12)", 90, 80, new List<string> { "Gå fram i produkter ->" });
                var showLessW = new Window("(13)", 30, 80, new List<string> { "<- Gå tillbaka i produkter" });
                var windowsToAdd = new List<Window>();
                foreach(var product in products)
                {
                    var productW = new Window("", 0, 0, new List<string> { product.Name, product.Price.ToString() + "kr", "Visa mer info ->" });
                    windowsToAdd.Add(productW);
                }
                int amountOfCols = 3;
                int amountOfRows = 3;
                int startX = 20;
                int startY = 30;
                int xPerWindow = 30;
                int yPerWindow = 20;
                int choiceStart = 3;
                AddDynamicWindows(indexOn, windowsToAdd, showMoreW, showLessW, amountOfCols, amountOfRows, startX, startY, xPerWindow, yPerWindow, choiceStart);
                

            }
            var searchField = new Window(searchInput == null ? "(2)" : "Sökt (2)", 90 , 0, new List<string> { searchInput == null ? "Sök bland alla produkter" : searchInput });
            Windows.Add(searchField);
            this.Render();
        }
    }
}
