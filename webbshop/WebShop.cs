using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Controller;

namespace webbshop
{
    internal class WebShop
    {
        public async Task StartWebShop()
        {
            IController currentController = new HomePageController();
            while (true)
            {
                currentController = await currentController.ActivateController();
            }
        }
    }
}
