using Dapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static webbshop.UI.StatsPage;
using webbshop.Database;

namespace webbshop.Controller
{
    internal class StatsController : IController
    {
        // För dapper
        public async Task<IController> ActivateController()
        {
            StatsPage page = new StatsPage();


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
                        case Buttons.AdminPanel:
                            return new AdminController();
                        case Buttons.MostSoldInCountry:
                            WriteOutMostSoldInCountry(await GetPaymentHistory());
                            break;
                        case Buttons.MostProfitableProducts:
                            WriteOutMostProfitableProducts(await GetPaymentHistory());
                            break;
                        case Buttons.SalesPerCategory:
                            WriteOutSalesPerCategory(await GetPaymentHistory());
                            break;
                        case Buttons.GenderBuyProduct:
                            WriteOutGenderProductRatio(await GetPaymentHistory());
                            break;
                        case Buttons.BestCustomers:
                            WriteOutBestCustomers(await GetPaymentHistory());
                            break;
                        case Buttons.CountriesWithBestCustomers:
                            WriteOutCountryWithBestCustomers(await GetPaymentHistory());
                            break;
                        case Buttons.AverageAgeProducts:
                            WriteOutAverageAgeOnProducts(await GetPaymentHistory());
                            break;
                        case Buttons.AccountSecurity:
                            await WriteOutUserSecurity();
                            break;
                        case Buttons.ProductAdded:
                            await WriteOutProductAddedLogs();
                            break;
                        case Buttons.OnlineUsers:
                            await WriteOutOnlineUsers();
                            break;
                        default:
                            page.Render();
                            break;
                    }
                }

            }
        }
        
        public enum SecurityLevel
        {
            Secure,
            Unsecure,
            VeryUnsecure
        }
        private async Task<User[]> GetUsersFromIds(int[] userIds)
        {
            using (var db = new ShopDbContext())
            {
                return await db.Users.Where(u => userIds.Contains(u.Id)).ToArrayAsync();
            }
        }
        private async Task<Product[]> GetProductsFromIds(int[] productIds)
        {
            using (var db = new ShopDbContext())
            {
                return await db.Products.Where(p => productIds.Contains(p.Id)).ToArrayAsync();
            }
        }
        
        
        private async Task<User[]> GetUsersFromEmails(string[] emails)
        {
            using (var db = new ShopDbContext())
            {
                return await db.Users.Where(u => emails.Contains(u.Email)).ToArrayAsync();
            }
        }
        private SecurityLevel CalculateSecurity(LoginLog attempt)
        {
            if (attempt.LoginAttemptsAmount < 5)
            {
                return SecurityLevel.Secure;
            }

            double failRatio = (double)attempt.FailedLoginAttempts / attempt.LoginAttemptsAmount * 100;

            if (failRatio < 40)
            {
                return SecurityLevel.Secure;
            }
            else if (failRatio < 60)
            {
                return SecurityLevel.Unsecure;
            }
            else
            {
                return SecurityLevel.VeryUnsecure;
            }
                
        }
        private async Task<PaymentHistory[]> GetPaymentHistory()
        {
            using (var db = new ShopDbContext())
            {
                return await db.PaymentHistories
                    .Include(ph => ph.Product)
                        .ThenInclude(p => p.Category)
                    .Include(ph => ph.DeliveryCity)
                        .ThenInclude(c => c.Country)
                    .Include(ph => ph.User)
                    .ToArrayAsync();
            }
        }

        // Utskrift metoder
        private async Task WriteOutUserSecurity()
        {
            LoginLog[] loginLogs = MongoConnection.GetLoginLogCollection().AsQueryable().ToArray();
            string[] emails = loginLogs.Select(la => la.Email).ToArray();
            User[] users = await GetUsersFromEmails(emails);


            foreach (var user in users)
            {
                LoginLog? loginLog = loginLogs.Where(la => la.Email == user.Email).SingleOrDefault();
                if (loginLog != null)
                {
                    SecurityLevel securityLvl = CalculateSecurity(loginLog);
                    string securityString;
                    switch (securityLvl)
                    {
                        case SecurityLevel.Unsecure:
                            securityString = "Osäker";
                            break;
                        case SecurityLevel.VeryUnsecure:
                            securityString = "Väldigt osäkert";
                            break;
                        default:
                            securityString = "Säker";
                            break;

                    }
                    Console.WriteLine($"{user.Email} | Konto säkerhet: {securityString} | Inloggning försök: {loginLog.LoginAttemptsAmount} | Inloggning försök som misslyckats: {loginLog.FailedLoginAttempts}");
                }
            }
        }
        private async Task WriteOutOnlineUsers()
        {
            var loginLogs = MongoConnection.GetLoginLogCollection().AsQueryable().ToArray();
            List<string> onlineUserEmails = new List<string>();
            foreach (var loginLog in loginLogs)
            {
                if (DateTime.UtcNow < loginLog.LoginSessionExpire)
                {
                    onlineUserEmails.Add(loginLog.Email);
                }
            }
            User[] onlineUsers;
            using (var db = new ShopDbContext())
            {
                onlineUsers = await db.Users.Where(u => onlineUserEmails.Contains(u.Email)).ToArrayAsync();
            }
            Console.WriteLine("Användare online: ");
            foreach (var onlineUser in onlineUsers)
            {
                Console.WriteLine($"{onlineUser.FirstName} {onlineUser.LastName} | {onlineUser.Email} | Användar id: {onlineUser.Id}");
            }

        }
        private async Task WriteOutProductAddedLogs()
        {
            AddedProduct[] addedProductLogs = MongoConnection.GetAddedProductCollection().AsQueryable().ToArray();

            int[] adminIds = addedProductLogs
                .SelectMany(ap => ap.ChangeAdminId.HasValue
                    ? new[] { ap.AdminId, ap.ChangeAdminId.Value }
                    : new[] { ap.AdminId })
                .Distinct()
                .ToArray();

            int[] productIds = addedProductLogs.Select(ap => ap.ProductId).ToArray();

            // Ser till att vi startar båda metoderna samtidigt så dom inte väntar efter varandra
            var adminsTask = GetUsersFromIds(adminIds);
            var productsTask = GetProductsFromIds(productIds);

            // Väntar tills båda är klara
            await Task.WhenAll(adminsTask, productsTask);

            User[] admins = adminsTask.Result;
            Product[] products = productsTask.Result;



            foreach (var addedProductLog in addedProductLogs)
            {
                User? admin = admins.Where(a => a.Id == addedProductLog.AdminId).SingleOrDefault();
                Product? product = products.Where(p => p.Id == addedProductLog.ProductId).SingleOrDefault();
                User? changeAdmin = admins.Where(a => a.Id == addedProductLog.ChangeAdminId).SingleOrDefault();
                string latestChangeString = addedProductLog.LastChangeDate.Year == 1 ? "" : "Senast ändrad " + addedProductLog.LastChangeDate + " av " + changeAdmin.Email;
                if (admin != null && product != null)
                {

                    Console.WriteLine($"{admin.Id}: {admin.Email}: La till produkten '{product.Name}' datumet {addedProductLog.AddedDate}, {latestChangeString}");
                }

            }
        }
        private void WriteOutAverageAgeOnProducts(PaymentHistory[] paymentHistories)
        {
            using var connection = new SqlConnection(DatabaseStrings.SqlConnectionString);
            connection.Open();

            var result = connection.Query(@"
                SELECT 
                    p.Name AS Product,
                    AVG(DATEDIFF(DAY, u.BirthDate, GETDATE()) / 365.25) AS AverageAge,
                    FLOOR(MIN(DATEDIFF(DAY, u.BirthDate, GETDATE()) / 365.25)) AS MinAge,
                    FLOOR(MAX(DATEDIFF(DAY, u.BirthDate, GETDATE()) / 365.25)) AS MaxAge
                FROM PaymentHistories ph
                INNER JOIN Users u ON ph.UserId = u.Id
                INNER JOIN Products p ON ph.ProductId = p.Id
                GROUP BY p.Name
                ORDER BY AVG(DATEDIFF(DAY, u.BirthDate, GETDATE()) / 365.25) DESC;
            ");

            foreach (var product in result)
            {
                Console.WriteLine($"{product.Product}: Medel: {product.AverageAge:F1} år | Min: {product.MinAge} år | Max: {product.MaxAge} år");
            }
            

        }
        private void WriteOutCountryWithBestCustomers(PaymentHistory[] paymentHistories)
        {
            using var connection = new SqlConnection(DatabaseStrings.SqlConnectionString);
            connection.Open();

            var result = connection.Query(@"
                SELECT
                    Country,
                    AVG(CustomerRevenue) AS AverageRevenuePerCustomer
                FROM
                (
                    SELECT 
                        co.Name AS Country,
                        u.Id AS UserId,
                        SUM(ph.Amount * p.Price) AS CustomerRevenue
                    FROM PaymentHistories ph
                    INNER JOIN Users u ON ph.UserId = u.Id
                    INNER JOIN Products p ON ph.ProductId = p.Id
                    INNER JOIN Cities c ON ph.DeliveryCityId = c.Id
                    INNER JOIN Countries co ON c.CountryId = co.Id
                    GROUP BY co.Name, u.Id
                ) AS CustomerTotals
                GROUP BY Country
                ORDER BY AverageRevenuePerCustomer DESC;
            ");


            foreach (var country in result)
            {
                Console.WriteLine($"{country.Country}: Medelintäkt per kund: {country.AverageRevenuePerCustomer:C}");
            }
        }
        private void WriteOutBestCustomers(PaymentHistory[] paymentHistory)
        {
            var topCustomers =
                paymentHistory
                    .GroupBy(ph => ph.User)
                    .Select(g => new
                    {
                        Customer = g.Key.FirstName + " " + g.Key.LastName,
                        TotalSpent = g.Sum(x => x.Amount * x.Product.Price)
                    })
                    .OrderByDescending(x => x.TotalSpent)
                    .Take(10)
                    .ToArray();
            int index = 1;
            foreach(var topCustomer in topCustomers)
            {
                Console.WriteLine($"\t{index}: {topCustomer.Customer} | Spenderat Totalt: {topCustomer.TotalSpent:C}");
                index++;
            }
        }
        private void WriteOutGenderProductRatio(PaymentHistory[] paymentHistories)
        {
            using (var db = new ShopDbContext())
            {
                var genderBuyRatio =
                    paymentHistories
                        .GroupBy(ph => ph.Product)
                        .Select(g =>
                        {
                            var total = g.Count();

                            return new
                            {
                                Product = g.Key.Name,

                                FemaleRatio = g.Count(x => x.User.Gender == "Kvinna") / (double)total,
                                MaleRatio = g.Count(x => x.User.Gender == "Man") / (double)total,
                                NonBinaryRatio = g.Count(x => x.User.Gender == "Ickebinär") / (double)total
                            };
                        })
                        .ToArray();
                foreach (var product in genderBuyRatio)
                {
                    Console.WriteLine($"{product.Product}: Kvinnor: {(int)(product.FemaleRatio * 100)}% Män: {(int)(product.MaleRatio * 100)}% Ickebinär: {(int)(product.NonBinaryRatio * 100)}%");
                }
            }
           
        }
        private void WriteOutSalesPerCategory(PaymentHistory[] paymentHistories)
        {
            var salesPerCategory =
                paymentHistories
                    .GroupBy(ph => ph.Product.Category.Name)
                    .Select(g => new
                    {
                        Category = g.Key,
                        TotalSold = g.Sum(x => x.Amount),
                        Revenue = g.Sum(x => x.Amount * x.Product.Price)
                    }).ToArray();
            foreach(var saleOnCategory in salesPerCategory)
            {
                Console.WriteLine($"\t{saleOnCategory.Category} | Totalt sålt: {saleOnCategory.TotalSold} | Intäkter: {saleOnCategory.Revenue:C}");
            }

        }
        private void WriteOutMostProfitableProducts(PaymentHistory[] paymentHistories)
        {
            var mpPaymentHistories = paymentHistories
                                .GroupBy(ph => ph.Product.Name)
                                .Select(g => new
                                {
                                    Product = g.Key,
                                    Revenue = g.Sum(x => x.Amount * x.Product.Price)
                                })
                                .OrderByDescending(x => x.Revenue)
                                .ToArray();
            int index = 1;
            foreach (var mostProfitableProducts in mpPaymentHistories)
            {

                Console.WriteLine($"\t{index}: {mostProfitableProducts.Product} | Intäkter: {mostProfitableProducts.Revenue:C}");
                index++;
            }
        }
        private void WriteOutMostSoldInCountry(PaymentHistory[] paymentHistory)
        {
            var mostSoldInCountry = paymentHistory
                                .GroupBy(ph => new
                                {
                                    Country = ph.DeliveryCity.Country.Name,
                                    Product = ph.Product.Name
                                })
                                .Select(g => new
                                {
                                    g.Key.Country,
                                    g.Key.Product,
                                    TotalSold = g.Sum(x => x.Amount)
                                })
                                .GroupBy(x => x.Country)
                                .SelectMany(g => g
                                    .OrderByDescending(x => x.TotalSold)
                                    .Take(3))
                                .ToArray();
            var groupedByCountry = mostSoldInCountry
                .OrderBy(x => x.Country)
                .ThenByDescending(x => x.TotalSold)
                .GroupBy(x => x.Country);


            foreach (var countryGroup in groupedByCountry)
            {
                Console.WriteLine($"\n\t{countryGroup.Key}:");
                int index = 1;
                foreach (var product in countryGroup)
                {
                    Console.WriteLine($"\t\t{index}: {product.Product} | Antal sålda: {product.TotalSold}");
                    index++;
                }

                Console.WriteLine();
            }
        }

    }
}
