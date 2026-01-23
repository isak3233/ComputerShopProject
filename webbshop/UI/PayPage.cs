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
            Pay
        }
        public PayPage()
        {
            Update();

        }
        public void Update(PaymentOption[]? paymentOption = null, CartProduct[]? cartProducts = null, DeliveryOption[]? deliveryOption = null)
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
                    var paymentOptionW = new Window($"({i + 3})", 70, 50 + i * 15, new List<string> { option.Name });
                    Windows.Add(paymentOptionW);
                }
            }

            // Produkter
            int col = 0;
            int row = 0;
            for (int i = 0; i < cartProducts.Length; i++)
            {
                if (col % 2 == 0)
                {
                    row++;
                    col = 0;
                }
                var cartProduct = cartProducts[i];
                var cartProductW = new Window($"", 15 + col * 30, 15 + row * 20, new List<string>() { cartProduct.Product.Name, cartProduct.Product.Price.ToString() + "kr", "Antal: " + cartProduct.Amount.ToString() });
                Windows.Add(cartProductW);
                col++;
            }

            var payW = new Window("(2)", 90, 90, new List<string> { "Betala ->" });
            Windows.Add(payW);
            this.Render();
        }
    }
}
