using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
namespace webbshop.UI
{
    internal class ProductPage : Page
    {
        public enum Buttons
        {
            Shop,
            AddToCart

        }
        public ProductPage(Product product, bool productAddedToCart = false)
        {
            var shopW = new Window("(1)", 0, 0, new List<string> { "<- Gå tillbaka till shop sidan " });
            Windows.Add(shopW);

            var productNameW = new Window("Produkt titel", 20, 30, new List<string> { product.Name });
            Windows.Add(productNameW);

            string inventoryAmount = product.InventoryBalance.ToString();
            if (product.InventoryBalance < 0)
            {
                inventoryAmount = "0";
                var warningW = new Window("VARNING", 10, 80, new List<string> { "Denna produkten finns just nu inte i lager", "detta kan leda till längre leveranstid" });
                Windows.Add(warningW);
            }
            var productPriceW = new Window("", 20, 45, new List<string> { product.Price.ToString() + "kr", "Antal i lager: " + inventoryAmount });
            Windows.Add(productPriceW);


            List<string> productDetailsBroken = BreakSentence(product.Details, 70);
            var productDetailsW = new Window("Beskrivning", 90, 30, productDetailsBroken);
            Windows.Add(productDetailsW);

            var addToCartW = new Window("(2)", 90, 90, new List<string> { Cookie.User == null ? "Du måste vara inloggad för att kunna lägga till produkten i varukorgen" : "Lägg till i varukorgen ->" });
            Windows.Add(addToCartW);

            if(productAddedToCart)
            {
                var productAddedW = new Window("", 50, 90, new List<string> { "Produkten har lagts till i din varukorg" });
                Windows.Add(productAddedW);
            }
        }
        // Metod som delar upp en lång sträng i mindre strängar och sedan lägger dom i en lista
        private List<string> BreakSentence(string text, int maxLength)
        {
            List<string> returnList = new List<string>();
            int start = 0;

            while (start < text.Length)
            {
                int end = Math.Min(start + maxLength, text.Length);

                if (end < text.Length)
                {
                    int lastSpace = text.LastIndexOf(' ', end - 1, end - start);
                    if (lastSpace > start)
                    {
                        end = lastSpace + 1;
                    }
                }

                string part = text.Substring(start, end - start).Trim();
                returnList.Add(part);

                start = end;
            }

            return returnList;
        }
    }
}
