using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static webbshop.UI.ProductPage;

namespace webbshop.Controller
{
    public class ProductController : IController
    {
        private Product SelectedProduct { get; set; }

        public ProductController(Product selectedProduct)
        {
            SelectedProduct = selectedProduct;
        }
        public async Task<IController> ActivateController()
        {

            ProductPage page = new ProductPage(SelectedProduct);
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
                        case Buttons.Shop:
                            // Skapar en category objekt bara med id för att shopcontroller ska kunna söka efter de andra produkterna med samma id
                            Category tempCategory = new Category();
                            tempCategory.Id = SelectedProduct.CategoryId;
                            return new ShopController(tempCategory);
                        case Buttons.AddToCart:
                            if(Cookie.User != null)
                            {
                                await AddProductToCart(SelectedProduct);
                                page = new ProductPage(SelectedProduct, true);
                                page.Render();
                            }
                            break;
                        default:
                            page.Render();
                            break;
                    }
                }

            }

        }
        private async Task AddProductToCart(Product product)
        {
            using(var db = new ShopDbContext())
            {
                var cartProduct = await db.CartProducts.Where(
                    cartproduct => cartproduct.ProductId == product.Id &&
                    cartproduct.UserId == Cookie.User.Id)
                    .SingleOrDefaultAsync();
                if(cartProduct != null)
                {
                    cartProduct.Amount++;
                } else
                {
                    cartProduct = new CartProduct()
                    {
                        UserId = Cookie.User.Id,
                        ProductId = product.Id,
                        Amount = 1
                    };
                    await db.CartProducts.AddAsync(cartProduct);
                }
                    
                await db.SaveChangesAsync();



            }
        }
    }
    
}
