using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Controller;
using webbshop.Models;

namespace webbshop
{
    internal class WebShop
    {
        public async Task StartWebShop()
        {
            using(var db = new ShopDbContext())
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
