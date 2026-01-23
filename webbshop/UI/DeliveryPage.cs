using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;

namespace webbshop.UI
{
    
    public class DeliveryPage : Page
    {
        public enum Buttons
        {
            Cart,
            CityCountry,
            Streetname,
            Pay

        }
        public DeliveryPage()
        {
            Update();
        }
        public void Update(DeliveryOption[]? deliveryOptions = null)
        {
            Windows = new List<Window>();
            // Om användaren har hoppat mellan fönster ser vi till att informationen dom skrev innan inte försvinner
            User? user = Cookie.DeliveryProcessUser;
            int selectedDeliveryOption = Cookie.DeliveryOption.Value;





            var backToCartW = new Window("(1)", 0, 0, new List<string> { "<- Gå tillbaka till kundvagnen" });
            Windows.Add(backToCartW);
            if (user == null || deliveryOptions == null || selectedDeliveryOption == null)
            {
                var loadingDeliveryOptionsW = new Window("", 50, 50, new List<string> { "Laddar fraktalternativ" });
                Windows.Add(loadingDeliveryOptionsW);
                this.Render();
                return;
            }

            var countryCityW = new Window("Stad och land (2)", 30, 30, new List<string> { user.City.Country.Name + "/" + user.City.Name });
            Windows.Add(countryCityW);

            var streetName = new Window("Gatuadress (3)", 30, 50, new List<string> { user.StreetName });
            Windows.Add(streetName);

            for (int i = 0; i < deliveryOptions.Length; i++)
            {
                DeliveryOption deliveryOption = deliveryOptions[i];
                if (selectedDeliveryOption == i)
                {
                    var deliveryOptionW = new Window($"Vald", 60, 40 + i * 20, new List<string> { deliveryOption.Name, deliveryOption.Price + "kr" });
                    Windows.Add(deliveryOptionW);
                }
                else
                {
                    var deliveryOptionW = new Window($"Välj ({i + 5})", 60, 40 + i * 20, new List<string> { deliveryOption.Name, deliveryOption.Price + "kr" });
                    Windows.Add(deliveryOptionW);
                }

            }
            var payPageW = new Window("(4)", 90, 90, new List<string> { "Gå till betalning -> " });
            Windows.Add(payPageW);
            this.Render();
        }
    }
}
