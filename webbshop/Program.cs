using Microsoft.Extensions.Configuration;
using System;
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

            
            WebShop webShop = new WebShop();
            await webShop.StartWebShop();
        }
    }
}