using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static webbshop.UI.ShopPage;


namespace webbshop.Controller
{
    public class ShopController : IController
    {
        private Category Category;
        public ShopController(Category category)
        {
            Category = category;
        }
        public async Task<IController> ActivateController()
        {
            var loadingProducts = GetProductsFromCategory(Category);
            ShopPage page = new ShopPage(null);
            page.Render();

            var products = await loadingProducts;
            page = new ShopPage(products);
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
                    if(option > 1 && (option - 2) < products.Length)
                    {
                        option -= 2;
                        var selectedProduct = products[option.Value];
                        return new ProductController(selectedProduct);

                    }

                    option -= 1;
                    switch ((Buttons)option)
                    {
                        case Buttons.Category:
                            return new CategoryController();
                        default:
                            page.Render();
                            break;
                    }
                }

            }
        }
        public async Task<Product[]> GetProductsFromCategory(Category category)
        {
            using (var db = new ShopDbContext())
            {
                return await db.Products.Where(p => p.CategoryId == category.Id).ToArrayAsync();
            }
        }

    }
}
