using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.UI
{
    public class HomePage : Page
    {
        public enum Buttons
        {
            Login,
            Cart,
            Category,
            Selected1,
            Selected2,
            Selected3
        }
        public HomePage()
        {
            var welcomeWindow = new Window("", 10, 0, new List<string> { "Välkommen till Datorbutiken" });
            Windows.Add(welcomeWindow);

            var loginWindow = new Window("(1)", 100, 0, new List<string> { "Logga in" });
            Windows.Add(loginWindow);

            var cartWindow = new Window("(2)", 90, 0, new List<string> { "Kundvagn" });
            Windows.Add(cartWindow);

            var categoryWindow = new Window("(3)", 60, 40, new List<string> { "Gå till Kategorier" });
            Windows.Add(categoryWindow);

            var selectedProduct1 = new Window("Erbjudande 1 (4)", 40, 80, new List<string> { "RAM DDR4 32GB", "1,000kr" });
            Windows.Add(selectedProduct1);

            var selectedProduct2 = new Window("Erbjudande 2 (5)", 60, 80, new List<string> { "GPU 4070TI", "10,000kr" });
            Windows.Add(selectedProduct2);

            var selectedProduct3 = new Window("Erbjudande 3 (6)", 80, 80, new List<string> { "Skärm 144hz", "1,500kr" });
            Windows.Add(selectedProduct3);

        }
        
        
    }
}
