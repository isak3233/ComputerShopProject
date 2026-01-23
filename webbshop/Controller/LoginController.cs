using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static webbshop.UI.LoginPage;

namespace webbshop.Controller
{
    public class LoginController : IController
    {
        public async Task<IController> ActivateController()
        {
            if(Cookie.User != null )
            {
                Cookie.User = null;
                Cookie.DeliveryOption = null;
                Cookie.DeliveryProcessUser = null;
                Cookie.SelectedPaymentOption = null;
                return new HomePageController();
            }
            LoginPage page = new LoginPage();

            string? email = null;
            string? password = null;

            while (true)
            {
                int? option = InputHelper.GetIntFromUser("", true);
                if (option == null)
                {
                    page.Render();
                }
                else
                {
                    option -= 1;
                    switch ((Buttons)option)
                    {
                        case Buttons.HomePage:
                            return new HomePageController();
                        case Buttons.Email:
                            Console.Write("Skriv din email: ");
                            email = Console.ReadLine().ToLower();
                            page.Update (email, password);
                            break;
                        case Buttons.Password:
                            Console.Write("Skriv ditt lösenord: ");
                            password = Console.ReadLine();
                            page.Update(email, password);
                            break;
                        case Buttons.Login:
                            User? user = await GetUser(email, password);
                            if(user == null)
                            {
                                Console.WriteLine("Email eller lösenord är fel");
                            } else
                            {
                                Cookie.User = user;
                                return new HomePageController();
                            }
                            break;
                        case Buttons.Register:
                            return new RegisterUserController();
                        default:
                            page.Render();
                            break;
                    }
                }

            }
        }
        public async Task<User?> GetUser(string? email, string? password)
        {
            using(var db = new ShopDbContext())
            {
                User? user = await db.Users.Where(user => user.Email == email && user.Password == password).SingleOrDefaultAsync();
                return user;
            }
        }
    }
}
