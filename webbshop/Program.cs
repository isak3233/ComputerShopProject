using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using webbshop.Database;
using webbshop.UI;

namespace webbshop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //admin@shop.com
            //Password123!

            //kund@shop.com
            //Password123!

            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
            DatabaseStrings.SqlConnectionString = config["SqlConnectionString"];
            DatabaseStrings.MongoConnectionString = config["MongoConnectionString"];


            WebShop webShop = new WebShop();
            await webShop.StartWebShop();
        }
    }
}