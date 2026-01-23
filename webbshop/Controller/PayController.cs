using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static webbshop.UI.PayPage;

namespace webbshop.Controller
{
    internal class PayController : IController
    {
        public async Task<IController> ActivateController()
        {

            if(Cookie.SelectedPaymentOption == null)
            {
                Cookie.SelectedPaymentOption = 0;
            }
            PayPage page = new PayPage();

            var paymentOptionsT = GetPaymentOptions();
            var cartProductsT = CartController.GetCartProducts(Cookie.User);
            var deliveryOptionsT = DeliveryController.GetDeliveryOptions();

            var paymentOptions = await paymentOptionsT;
            var cartProducts = (await cartProductsT).ToArray();
            var deliveryOptions = await deliveryOptionsT;

            page.Update(paymentOptions, cartProducts, deliveryOptions);

            while (true)
            {
                int? option = InputHelper.GetIntFromUser("", true);
                if (option == null)
                {
                    page.Render();
                }
                else
                {
                    if(option.Value - 3 >= 0 && option - 3 < paymentOptions.Length)
                    {
                        Cookie.SelectedPaymentOption = option.Value - 3;
                        page.Update(paymentOptions, cartProducts, deliveryOptions);
                    }
                    option -= 1;
                    switch ((Buttons)option)
                    {
                        case Buttons.Delivery:
                            return new DeliveryController();
                        case Buttons.Pay:
                            // Sparar våran order, Tar bort produkterna från vagnen, Tar bort antalet produkter beställt från lager saldot
                            List<Task> tasks = new List<Task>();
                            foreach (var cartProduct in cartProducts)
                            {
                                PaymentHistory paymentHistory = new PaymentHistory()
                                {
                                    PaymentOptionId = Cookie.SelectedPaymentOption.Value + 1,
                                    ProductId = cartProduct.ProductId,
                                    UserId = Cookie.DeliveryProcessUser.Id,
                                    Amount = cartProduct.Amount,
                                    DeliveryStreet = Cookie.DeliveryProcessUser.StreetName,
                                    DeliveryCityId = Cookie.DeliveryProcessUser.CityId.Value,
                                    DeliveryOptionId = Cookie.DeliveryOption.Value + 1
                                };
                                tasks.Add(RegisterPayment(paymentHistory));
                                tasks.Add(CartController.RemoveProductFromCart(cartProduct));
                                tasks.Add(RemoveFromStock(paymentHistory.ProductId, paymentHistory.Amount));
                            }
                            
                            // Resetar cookie så nästa gång vid beställning använder den uppgifterna som är sparade under user objektet
                            Cookie.DeliveryProcessUser = null;
                            Cookie.DeliveryOption = null;
                            Cookie.SelectedPaymentOption = null;


                            // Väntar på alla tasks som har startas
                            await Task.WhenAll(tasks); 

                            ThankYouPage thankYouPage = new ThankYouPage();
                            Console.ReadLine();
                            return new HomePageController();

                        default:
                            page.Render();
                            break;

                    }
                }

            }
        }
        private async Task RegisterPayment(PaymentHistory paymentHistory)
        {
            using(var db = new ShopDbContext())
            {
                db.PaymentHistories.Add(paymentHistory);

                await db.SaveChangesAsync();
            }
        }
        private async Task<PaymentOption[]> GetPaymentOptions()
        {
            using(var db = new ShopDbContext())
            {
                return await db.PaymentOptions.ToArrayAsync();
            }
        }
        private async Task RemoveFromStock(int productId, int amount)
        {
            using(var db = new ShopDbContext())
            {
                var product = await db.Products.Where(p => p.Id == productId).SingleOrDefaultAsync();
                if(product != null)
                {
                    product.InventoryBalance -= amount;
                    
                }
                await db.SaveChangesAsync();

            }
        }
    }
}
