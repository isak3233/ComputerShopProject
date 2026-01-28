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
            string? searchInput = null;
            int productIndexOn = 0;
            var loadingProducts = GetProductsFromCategory(Category);
            ShopPage page = new ShopPage();

            var products = await loadingProducts;
            page.Update(products, searchInput, productIndexOn);


            while (true)
            {
                int? option = InputHelper.GetIntFromUser("", true);
                if (option == null)
                {
                    page.Render();
                }
                else
                {
                    if(option > 2 && (option - 3) < 9)
                    {
                        int productSelected = (option.Value - 3) + productIndexOn;
                        return new ProductController(products[productSelected]);

                    }

                    option -= 1;
                    switch ((Buttons)option)
                    {
                        case Buttons.Category:
                            return new CategoryController();
                        case Buttons.Search:
                            Console.Write("Sök: ");
                            searchInput = Console.ReadLine();
                            if (searchInput == null) break;
                            products = await GetProductFromSearch(searchInput);
                            page.Update(products, searchInput);
                            break;
                        case Buttons.ShowMore:
                            if(productIndexOn + 9 < products.Count())
                            {
                                productIndexOn += 9;
                            }
                            page.Update(products, searchInput, productIndexOn);
                            break;
                        case Buttons.ShowLess:
                            if (productIndexOn - 9 >= 0)
                            {
                                productIndexOn -= 9;
                            }
                            page.Update(products, searchInput, productIndexOn);
                            break;
                        default:
                            page.Render();
                            break;
                    }
                }

            }
        }
        private async Task<Product[]> GetProductsFromCategory(Category category)
        {
            using (var db = new ShopDbContext())
            {
                return await db.Products.Where(p => p.CategoryId == category.Id).ToArrayAsync();
            }
        }
        private async Task<Product[]> GetProductFromSearch(string searchInput)
        {
            using(var db = new ShopDbContext())
            {
                return await db.Products
                    .Include(p => p.Category)
                    .Where(p => p.Name.Contains(searchInput) || p.Category.Name.Contains(searchInput)).ToArrayAsync();
            }
        }

    }
}
