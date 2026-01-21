using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static webbshop.UI.CategoryPage;

namespace webbshop.Controller
{
    internal class CategoryController : IController
    {
        
        public async Task<IController> ActivateController()
        {
            Category[] categories = new Category[3];
            CategoryPage page = new CategoryPage(categories);
            page.Render();

            categories = await GetCategories();
            page = new CategoryPage(categories);
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
                    option -= 1;
                    switch ((Buttons)option)
                    {
                        case Buttons.HomePage:
                            return new HomePageController();
                        case Buttons.Category1:
                            if(categories.Length > 0) return new ShopController(categories[0]);
                            break;
                        case Buttons.Category2:
                            if (categories.Length > 1) return new ShopController(categories[1]);
                            break;
                        case Buttons.Category3:
                            if (categories.Length > 2) return new ShopController(categories[2]);
                            break;
                        default:
                            break;

                    }
                }

            }
        }
        private async Task<Category[]> GetCategories()
        {
            using (var db = new ShopDbContext())
            {
                return await db.Categories.ToArrayAsync();
                
            }
        }
        
    }
}
