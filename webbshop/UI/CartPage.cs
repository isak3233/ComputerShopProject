using Azure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            ShowMore = 8,
            ShowLess = 9
        }
        public CartPage()
        {
            Update();
        }
        public void Update(List<CartProduct>? cartProducts = null, int indexOn = 0)
        {
            Windows = new List<Window>();
            var backToHomePageW = new Window("(1)", 0, 0, new List<string>() { "<- Gå tillbaka till startsidan" });
            Windows.Add(backToHomePageW);
            if (cartProducts == null)
            {
                var loadingProductsW = new Window("", 50, 50, new List<string>() { "Laddar in produkter" });
                Windows.Add(loadingProductsW);
                this.Render();
                return;
            }
            if (cartProducts.Count == 0)
            {
                var noProductsW = new Window("", 50, 50, new List<string>() { "Inga produkter hittades i din kundvagn :(" });
                Windows.Add(noProductsW);
                this.Render();
                return;
            }



            var showMoreW = new Window("(9)", 60, 80, new List<string> { "Gå fram i kundvagnen ->" });
            var showLessW = new Window("(10)", 0, 80, new List<string> { "<- Gå tillbaka i kundvagnen" });
            var windowsToAdd = new List<Window>();
            foreach (var cartProduct in cartProducts)
            {
                var productW = new Window("", 0, 0, new List<string>() { cartProduct.Product.Name, cartProduct.Product.Price.ToString() + "kr", "Antal: " + cartProduct.Amount.ToString() });
                windowsToAdd.Add(productW);
            }
            int amountOfCols = 2;
            int amountOfRows = 3;
            int startX = 10;
            int startY = 30;
            int xPerWindow = 30;
            int yPerWindow = 20;
            int choiceStart = 3;
            AddDynamicWindows(indexOn, windowsToAdd, showMoreW, showLessW, amountOfCols, amountOfRows, startX, startY, xPerWindow, yPerWindow, choiceStart);





            


            decimal total = 0;
            foreach (var cartProduct in cartProducts)
            {
                total += cartProduct.Product.Price * cartProduct.Amount;
            }
            var cartInfoW = new Window("", 100, 30, new List<string>() { $"Totalt: {total}kr", $"Av det är {Math.Round(total * 0.25M, 2)}kr moms", "Fraktpriset visas när du fortsätter till betalning" });
            Windows.Add(cartInfoW);

            var deliveryOptionsW = new Window("(2)", 90, 90, new List<string>() { "Gå vidare till fraktalternativen ->" });
            Windows.Add(deliveryOptionsW);

            this.Render();
        }
    }
}
