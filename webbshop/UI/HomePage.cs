using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;

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
            Selected3,
            AdminPanel
        }
        public HomePage()
        {
            Update();
        }
        public void Update(Product[]? selectedProducts = null)
        {
            Windows = new List<Window>();
            var welcomeWindow = new Window("", 10, 0, new List<string> { Cookie.User == null ? "Välkommen till datorbutiken!" : "Välkommen till datorbutiken " + Cookie.User.FirstName });
            Windows.Add(welcomeWindow);

            if (Cookie.User == null)
            {
                var loginWindow = new Window("(1)", 100, 0, new List<string> { "Logga in" });
                Windows.Add(loginWindow);
            }
            else
            {
                var logoutWindow = new Window("(1)", 100, 0, new List<string> { "Logga ut" });
                Windows.Add(logoutWindow);

                var cartWindow = new Window("(2)", 90, 0, new List<string> { "Kundvagn" });
                Windows.Add(cartWindow);

                if (Cookie.User.IsAdmin)
                {
                    var adminWindow = new Window("(7)", 100, 100, new List<string> { "Gå till admin panel" });
                    Windows.Add(adminWindow);
                }
            }



            var categoryWindow = new Window("(3)", 60, 40, new List<string> { "Gå till Kategorier" });
            Windows.Add(categoryWindow);
            if (selectedProducts == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    var selectedProductInfo = new Window($"Erbjudande {i + 1} ({i + 4})", 40 + (i) * 20, 80, new List<string> { "Laddar" });
                    Windows.Add(selectedProductInfo);
                }
            }
            else
            {
                int index = 1;
                foreach (var selectedProduct in selectedProducts)
                {
                    var selectedProductInfo = new Window($"Erbjudande {index} ({index + 3})", 30 + (index - 1) * 30, 80, new List<string> { selectedProduct.Name, selectedProduct.Price.ToString() + "kr" });
                    Windows.Add(selectedProductInfo);
                    index++;
                }
            }
            this.Render();
        }
        
        
    }
}
