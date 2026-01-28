using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
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
        private readonly DatabaseActions<Product> _productDb = new();
        private readonly DatabaseActions<Supplier> _supplierDb = new();
        private readonly DatabaseActions<Category> _categoryDb = new();
        private readonly DatabaseActions<User> _userDb = new();
        private readonly DatabaseActions<PaymentHistory> _paymentDb = new();
        public async Task<IController> ActivateController()
        {

            AdminPage page = new AdminPage();

            while (true)
            {
                int? option = InputHelper.GetIntFromUser("", true);

                if (option == null)
                {
                    page.Render();
                    continue;
                }

                option -= 1;

                switch ((Buttons)option)
                {
                    case Buttons.HomePage:
                        return new HomePageController();

                    case Buttons.AddProduct:
                        await AddProductFlow();
                        break;

                    case Buttons.RemoveProduct:
                        await RemoveProductFlow();
                        break;

                    case Buttons.ChangeProduct:
                        await ChangeProductFlow();
                        break;

                    case Buttons.AddSupplier:
                        await AddSupplierFlow();
                        break;

                    case Buttons.RemoveSupplier:
                        await RemoveSupplierFlow();
                        break;

                    case Buttons.ChangeSupplier:
                        await ChangeSupplierFlow();
                        break;

                    case Buttons.AddCategory:
                        await AddCategoryFlow();
                        break;

                    case Buttons.RemoveCategory:
                        await RemoveCategoryFlow();
                        break;

                    case Buttons.ChangeCategory:
                        await ChangeCategoryFlow();
                        break;

                    case Buttons.ChangeUserInfo:
                        await ChangeUserFlow();
                        break;

                    case Buttons.ChangePaymentHistory:
                        await ChangePaymentFlow();
                        break;

                    case Buttons.SendOrder:
                        await SendOrderFlow();
                        break;

                    case Buttons.Stats:
                        return new StatsController();

                    default:
                        page.Render();
                        break;
                }
            }
        }
        // Produkt Flow
        private async Task AddProductFlow()
        {
            Product product = await GetProductInfoFromUser();

            try
            {
                await _productDb.Execute(DbAction.Add, product);
                await AddMongoProduct(product.Id, Cookie.User.Id);
                Console.WriteLine("Ny produkt har lagts till :)");
            }
            catch
            {
                Console.WriteLine("Något gick fel!");
            }
        }

        private async Task RemoveProductFlow()
        {
            await WriteOutAllProducts();
            int id = InputHelper.GetIntFromUser("Ange produkt id: ", true);

            Product? product = await _productDb.GetById(id);
            if (product == null)
            {
                Console.WriteLine("Produkten finns inte");
                return;
            }

            try
            {
                await _productDb.Execute(DbAction.Remove, product);
                await RemoveMongoProduct(id);
                Console.WriteLine("Produkten borttagen :)");
            }
            catch
            {
                Console.WriteLine("Produkten kan inte tas bort");
            }
        }

        private async Task ChangeProductFlow()
        {
            await WriteOutAllProducts();
            int id = InputHelper.GetIntFromUser("Ange produkt id: ", true);

            Product? product = await _productDb.GetById(id);
            if (product == null)
            {
                Console.WriteLine("Produkten finns inte");
                return;
            }

            Product updated = await ChangeProductValues(product);

            try
            {
                await _productDb.Execute(DbAction.Update, updated);
                await ChangeMongoProduct(updated.Id);
                Console.WriteLine("Produkten har ändrats :)");
            }
            catch
            {
                Console.WriteLine("Något gick fel");
            }
        }

        // Levrantör Flow

        private async Task AddSupplierFlow()
        {
            string name = InputHelper.GetStringFromUser("Levrantörens namn: ");
            await _supplierDb.Execute(DbAction.Add, new Supplier { Name = name });
            Console.WriteLine("La till levrantör");
        }

        private async Task RemoveSupplierFlow()
        {
            await WriteOutAllSuppliers();
            int id = InputHelper.GetIntFromUser("Levrantör id: ", true);

            Supplier? supplier = await _supplierDb.GetById(id);
            if (supplier == null)
            {
                Console.WriteLine("Levrantören finns inte");
                return;
            }

            try
            {
                await _supplierDb.Execute(DbAction.Remove, supplier);
                Console.WriteLine("Levrantören borttagen!");
            }
            catch
            {
                Console.WriteLine("Kan finnas produkter kopplade till levrantören");
            }
        }

        private async Task ChangeSupplierFlow()
        {
            await WriteOutAllSuppliers();
            int id = InputHelper.GetIntFromUser("Levrantör id: ", true);

            Supplier? supplier = await _supplierDb.GetById(id);
            if (supplier == null)
            {
                Console.WriteLine("Levrantören finns inte");
                return;
            }

            supplier.Name = InputHelper.GetStringFromUser("Nytt levrantörs namn: ");
            await _supplierDb.Execute(DbAction.Update, supplier);
            Console.WriteLine("Levrantören ändrades :)");
        }

        // Kategori Flow

        private async Task AddCategoryFlow()
        {
            string name = InputHelper.GetStringFromUser("Kategori namn: ");
            await _categoryDb.Execute(DbAction.Add, new Category { Name = name });
            Console.WriteLine("La till kategori");
        }

        private async Task RemoveCategoryFlow()
        {
            await WriteOutAllCategories();
            int id = InputHelper.GetIntFromUser("Kategori id: ", true);

            Category? category = await _categoryDb.GetById(id);
            if (category == null)
            {
                Console.WriteLine("Kategorin finns inte");
                return;
            }

            try
            {
                await _categoryDb.Execute(DbAction.Remove, category);
                Console.WriteLine("Kategori borttagen!");
            }
            catch
            {
                Console.WriteLine("Kan finnas produkter under denna kategori");
            }
        }

        private async Task ChangeCategoryFlow()
        {
            await WriteOutAllCategories();
            int id = InputHelper.GetIntFromUser("Kategori id: ", true);

            Category? category = await _categoryDb.GetById(id);
            if (category == null)
            {
                Console.WriteLine("Kategori finns inte");
                return;
            }

            category.Name = InputHelper.GetStringFromUser("Nytt kategori namn: ");
            await _categoryDb.Execute(DbAction.Update, category);
            Console.WriteLine("Kategori ändrades :)");
        }

        // Användare Flow

        private async Task ChangeUserFlow()
        {
            await WriteOutAllUsers();
            int id = InputHelper.GetIntFromUser("Användarens id: ", true);

            User? user = await _userDb.GetById(id);
            if (user == null || user.Id == Cookie.User.Id)
            {
                Console.WriteLine("Ogiltig användare");
                return;
            }

            User updated = await ChangeUserValues(user);

            await _userDb.Execute(DbAction.Update, updated);

            if (updated.Email != user.Email)
                await UpdateMongoUserEmail(user.Email, updated.Email);

            Console.WriteLine("Användarens uppgifter är ändrade");
        }

        // Betalning Flow

        private async Task ChangePaymentFlow()
        {
            await WriteOutAllUsers();
            int userId = InputHelper.GetIntFromUser("Användarens id: ", true);

            User? user = await _userDb.GetById(userId);
            if (user == null) return;

            await WriteOutPaymentHistories(user);
            int paymentId = InputHelper.GetIntFromUser("Betalning historik id: ", true);

            PaymentHistory? payment = await GetPaymentHistory(paymentId, userId);
            if (payment == null) return;

            PaymentHistory updated = await ChangePaymentValues(payment);
            await _paymentDb.Execute(DbAction.Update, updated);

            Console.WriteLine("Ändrade betalning historiken!");
        }

        private async Task SendOrderFlow()
        {
            await WriteOutAllUsersWithOrders();
            int userId = InputHelper.GetIntFromUser("Användarens id: ", true);

            User? user = await _userDb.GetById(userId);
            if (user == null) return;

            await WriteOutPaymentHistories(user);
            int paymentId = InputHelper.GetIntFromUser("Beställnings id: ", true);

            PaymentHistory? payment = await GetPaymentHistory(paymentId, userId);
            if (payment == null) return;

            int? stock = await GetStock(payment.ProductId);
            if (stock == null || stock - payment.Amount < 0)
            {
                Console.WriteLine("Otillräckligt lagersaldo");
                return;
            }

            await RemoveStock(payment.ProductId, payment.Amount);
            payment.SendDate = DateTime.Now;
            await _paymentDb.Execute(DbAction.Update, payment);

            Console.WriteLine("Beställning skickad");
        }

        // MongoDb metoder
        private async Task RemoveMongoProduct(int productId)
        {
            var filter = Builders<Models.AddedProduct>.Filter.Eq(p => p.ProductId, productId);
            await MongoConnection.GetAddedProductCollection().DeleteOneAsync(filter);
        }
        private async Task ChangeMongoProduct(int productId)
        {
            AddedProduct? addedProduct = MongoConnection.GetAddedProductCollection().AsQueryable().SingleOrDefault(ap => ap.ProductId == productId);
            if (addedProduct == null) return;
            addedProduct.LastChangeDate = DateTime.Now;
            addedProduct.ChangeAdminId = Cookie.User.Id;
            
            await MongoConnection.GetAddedProductCollection().ReplaceOneAsync(ap => ap.Id == addedProduct.Id, addedProduct);
            
        }
        private async Task AddMongoProduct(int productId, int adminId)
        {
            AddedProduct addedProductEvent = new AddedProduct()
            {
                ProductId = productId,
                AdminId = adminId,
                AddedDate = DateTime.Now
            };

            await MongoConnection.GetAddedProductCollection().InsertOneAsync(addedProductEvent);
        }
        private async Task UpdateMongoUserEmail(string oldEmail, string newEmail)
        {
            LoginLog? loginLog = MongoConnection.GetLoginLogCollection().AsQueryable().SingleOrDefault(la => la.Email == oldEmail);
            if (loginLog == null) return;
            loginLog.Email = newEmail;
            await MongoConnection.GetLoginLogCollection().ReplaceOneAsync(la => la.Id == loginLog.Id, loginLog);
        }

        // Metoder som hjälper användare ändra värden på objekt
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
        private async Task<User> ChangeUserValues(User user)
        {
            // ser till att objektet vi skickas in inte ändras
            User returnUser = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                StreetName = user.StreetName,
                CityId = user.CityId,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Password = user.Password,
                BirthDate = user.BirthDate,
                IsAdmin = user.IsAdmin
            };
            Console.WriteLine("Förnamn (1)");
            Console.WriteLine("Efternamn (2)");
            Console.WriteLine("Kön (3)");
            Console.WriteLine("Gatuadress (4)");
            Console.WriteLine("Stad (5)");
            Console.WriteLine("Telefon nummer (6)");
            Console.WriteLine("Email (7)");
            Console.WriteLine("Lösenord (8)");
            Console.WriteLine("Födelsdatum (9)");
            Console.WriteLine("Ändra till admin använare (10)");

            int option = InputHelper.GetIntFromUser("Vad vill du ändra: ");


            switch (option)
            {
                case 1:
                    returnUser.FirstName = InputHelper.GetStringFromUser("Ange nytt förnamn: ");
                    break;
                case 2:
                    returnUser.LastName = InputHelper.GetStringFromUser("Ange nytt efternamn: ");
                    break;
                case 3:
                    string? input = InputHelper.GetGenderFromUser("Ange kön, Kvinna(1) Man(2) Ickebinär(3): ");
                    if (input != null)
                    {
                        returnUser.Gender = input;
                    }
                    break;
                case 4:
                    returnUser.StreetName = InputHelper.GetStringFromUser("Ange ny Gatuadress: ");
                    break;
                case 5:
                    await WriteOutAllCities();
                    returnUser.CityId = InputHelper.GetIntFromUser("Levrantör id: ");
                    break;
                case 6:
                    returnUser.PhoneNumber = InputHelper.GetStringFromUser("Ange nytt telefon nummer: ");
                    break;
                case 7:
                    returnUser.Email = InputHelper.GetStringFromUser("Ange ny email: ");
                    break;
                case 8:
                    returnUser.Password = InputHelper.GetStringFromUser("Ange nytt lösenord: ");
                    break;
                case 9:
                    returnUser.BirthDate = InputHelper.GetDateTimeFromUser();
                    break;
                case 10:
                    returnUser.IsAdmin = InputHelper.GetBoolFromUser("Ska användaren vara admin, ja(1) nej(2): ");
                    break;

                default:
                    return returnUser;
            }
            return returnUser;

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

            switch (option)
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

        // Utskrivft metoder
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
        private async Task WriteOutAllUsersWithOrders()
        {
            using (var db = new ShopDbContext())
            {
                var usersWithOrders = await db.Users
                    .Where(u => u.PaymentHistories.Any())
                    .Select(u => new
                    {
                        u.Id,
                        u.Email
                    })
                    .ToListAsync();

                foreach (var user in usersWithOrders)
                {
                    Console.WriteLine($"{user.Id}: {user.Email}");
                }
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
        private async Task WriteOutAllCities()
        {
            using (var db = new ShopDbContext())
            {
                foreach (var city in await db.Cities.ToListAsync())
                {
                    Console.WriteLine($"{city.Id}: {city.Name}");
                }
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
        private async Task WriteOutPaymentMetods()
        {
            using (var db = new ShopDbContext())
            {
                foreach (var paymentOption in await db.PaymentOptions.ToArrayAsync())
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

        
        
         // Hjälp metoder
        private async Task<PaymentHistory?> GetPaymentHistory(int id, int userId)
        {
            using (var db = new ShopDbContext())
            {
                return await db.PaymentHistories
                    .Where(p => p.Id == id && p.UserId == userId)
                    .SingleOrDefaultAsync();
            }
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
