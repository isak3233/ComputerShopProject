using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static webbshop.UI.AdminPage;

namespace webbshop.Controller
{
    public class AdminController : IController
    {
        public async Task<IController> ActivateController()
        {
            AdminPage page = new AdminPage();

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
                        case Buttons.AddProduct:
                            Product newProduct = await GetProductInfoFromUser();
                            try
                            {
                                await AddProduct(newProduct);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Något gick fel!");
                            }

                            Console.WriteLine("Ny produkt har lagts till :)");
                            break;
                        case Buttons.RemoveProduct:
                            await WriteOutAllProducts();
                            int removeProductId = InputHelper.GetIntFromUser("Ange produkt id: ", true);
                            Product? removedProduct = await GetProductFromId(removeProductId);
                            if(removedProduct == null)
                            {
                                Console.WriteLine("Produkten finns inte");
                                break;
                            }
                            await RemoveProduct(removeProductId);
                            Console.WriteLine("Produkten borttagen :)");
                            break;
                        case Buttons.ChangeProduct:
                            await WriteOutAllProducts();
                            int changeProductId = InputHelper.GetIntFromUser("Ange produkt id: ", true);
                            Product? product = await GetProductFromId(changeProductId);
                            if (product == null)
                            {
                                Console.WriteLine("Produkten finns inte");
                                break;
                            }
                            Product changedProduct = await ChangeProductValues(product);
                            try
                            {
                                await UpdateProduct(changedProduct);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Något gick fel :(");
                            }
                            Console.WriteLine("Produkten har nu ändrats :)");

                            break;
                        case Buttons.AddCategory:
                            string newCategoryName = InputHelper.GetStringFromUser("Kategori namn: ");
                            Category newCategory = new Category()
                            {
                                Name = newCategoryName,
                            };
                            await AddCategory(newCategory);
                            Console.WriteLine("La till kategori");
                            break;
                        case Buttons.RemoveCategory:
                            await WriteOutAllCategories();
                            int removeCategoryId = InputHelper.GetIntFromUser("Kategori id: ", true);
                            Category? removedCategory = await GetCategoryFromId(removeCategoryId);
                            if(removedCategory == null)
                            {
                                Console.WriteLine("Kategorin finns inte");
                                break;
                            }
                            try
                            {
                                await RemoveCategory(removeCategoryId);
                                Console.WriteLine("Kategori borttagen!");
                            } catch (Exception ex)
                            {
                                Console.WriteLine("Något gick fel :(");
                                Console.WriteLine("Kan vara att det finns produkter under denna kategori!");
                            }
                            
                            break;
                        case Buttons.ChangeCategory:
                            await WriteOutAllCategories();
                            int changeCategoryId = InputHelper.GetIntFromUser("Kategori id: ", true);
                            Category? changeCategory = await GetCategoryFromId(changeCategoryId);
                            if(changeCategory == null )
                            {
                                Console.WriteLine("Kategori finns inte");
                                break;
                            }

                            string changedCategoryName = InputHelper.GetStringFromUser("Nytt kategori namn: ");
                            changeCategory.Name = changedCategoryName;
                            await UpdateCategory(changeCategory);
                            Console.WriteLine("Kategori ändrades :)");
                            break;
                        case Buttons.ChangeUserInfo:
                            await WriteOutAllUsers();
                            int changeUserId = InputHelper.GetIntFromUser("Använderens id: ", true);
                            User? changeUser = await GetUserFromId(changeUserId);
                            if (changeUser == null)
                            {
                                Console.WriteLine("Använderen finns inte");
                                break;
                            }
                            if(changeUser.Id == Cookie.User.Id)
                            {
                                Console.WriteLine("Du kan inte ändra uppgifter på kontot du är inloggad på!");
                                break;
                            }
                            User changedUser = await ChangeUserValues(changeUser);
                            try
                            {
                                await UpdateUser(changedUser);
                                Console.WriteLine("Användarens uppgifter är ändrade");
                            } catch(Exception ex)
                            {
                                Console.WriteLine("Lyckades inte ändra användarens uppgifter");
                                break;
                            }
                            break;
                        case Buttons.ChangePaymentHistory:
                            await WriteOutAllUsers();
                            int changePaymentUserId = InputHelper.GetIntFromUser("Användarens id: ", true);
                            User? changePaymentUser = await GetUserFromId(changePaymentUserId);
                            if(changePaymentUser == null)
                            {
                                Console.WriteLine("Användaren finns inte");
                                break;
                            }
                            await WriteOutPaymentHistories(changePaymentUser);
                            int changePaymentId = InputHelper.GetIntFromUser("Betalning historik id: ", true);
                            PaymentHistory? changePayment = await GetPaymentHistory(changePaymentId, changePaymentUser.Id);
                            if(changePayment == null)
                            {
                                Console.WriteLine("Betalning historiken finns inte");
                                break;
                            }

                            PaymentHistory changedPayment = await ChangePaymentValues(changePayment);
                            try
                            {
                                await UpdatePayment(changedPayment);
                                Console.WriteLine("Ändrade betalning historiken!");
                            } catch (Exception ex)
                            {
                                Console.WriteLine("Något gick fel :(");
                            }
                            

                            break;
                        case Buttons.SendOrder:
                            await WriteOutAllUsers();
                            int sendUserId = InputHelper.GetIntFromUser("Användarens id: ", true);
                            User? sendUser = await GetUserFromId(sendUserId);
                            if(sendUser == null)
                            {
                                Console.WriteLine("Användaren finns inte");
                                break;
                            }
                            await WriteOutPaymentHistories(sendUser);
                            int sendPaymentId = InputHelper.GetIntFromUser("Bestälnings id: ", true);
                            PaymentHistory? sendPayment = await GetPaymentHistory(sendPaymentId, sendUserId);
                            if(sendPayment == null)
                            {
                                Console.WriteLine("Bestälningen finns inte");
                                break;
                            }
                            int? inventoryBalance = await GetStock(sendPayment.ProductId);
                            if(inventoryBalance == null)
                            {
                                Console.WriteLine("Hittade inte produkten som var i bestälningen");
                                break;
                            }
                            
                            if(inventoryBalance - sendPayment.Amount >= 0)
                            {
                                await RemoveStock(sendPayment.ProductId, sendPayment.Amount);
                                sendPayment.SendDate = DateTime.Now;
                                await UpdatePayment(sendPayment);
                                Console.WriteLine("Bestälning skickad");
                            } else
                            {
                                Console.WriteLine("Det finns inte tillräckligt med produkter i vårat lager saldo för denna bestälning!");
                            }

                                break;
                        case Buttons.Stats:
                            return new StatsController();


                        default:
                            page.Render();
                            break;
                    }
                }

            }
        }
        
        private async Task UpdateProduct(Product product)
        {
            using (var db = new ShopDbContext())
            {
                db.Products.Update(product);
                await db.SaveChangesAsync();
            }
        }
        private async Task<Product> ChangeProductValues(Product product)
        {
            Console.WriteLine("Namn (1)");
            Console.WriteLine("Kategori (2)");
            Console.WriteLine("Beskrivning (3)");
            Console.WriteLine("Levrantör (4)");
            Console.WriteLine("Produkt pris (5)");
            Console.WriteLine("lager saldo (6)");
            Console.WriteLine("Utvald för erbjudande (7)");

            int option = InputHelper.GetIntFromUser("Vad vill du ändra: ", true);

            switch (option)
            {
                case 1:
                    product.Name = InputHelper.GetStringFromUser("Ange nytt namn: ");
                    break;
                case 2:
                    await WriteOutAllCategories();
                    product.CategoryId = InputHelper.GetIntFromUser("Kategori id: ");
                    break;
                case 3:
                    product.Details = InputHelper.GetStringFromUser("Ny beskrivning: ");
                    break;
                case 4:
                    await WriteOutAllSuppliers();
                    product.SupplierId = InputHelper.GetIntFromUser("Levrantör id: ", true);
                    break;
                case 5:
                    product.Price = InputHelper.GetDecimalFromUser("Nytt pris på produkt: ");
                    break;
                case 6:
                    product.InventoryBalance = InputHelper.GetIntFromUser("Antal i lager saldo: ", true);
                    break;
                case 7:
                    product.IsSelected = InputHelper.GetBoolFromUser("Utvald för erbjudande ja(1) eller nej(2): ");
                    break;
                default:
                    return product;
            }
            return product;
        }
        private async Task<Product> GetProductInfoFromUser()
        {
            string productName = InputHelper.GetStringFromUser("Ange namn: ");

            await WriteOutAllCategories();
            int productCategoryId = InputHelper.GetIntFromUser("Kategori id:  ");

            string productDetails = InputHelper.GetStringFromUser("Produktens beskrivning: ");

            await WriteOutAllSuppliers();
            int productSupplierId = InputHelper.GetIntFromUser("Levrantör id: ");

            decimal productPrice = InputHelper.GetDecimalFromUser("Produktens pris: ");

            int productAmount = InputHelper.GetIntFromUser("Antal produkter i lager saldo: ");

            bool productSelected = InputHelper.GetBoolFromUser("Är denna produkt utvald för erbjudande, ja(1) nej(2): ");

            Product newProduct = new Product()
            {
                Name = productName,
                CategoryId = productCategoryId,
                Details = productDetails,
                SupplierId = productSupplierId,
                Price = productPrice,
                InventoryBalance = productAmount,
                IsSelected = productSelected,

            };
            return newProduct;
        }
        private async Task RemoveProduct(int id)
        {
            using (var db = new ShopDbContext())
            {
                Product? product = await GetProductFromId(id);
                if (product == null) return;
                db.Products.Remove(product);
                await db.SaveChangesAsync();
            }
        }
        private async Task AddProduct(Product newProduct)
        {
            using (var db = new ShopDbContext())
            {
                await db.Products.AddAsync(newProduct);
                await db.SaveChangesAsync();
            }
        }
        private async Task WriteOutAllSuppliers()
        {
            using (var db = new ShopDbContext())
            {
                foreach (var supplier in await db.Suppliers.ToArrayAsync())
                {
                    Console.WriteLine($"{supplier.Id}: {supplier.Name}");
                }
            }
        }
        private async Task WriteOutAllCategories()
        {
            using (var db = new ShopDbContext())
            {
                foreach (var category in await db.Categories.ToArrayAsync())
                {
                    Console.WriteLine($"{category.Id}: {category.Name}");
                }
            }
        }
        private async Task WriteOutAllProducts()
        {
            using (var db = new ShopDbContext())
            {
                foreach (var product in await db.Products.ToArrayAsync())
                {
                    Console.WriteLine($"{product.Id}: {product.Name}");
                }
            }
        }
        private async Task<Product?> GetProductFromId(int id)
        {
            using (var db = new ShopDbContext())
            {
                return await db.Products.FindAsync(id);
            }
        }
        private async Task AddCategory(Category category)
        {
            using (var db = new ShopDbContext())
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
            }
        }
        private async Task RemoveCategory(int id)
        {
            using (var db = new ShopDbContext())
            {
                var category = await db.Categories.FindAsync(id);
                if (category == null) return;
                db.Categories.Remove(category);
                await db.SaveChangesAsync();
            }
        }
        private async Task UpdateCategory(Category category)
        {
            using (var db = new ShopDbContext())
            {
                db.Categories.Update(category);
                await db.SaveChangesAsync();
            }
        }
        private async Task<Category?> GetCategoryFromId(int id)
        {
            using (var db = new ShopDbContext())
            {
                return await db.Categories.FindAsync(id);
            }
        }
        private async Task WriteOutAllUsers()
        {
            using (var db = new ShopDbContext())
            {
                foreach (var user in await db.Users.ToArrayAsync())
                {
                    Console.WriteLine($"{user.Id}: {user.Email}");
                }
            }
        }
        private async Task<User?> GetUserFromId(int id)
        {
            using (var db = new ShopDbContext())
            {
                return await db.Users.FindAsync(id);
            }
        }
        private async Task<User> ChangeUserValues(User user)
        {
            Console.WriteLine("Förnamn (1)");
            Console.WriteLine("Efternamn (2)");
            Console.WriteLine("Gatuadress (3)");
            Console.WriteLine("Stad (4)");
            Console.WriteLine("Telefon nummer (5)");
            Console.WriteLine("Email (6)");
            Console.WriteLine("Lösenord (7)");
            Console.WriteLine("Födelsdatum (8)");
            Console.WriteLine("Ändra till admin använare (9)");

            int option = InputHelper.GetIntFromUser("Vad vill du ändra: ");

            switch (option)
            {
                case 1:
                    user.FirstName = InputHelper.GetStringFromUser("Ange nytt förnamn: ");
                    break;
                case 2:
                    user.LastName = InputHelper.GetStringFromUser("Ange nytt efternamn: ");
                    break;
                case 3:
                    user.StreetName = InputHelper.GetStringFromUser("Ange ny Gatuadress: ");
                    break;
                case 4:
                    await WriteOutAllCities();
                    user.CityId = InputHelper.GetIntFromUser("Levrantör id: ");
                    break;
                case 5:
                    user.PhoneNumber = InputHelper.GetStringFromUser("Ange nytt telefon nummer: ");
                    break;
                case 6:
                    user.Email = InputHelper.GetStringFromUser("Ange ny email: ");
                    break;
                case 7:
                    user.Password = InputHelper.GetStringFromUser("Ange nytt lösenord: ");
                    break;
                case 8:
                    user.BirthDate = InputHelper.GetDateTimeFromUser();
                    break;
                case 9:
                    user.IsAdmin = InputHelper.GetBoolFromUser("Ska användaren vara admin, ja(1) nej(2): ");
                    break;

                default:
                    return user;
            }
            return user;

        }
        private async Task WriteOutAllCities()
        {
            using (var db = new ShopDbContext())
            {
                foreach(var city in await db.Cities.ToListAsync())
                {
                    Console.WriteLine($"{city.Id}: {city.Name}");
                }
            }
        }
        private async Task UpdateUser(User user)
        {
            using (var db = new ShopDbContext())
            {
                db.Users.Update(user);
                await db.SaveChangesAsync();
            }
        }
        private async Task WriteOutPaymentHistories(User user)
        {
            using (var db = new ShopDbContext())
            {
                PaymentHistory[] paymentHistories = await db.PaymentHistories
                    .Include(p => p.Product)
                    .Where(p => p.UserId == user.Id)
                    .ToArrayAsync();
                foreach (var paymentHistory in paymentHistories)
                {
                    string sendDate = paymentHistory.SendDate.Year == 1 ? "Inte skickad" : paymentHistory.SendDate.ToString();
                    Console.WriteLine($"{paymentHistory.Id}: {user.Email}, {paymentHistory.Product.Name}, Antal:{paymentHistory.Amount}, Betalad:{paymentHistory.PayedDate}, Skickad: {sendDate}");
                }
            }
        }
        private async Task<PaymentHistory?> GetPaymentHistory(int id, int userId)
        {
            using (var db = new ShopDbContext())
            {
                return await db.PaymentHistories
                    .Where(p => p.Id == id && p.UserId == userId)
                    .SingleOrDefaultAsync();
            }
        }
        private async Task<PaymentHistory> ChangePaymentValues(PaymentHistory paymentHistory)
        {
            Console.WriteLine("Betalnings metod(1)");
            Console.WriteLine("Produkt(2)");
            Console.WriteLine("Användare(3)");
            Console.WriteLine("Antal(4)");
            Console.WriteLine("Leverans gatuadress(5)");
            Console.WriteLine("Leverans stad(6)");
            Console.WriteLine("Fraktmetod(7)");
            Console.WriteLine("Betald tid(8)");
            Console.WriteLine("Skickad tid(9)");

            int option = InputHelper.GetIntFromUser("Vad vill du ändra: ");

            switch(option)
            {
                case 1:
                    await WriteOutPaymentMetods();
                    paymentHistory.PaymentOptionId = InputHelper.GetIntFromUser("Betalning metod id: ");
                    break;
                case 2:
                    await WriteOutAllProducts();
                    paymentHistory.ProductId = InputHelper.GetIntFromUser("Produkt id: ");
                    break;
                case 3:
                    await WriteOutAllUsers();
                    paymentHistory.UserId = InputHelper.GetIntFromUser("Användaren id: ");
                    break;
                case 4:
                    paymentHistory.Amount = InputHelper.GetIntFromUser("Antal produkter köpta: ");
                    break;
                case 5:
                    paymentHistory.DeliveryStreet = InputHelper.GetStringFromUser("Leverans gatuadress: ");
                    break;
                case 6:
                    await WriteOutAllCities();
                    paymentHistory.DeliveryCityId = InputHelper.GetIntFromUser("Stads id: ");
                    break;
                case 7:
                    await WriteOutDeliveryOptions();
                    paymentHistory.DeliveryOptionId = InputHelper.GetIntFromUser("Fraktmetod id: ");
                    break;
                case 8:
                    paymentHistory.PayedDate = InputHelper.GetDateTimeFromUser();
                    break;
                case 9:
                    paymentHistory.SendDate = InputHelper.GetDateTimeFromUser();
                    break;
                default:
                    return paymentHistory;
            }
            return paymentHistory;

        }
        private async Task WriteOutPaymentMetods()
        {
            using (var db = new ShopDbContext())
            {
                foreach(var paymentOption in await db.PaymentOptions.ToArrayAsync())
                {
                    Console.WriteLine($"{paymentOption.Id}: {paymentOption.Name}");
                }
                
            }
        }
        private async Task WriteOutDeliveryOptions()
        {
            using (var db = new ShopDbContext())
            {
                foreach (var deliveryOption in await db.DeliveryOptions.ToArrayAsync())
                {
                    Console.WriteLine($"{deliveryOption.Id}: {deliveryOption.Name}");
                }

            }
        }
        private async Task UpdatePayment(PaymentHistory paymentOption)
        {
            using(var db = new ShopDbContext())
            {
                db.PaymentHistories.Update(paymentOption);
                await db.SaveChangesAsync();
            }
        }
        private async Task<int?> GetStock(int productId)
        {
            using(var db = new ShopDbContext())
            {
                var product = await db.Products.FindAsync(productId);
                if(product == null) return null;
                return product.InventoryBalance;
            }
        }
        private async Task RemoveStock(int productId, int amount)
        {
            using(var db = new ShopDbContext())
            {
                var product = await db.Products.FindAsync(productId);
                if(product == null) return;
                product.InventoryBalance -= amount;
                await db.SaveChangesAsync();
            }
        }


    }
}
