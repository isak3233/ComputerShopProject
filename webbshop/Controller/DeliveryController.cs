using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static webbshop.UI.DeliveryPage;


namespace webbshop.Controller
{
    public class DeliveryController : IController
    {
        public async Task<IController> ActivateController()
        {

            
            if (Cookie.DeliveryOption == null)
            {
                Cookie.DeliveryOption = 0;
            }

            DeliveryPage page = new DeliveryPage();
            page.Render();

            // Lägger till country info i user objektet så våran frakt sida kan visa den informationen
            var user = await GetUserCountry(Cookie.User); 
            if (Cookie.DeliveryProcessUser == null)
            {
                Cookie.DeliveryProcessUser = user;
            }

            var deliveryOptions = await GetDeliveryOptions();
            page = new DeliveryPage(deliveryOptions);
            page.Render();








            while (true)
            {
                int? option = InputHelper.GetIntFromUser("", true);
                if (option == null)
                {
                    page.Render();
                }
                else
                {
                    if(option - 5 >= 0 && option - 5 < deliveryOptions.Length)
                    {
                        Cookie.DeliveryOption = option.Value - 5;
                        page = new DeliveryPage(deliveryOptions);
                        page.Render();
                    }
                    option -= 1;
                    switch ((Buttons)option)
                    {

                        case Buttons.Cart:
                            return new CartController();
                        case Buttons.CityCountry:
                            Country selectedCountry = await RegisterUserController.GetCountry();
                            City selectedCity = await RegisterUserController.GetCity(selectedCountry);
                            Cookie.DeliveryProcessUser.City = selectedCity;
                            Cookie.DeliveryProcessUser.City.Country = selectedCountry;
                            page = new DeliveryPage(deliveryOptions);
                            page.Render();
                            break;
                        case Buttons.Streetname:
                            Cookie.DeliveryProcessUser.StreetName = InputHelper.GetStringFromUser("Gatuadress: ");
                            page = new DeliveryPage(deliveryOptions);
                            page.Render();
                            break;
                        case Buttons.Pay:
                            return new PayController();
                        default:
                            page.Render();
                            break;
                    }
                }

            }

        }
        private async Task<User> GetUserCountry(User user)
        {
            using(var db = new ShopDbContext())
            {
                var result = await db.Users
                    .Include(u => u.City)
                        .ThenInclude(c => c.Country)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);
                return result;
            }
        }
        static public async Task<DeliveryOption[]> GetDeliveryOptions()
        {
            using(var db = new ShopDbContext())
            {
                return await db.DeliveryOptions.ToArrayAsync();
            }
        }
    }
}
