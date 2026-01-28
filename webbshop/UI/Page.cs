using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;

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
        public void AddDynamicWindows(int indexOn, List<Window> windowsToShow, Window showMoreW, Window showLessW, int cols, int rows, int startX, int startY, int xPerWindow, int yPerWindow, int? choiceStart = null)
        {
            int row = 0;
            int col = 0;
            int indexString = 0;
            int totalWindowsOnPage = cols * rows;
            if (indexOn + totalWindowsOnPage < windowsToShow.Count())
            {
                Windows.Add(showMoreW);
            }
            if (indexOn > 0)
            {
                Windows.Add(showLessW);
            }
            for (int i = indexOn; i < indexOn + totalWindowsOnPage; i++)
            {

                if (i >= windowsToShow.Count())
                {
                    break;
                }
                var windowToAdd = windowsToShow[i];
                windowToAdd.Header += choiceStart != null ? $"({choiceStart + indexString})" : "";
                windowToAdd.WidthPercentage = startX + xPerWindow * col;
                windowToAdd.HeightPercentage = startY + yPerWindow * row;
                Windows.Add(windowToAdd);
                col++;
                if (col >= cols)
                {
                    col = 0;
                    row++;
                }
                indexString++;
            }
        }
    }
}
