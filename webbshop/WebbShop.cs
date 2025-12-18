using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Controller;

namespace webbshop
{
    internal class WebbShop
    {
        public void StartWebbShop()
        {
            IController currentController = new HomePageController();
            while (true)
            {
                currentController = currentController.ActivateController();
            }
        }
    }
}
