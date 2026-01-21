using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;

namespace webbshop.UI
{
    public class CartPage : Page
    {
        public enum Buttons
        {
            HomePage,
            DeliveryOptions,
        }
        public CartPage(List<CartProduct>? cartProducts = null)
        {
            var backToHomePageW = new Window("(1)", 0, 0, new List<string>() { "<- Gå tillbaka till startsidan" });
            Windows.Add(backToHomePageW);
            if (cartProducts == null)
            {
                var loadingProductsW = new Window("", 50, 50, new List<string>() { "Laddar in produkter" });
                Windows.Add(loadingProductsW);
                return;
            }
            if (cartProducts.Count == 0)
            {
                var noProductsW = new Window("", 50, 50, new List<string>() { "Inga produkter hittades i din kundvagn :(" });
                Windows.Add(noProductsW);
                return;
            }

            // Skriver ut all produkter
            int col = 0;
            int row = 0;
            for (int i = 0; i < cartProducts.Count; i++)
            {
                if (col % 2 == 0)
                {
                    row++;
                    col = 0;
                }
                var cartProduct = cartProducts[i];
                var cartProductW = new Window($"({i + 3})", 10 + col * 30, 10 + row * 20, new List<string>() { cartProduct.Product.Name, cartProduct.Product.Price.ToString() + "kr" , "Antal: " + cartProduct.Amount.ToString() });
                Windows.Add(cartProductW);
                col++;
            }

            // Skriver ut det totala priset
            decimal total = 0;
            foreach(var cartProduct in cartProducts)
            {
                total += cartProduct.Product.Price * cartProduct.Amount;
            }
            var cartInfoW = new Window("", 100, 30, new List<string>() { $"Totalt: {total}kr", $"Av det är {Math.Round(total * 0.25M, 2)}kr moms", "Fraktpriset visas när du fortsätter till betalning" });
            Windows.Add(cartInfoW);

            var deliveryOptionsW = new Window("(2)", 90, 90, new List<string>() { "Gå vidare till fraktalternativen ->"});
            Windows.Add(deliveryOptionsW);
        }
    }
}
