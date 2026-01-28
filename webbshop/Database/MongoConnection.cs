using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.Database
{
    internal class MongoConnection
    {
        private static MongoClient GetClient()
        {

            string connectionString = DatabaseStrings.MongoConnectionString;
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            var client = new MongoClient(settings);
            return client;
        }

        public static IMongoCollection<Models.LoginLog> GetLoginLogCollection()
        {
            var client = GetClient();

            var dataBase = client.GetDatabase("WebShop");

            var loginLogCollection = dataBase.GetCollection<Models.LoginLog>("LoginLogs");

            return loginLogCollection;
        }
        public static IMongoCollection<Models.AddedProduct> GetAddedProductCollection()
        {
            var client = GetClient();

            var dataBase = client.GetDatabase("WebShop");

            var addedProductCollection = dataBase.GetCollection<Models.AddedProduct>("AddedProduct");

            return addedProductCollection;
        }
    }
}
