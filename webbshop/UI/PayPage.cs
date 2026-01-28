using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;

namespace webbshop.UI
{

    public class PayPage : Page
    {
        public enum Buttons
        {
            Delivery,
            Pay,
            ShowMore,
            ShowLess
        }
        public PayPage()
        {
            Update();

        }
        public void Update(PaymentOption[]? paymentOption = null, CartProduct[]? cartProducts = null, DeliveryOption[]? deliveryOption = null, int indexOn = 0)
        {
            Windows = new List<Window>();
            var backToDeliveryOptionsW = new Window("(1)", 0, 0, new List<string> { "<- Gå tillbaka till fraktalternativ" });
            Windows.Add(backToDeliveryOptionsW);


            if (paymentOption == null || cartProducts == null || deliveryOption == null)
            {
                var loadingOptionsW = new Window("", 70, 50, new List<string> { "Laddar" });
                Windows.Add(loadingOptionsW);
                this.Render();
                return;
            }

            // Pris info
            decimal totalCartProduct = 0;
            decimal deliveryFee = deliveryOption[Cookie.DeliveryOption.Value].Price;
            foreach (var cartProduct in cartProducts)
            {
                totalCartProduct += cartProduct.Product.Price;

            }
            decimal total = totalCartProduct + deliveryFee;
            var totalW = new Window("", 90, 10, new List<string> { $"Produkter: {totalCartProduct}kr", $"Frakt: {deliveryFee}kr", $"Moms: {Math.Round(total * 0.25M, 2)}kr", $"Totalt: {total}kr" });
            Windows.Add(totalW);

            // Betalningalternativ fönster

            for (int i = 0; i < paymentOption.Length; i++)
            {
                var option = paymentOption[i];
                if (Cookie.SelectedPaymentOption == i)
                {
                    var paymentOptionW = new Window($"Vald", 70, 50 + i * 15, new List<string> { option.Name });
                    Windows.Add(paymentOptionW);
                }
                else
                {
                    var paymentOptionW = new Window($"({i + 5})", 70, 50 + i * 15, new List<string> { option.Name });
                    Windows.Add(paymentOptionW);
                }
            }


            //Produkter
            var showMoreW = new Window("(3)", 60, 90, new List<string> { "Gå fram i produkter ->" });
            var showLessW = new Window("(4)", 30, 90, new List<string> { "<- Gå tillbaka i produkter" });
            var windowsToAdd = new List<Window>();
            foreach (var cartProduct in cartProducts)
            {
                var productW = new Window("", 0, 0, new List<string>() { cartProduct.Product.Name, cartProduct.Product.Price.ToString() + "kr", "Antal: " + cartProduct.Amount.ToString() });
                windowsToAdd.Add(productW);
            }
            int amountOfCols = 2;
            int amountOfRows = 3;
            int startX = 15;
            int startY = 35;
            int xPerWindow = 30;
            int yPerWindow = 20;
            AddDynamicWindows(indexOn, windowsToAdd, showMoreW, showLessW, amountOfCols, amountOfRows, startX, startY, xPerWindow, yPerWindow);



            
            
            

            var payW = new Window("(2)", 90, 90, new List<string> { "Betala ->" });
            Windows.Add(payW);
            this.Render();
        }
    }
}
