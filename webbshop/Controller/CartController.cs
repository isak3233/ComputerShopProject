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
            int cartIndexOn = 0;

            cartProducts = await GetCartProducts(Cookie.User);
            page.Update(cartProducts, cartIndexOn);


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
                    if(option.Value > 2 && (option.Value - 3) < 6)
                    {
                        int cartProductSelected = (option.Value - 3) + cartIndexOn;
                        var cartProduct = cartProducts[cartProductSelected];
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
                        page.Update(cartProducts, cartIndexOn);
                    }
                    option -= 1;
                    switch ((Buttons)option)
                    {
                        case Buttons.HomePage:
                            return new HomePageController();
                        case Buttons.DeliveryOptions:
                            return new DeliveryController();
                        case Buttons.ShowMore:
                            if(cartIndexOn + 6 < cartProducts.Count)
                            {
                                cartIndexOn += 6;
                            }
                            page.Update(cartProducts, cartIndexOn);
                            break;
                        case Buttons.ShowLess:
                            if (cartIndexOn - 6 >= 0)
                            {
                                cartIndexOn -= 6;
                            }
                            page.Update(cartProducts, cartIndexOn);
                            break;
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
