using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
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
                            if(email != null)
                            {
                                RegisterLogin(user, email); // Vi behöver inte veta resultatet och vårat program är inte beroende på metoden så vi behöver inte köra await
                            }
                            
                            if (user == null)
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
        private async Task RegisterLogin(User? user, string email)
        {

            
            using (var db = new ShopDbContext())
            {
                if( !(await db.Users.AnyAsync(u => u.Email == email)) ) // Vi kollar om emailen är kopplad till en user i databasen innan vi spara loggning
                {
                    return;
                }
            }
            LoginAttempt? loginAttempt = MongoConnection.GetLoginAttemptCollection().AsQueryable().SingleOrDefault(la => la.Email == email);
            if(loginAttempt == null)
            {
                LoginAttempt newLoginAttempt = new LoginAttempt()
                {
                    Email = email,
                    LoginAttemptsAmount = 0,
                    FailedLoginAttempts = 0,

                };
                if(user == null)
                {
                    newLoginAttempt.FailedLoginAttempts += 1;
                }  else
                {
                    newLoginAttempt.LastLogonDate = DateTime.Now;
                }
                newLoginAttempt.LoginAttemptsAmount += 1;
                await MongoConnection.GetLoginAttemptCollection().InsertOneAsync(newLoginAttempt);
            } else
            {
                if (user == null)
                {
                    loginAttempt.FailedLoginAttempts += 1;
                } else
                {
                    loginAttempt.LastLogonDate = DateTime.Now;
                }
                loginAttempt.LoginAttemptsAmount += 1;
                await MongoConnection.GetLoginAttemptCollection().ReplaceOneAsync(la => la.Id == loginAttempt.Id, loginAttempt);

            }
            
            
        }
    }
}
