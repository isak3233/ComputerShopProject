using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.UI;
using static webbshop.UI.CategoryPage;

namespace webbshop.Controller
{
    internal class CategoryController : IController
    {
        public IController ActivateController()
        {
            CategoryPage page = new CategoryPage();
            page.Render();
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
                        case Buttons.homePage:
                            return new HomePageController();
                        default:
                            break;

                    }
                }

            }
        }
    }
}
