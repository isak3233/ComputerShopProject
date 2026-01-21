using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static webbshop.UI.CartPage;

namespace webbshop.Controller
{
    internal class CartController : IController
    {
        public async Task<IController> ActivateController()
        {
            List<CartProduct>? cartProducts = null;
            CartPage page = new CartPage();
            page.Render();


            cartProducts = await GetCartProducts(Cookie.User);
            page = new CartPage(cartProducts);
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
                    // Ta bort eller ändra antalet på en produkt i vagnen
                    if(option.Value > 2 && (option.Value - 3) < cartProducts.Count)
                    {
                        var cartProduct = cartProducts[option.Value - 3];
                        int[] result = InputHelper.GetCartProductOptionFromUser();
                        int cartProductOption = result[0];
                        int cartProductAmount = result[1];
                        if(cartProductOption == 1)
                        {
                            await RemoveProductFromCart(cartProduct);
                        }
                        else if(cartProductOption == 2)
                        {
                            await ChangeCartProductAmount(cartProduct, cartProductAmount);
                        }
                        cartProducts = await GetCartProducts(Cookie.User);
                        page = new CartPage(cartProducts);
                        page.Render();
                    }
                    option -= 1;
                    switch ((Buttons)option)
                    {
                        case Buttons.HomePage:
                            return new HomePageController();
                        case Buttons.DeliveryOptions:
                            return new DeliveryController();
                        default:
                            page.Render();
                            break;
                    }
                }

            }
        }
        static public async Task<List<CartProduct>> GetCartProducts(User user)
        {
            using (var db = new ShopDbContext())
            {
                var result = await db.CartProducts
                    .Include(cartproduct => cartproduct.Product)
                    .Where(cartproduct => cartproduct.UserId == user.Id)
                    .ToListAsync();
                return result;
            }
        }
        static public async Task RemoveProductFromCart(CartProduct cartProduct)
        {
            using(var db = new ShopDbContext())
            {
                db.CartProducts.Remove(cartProduct);
                await db.SaveChangesAsync();
            }

        }
        private async Task ChangeCartProductAmount(CartProduct cartProduct, int Amount)
        {
            using(var db = new ShopDbContext())
            {
                db.CartProducts.Attach(cartProduct);
                cartProduct.Amount = Amount;
                await db.SaveChangesAsync();

            }
        }
        
    }
}
