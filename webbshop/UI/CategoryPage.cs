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
            ShowMore = 7,
            ShowLess = 8,
        }
        public CategoryPage()
        {
            Update();
        }
        public void Update(List<Category>? categories = null, int indexOn = 0)
        {
            Windows = new List<Window>();
            var backToHomePage = new Window("(1)", 0, 0, new List<string>() { "<- Gå tillbaka till startsidan" });
            if (categories == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    var categoriW = new Window($"Kategori {i + 1} ({i + 2})", 40 + (i) * 20, 50, new List<string> { "Laddar" });
                    Windows.Add(categoriW);
                }
            }
            else
            {
                var windowsToAdd = new List<Window>();

                var showMoreW = new Window("(8)", 90, 80, new List<string> { "Gå fram i kategorier ->" });

                var showLessW = new Window("(9)", 30, 80, new List<string> { "<- Gå tillbaka i kategorier" });
                foreach (var category in categories)
                {
                    var categoriW = new Window($"Kategori ", 0, 0, new List<string> { category.Name });
                    windowsToAdd.Add(categoriW);
                }
                int amountOfCols = 3;
                int amountOfRows = 2;
                int startX = 40;
                int startY = 50;
                int xPerWindow = 20;
                int yPerWindow = 20;
                int choiceStart = 2;
                AddDynamicWindows(indexOn, windowsToAdd, showMoreW, showLessW, amountOfCols, amountOfRows, startX, startY, xPerWindow, yPerWindow, choiceStart);
                
                    
                
            }
            Windows.Add(backToHomePage);
            this.Render();
        }
    }
}
