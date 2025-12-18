using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.UI
{
    internal class CategoryPage : Page
    {
        public enum Buttons
        {
            homePage
        }
        public CategoryPage()
        {
   
            var backToHomePage = new Window("(1)", 0, 0, new List<string>() { "<- Gå tillbaka till startsidan"});
            Windows.Add(backToHomePage);
        }
    }
}
