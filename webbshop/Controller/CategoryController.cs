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
            using (var db = new ShopDbContext())
            {
                categories = await db.Categories.ToArrayAsync();
                page = new CategoryPage(categories);
                page.Render();
            }
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
                           
                            return new ShopController(categories[0]);
                        default:
                            break;

                    }
                }

            }
        }
        
    }
}
