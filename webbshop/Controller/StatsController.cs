using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static webbshop.UI.StatsPage;

namespace webbshop.Controller
{
    internal class StatsController : IController
    {
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
                        default:
                            page.Render();
                            break;
                    }
                }

            }
        }
    }
}
