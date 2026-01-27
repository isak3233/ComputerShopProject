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
                WebShop.Cts.Cancel();
                SetLoginSession(Cookie.User, DateTime.UtcNow);
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
                                await RegisterLogin(user, email); 
                            }
                            
                            if (user == null)
                            {
                                Console.WriteLine("Email eller lösenord är fel");
                            } else
                            {
                                Cookie.User = user;

                                // Sätter upp ett nytt alarm och sedan startar våran loop som skickar till databasen att vi fortfarande är inloggade
                                WebShop.Cts = new CancellationTokenSource();
                                StartUpdateLoginSessionLoop(WebShop.Cts.Token); // Vi behöver inte veta resultatet och vårat program är inte beroende på metoden så vi behöver inte köra await
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
        public async Task StartUpdateLoginSessionLoop(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (Cookie.User == null) return;
                await SetLoginSession(Cookie.User, DateTime.UtcNow.AddMinutes(5));
                await Task.Delay(TimeSpan.FromMinutes(4), stoppingToken);
                
                
            }
        }
        static public async Task SetLoginSession(User user, DateTime expireDate)
        {
            LoginLog? loginLog = MongoConnection.GetLoginLogCollection().AsQueryable().SingleOrDefault(la => la.Email == user.Email);
            if (loginLog == null) return;
            loginLog.LoginSessionExpire = expireDate;
            await MongoConnection.GetLoginLogCollection().ReplaceOneAsync(la => la.Id == loginLog.Id, loginLog);

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
            LoginLog? loginLog = MongoConnection.GetLoginLogCollection().AsQueryable().SingleOrDefault(la => la.Email == email);
            if(loginLog == null)
            {
                LoginLog newLoginLog = new LoginLog()
                {
                    Email = email,
                    LoginAttemptsAmount = 0,
                    FailedLoginAttempts = 0,

                };
                if(user == null)
                {
                    newLoginLog.FailedLoginAttempts += 1;
                }  else
                {
                    newLoginLog.LastLogonDate = DateTime.UtcNow;
                }
                newLoginLog.LoginAttemptsAmount += 1;
                await MongoConnection.GetLoginLogCollection().InsertOneAsync(newLoginLog);
            } else
            {
                if (user == null)
                {
                    loginLog.FailedLoginAttempts += 1;
                } else
                {
                    loginLog.LastLogonDate = DateTime.UtcNow;
                }
                loginLog.LoginAttemptsAmount += 1;
                await MongoConnection.GetLoginLogCollection().ReplaceOneAsync(la => la.Id == loginLog.Id, loginLog);

            }
            
            
        }
    }
}
