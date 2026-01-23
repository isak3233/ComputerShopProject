using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.UI
{
    public class StatsPage : Page
    {
        public enum Buttons
        {
            AdminPanel
        }
        public StatsPage()
        {
            Update();
        }
        public void Update()
        {
            Windows = new List<Window>();
            var backToAdminPanel = new Window("(1)", 0, 0, new List<string> { "<- Tillbaka till admin panelen"});
            Windows.Add(backToAdminPanel);
            this.Render();
        }
    }
}
