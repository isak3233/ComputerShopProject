using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.UI;
using static webbshop.UI.HomePage;

namespace webbshop.Controller
{
    internal class HomePageController : IController
    {
        public IController ActivateController()
        {
            HomePage page = new HomePage();
            page.Render();
            while (true)
            {
                int? option = InputHelper.GetIntFromUser("", true);
                if(option == null)
                {
                    page.Render();
                } else
                {
                    option -= 1;
                    switch ((Buttons)option)
                    {
                        case Buttons.Category:
                            return new CategoryController();
                        default:
                            break;
                    }
                }
                    
            }

        }
    }
}
