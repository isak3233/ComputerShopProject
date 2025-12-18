using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.UI
{
    public abstract class Page
    {
        protected List<Window> Windows { get; set; } = new List<Window>();

        public void Render()
        {
            Console.Clear();
            Lowest.LowestPosition = 0;
            foreach (var window in Windows)
            {
                
                window.Draw();
            }
        }
    }
}
