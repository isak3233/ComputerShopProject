using System;
using webbshop.UI;

namespace webbshop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebShop webShop = new WebShop();
            await webShop.StartWebShop();
        }
    }
}