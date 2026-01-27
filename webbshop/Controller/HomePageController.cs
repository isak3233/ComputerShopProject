using Azure;
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
        public async Task<IController> ActivateController()
        {

            Product[] selectedProducts = new Product[3];
            HomePage page = new HomePage();

            selectedProducts = await GetSelectedProducts();
            page.Update(selectedProducts);



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
                            return new CategoryController();
                        case Buttons.Login:
                            return new LoginController();
                        case Buttons.Cart:
                            if(Cookie.User != null)
                            {
                                return new CartController();
                            }
                            break;
                        case Buttons.Selected1:
                            if(selectedProducts.Length >= 1)
                            {
                                return new ProductController(selectedProducts[0]);
                            }
                            break;
                        case Buttons.Selected2:
                            if (selectedProducts.Length >= 2)
                            {
                                return new ProductController(selectedProducts[1]);
                            }
                            break;
                            
                        case Buttons.Selected3:
                            if (selectedProducts.Length >= 3)
                            {
                                return new ProductController(selectedProducts[2]);
                            }
                            break;
                        case Buttons.AdminPanel:
                            if(Cookie.User != null && Cookie.User.IsAdmin == true)
                            {
                                return new AdminController();   
                            }
                            break;

                        default:
                            page.Render();
                            break;
                    }
                }
                    
            }

        }
        private async Task<Product[]> GetSelectedProducts()
        {
            using (var db = new ShopDbContext())
            {
                return await db.Products.Where(Product => Product.IsSelected == true).Take(3).ToArrayAsync();

            }
        }
    }
}
