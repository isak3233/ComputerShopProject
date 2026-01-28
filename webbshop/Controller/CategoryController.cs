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
            List<Category> categories = new List<Category>();
            CategoryPage page = new CategoryPage();
            int categoriIndexOn = 0;

            categories = await GetCategories();
            page.Update(categories, categoriIndexOn);
            
            while (true)
            {
                int? option = InputHelper.GetIntFromUser("", true);
                if (option == null)
                {
                    page.Render();
                }
                else
                {
                    if (option > 1 && option < 8)
                    {
                        int categoriSelected = (option.Value - 2) + categoriIndexOn;
                        return new ShopController(categories[categoriSelected]);
                    }
                    option -= 1;
                    switch ((Buttons)option)
                    {
                        case Buttons.HomePage:
                            return new HomePageController();
                        case Buttons.ShowMore:
                            if(categoriIndexOn + 6 < categories.Count())
                            {
                                categoriIndexOn += 6;
                            }
                            page.Update(categories, categoriIndexOn);
                            break;
                        case Buttons.ShowLess:
                            if(categoriIndexOn - 6 >= 0)
                            {
                                categoriIndexOn -= 6;
                            }
                            page.Update(categories, categoriIndexOn);
                            break;
                        default:
                            break;

                    }
                }

            }
        }
        private async Task<List<Category>> GetCategories()
        {
            using (var db = new ShopDbContext())
            {
                return await db.Categories.ToListAsync();
                
            }
        }
        
    }
}
