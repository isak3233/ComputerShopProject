using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using webbshop.Controller;
using webbshop.Models;

namespace webbshop
{
    internal class WebShop
    {
        static public CancellationTokenSource Cts; // En variable som vi kan kalla cancel på för att stänga av metoder som beror på den
        public async Task StartWebShop()
        {
            // När konsol applikationen stängs av så skickar vi till databasen att vi logar ut
            AppDomain.CurrentDomain.ProcessExit += async (sender, e) =>
            {
                if (Cookie.User == null) return;
                Cts.Cancel();
                await LoginController.SetLoginSession(Cookie.User, DateTime.UtcNow);
            };

            using (var db = new ShopDbContext())
            {
                // Om databasen är redan populerad så kommer inget hända
                
                
                await DatabaseSeeder.PopulateDatabase(db);
            }
            
            IController currentController = new HomePageController();
            while (true)
            {
                currentController = await currentController.ActivateController();
            }
        }

    }
 
}
