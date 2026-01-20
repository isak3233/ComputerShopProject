using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static webbshop.UI.HomePage;

namespace webbshop.Controller
{
    internal class HomePageController : IController
    {
        private User? User;
        public HomePageController(User? user = null)
        {
            User = user;
        }
        public async Task<IController> ActivateController()
        {

            Product[] selectedProducts = new Product[3];
            HomePage page = new HomePage(selectedProducts, User);
            
            using (var db = new ShopDbContext())
            {
                var selectedProductsTask = db.Products.Where(Product => Product.IsSelected == true).ToArrayAsync();

                page.Render();

                selectedProducts = await selectedProductsTask;
                page = new HomePage(selectedProducts, User);
                page.Render();

            }

            


            while (true)
            {
                int? option = InputHelper.GetIntFromUser("", true);
                if(option == null)
                {
                    page.Render();
                } else
                {
                    option -= 1;
                    switch ((Buttons)option)
                    {
                        case Buttons.Category:
                            return new CategoryController(User);
                        case Buttons.Login:
                            return new LoginController(User);
                        case Buttons.AdminPanel:
                            break;
                        default:
                            page.Render();
                            break;
                    }
                }
                    
            }

        }
    }
}
