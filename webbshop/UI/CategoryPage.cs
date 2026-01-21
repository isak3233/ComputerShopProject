using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;

namespace webbshop.UI
{
    internal class CategoryPage : Page
    {
        public enum Buttons
        {
            HomePage,
            Category1,
            Category2,
            Category3,
        }
        public CategoryPage(Category[] categories)
        {
   
            var backToHomePage = new Window("(1)", 0, 0, new List<string>() { "<- Gå tillbaka till startsidan"});
            if (categories[0] == null)
            {
                for(int i = 0; i < 3; i++)
                {
                    var categoriW = new Window($"Kategori {i + 1} ({i + 2})", 40 + (i) * 20, 50, new List<string> { "Laddar" });
                    Windows.Add(categoriW);
                }
            } else
            {
                int index = 0;
                foreach(var categori in categories)
                {
                    var categoriW = new Window($"Kategori {index + 1} ({index + 2})", 40 + (index) * 20, 50, new List<string> { categori.Name });
                    Windows.Add(categoriW);
                    index++;
                }
            }
                Windows.Add(backToHomePage);
        }
    }
}
