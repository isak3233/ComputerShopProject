using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace webbshop
{
    internal class MongoConnection
    {
        private static MongoClient GetClient()
        {
            string connectionString = "mongodb+srv://kasity:Kebabrulle123@isakcluster.bzlbau2.mongodb.net/?appName=IsakCluster";
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            var client = new MongoClient(settings);
            return client;
        }

        public static IMongoCollection<Models.LoginAttempt> GetLoginAttemptCollection()
        {
            var client = GetClient();

            var dataBase = client.GetDatabase("WebShop");

            var loginAttemptCollection = dataBase.GetCollection<Models.LoginAttempt>("LoginAttempts");

            return loginAttemptCollection;
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
