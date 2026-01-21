using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.UI
{
    internal class ThankYouPage : Page
    {
        public ThankYouPage()
        {
            var ThankYouW = new Window("", 50, 50, new List<string> {"Tack för din beställning :)"});
            Windows.Add(ThankYouW);
        }
    }
}
