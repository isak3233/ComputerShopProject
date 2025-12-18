using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace webbshop
{
    public class Window
    {
        public string Header { get; set; }
        public List<string> TextRows { get; set; }
        public int WidthPercentage { get; set; }
        public int HeightPercentage { get; set; }

        public Window(string header, int widthPercentage, int heightPercentage, List<string> textRows)
        {
            Header = header;
            TextRows = textRows;
            WidthPercentage = widthPercentage;
            HeightPercentage = heightPercentage;

        }

        public void Draw()
        {
            int left = GetWidthPos();
            int top = GetHeight();
            var width = TextRows.OrderByDescending(s => s.Length).FirstOrDefault().Length;

            // Kolla om Header är längre än det längsta ordet i listan
            if (width < Header.Length + 4)
            {
                width = Header.Length + 4;
            }
            ;

            // Rita Header
            Console.SetCursorPosition(left, top);
            if (Header != "")
            {
                Console.Write('┌' + " ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(Header);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + new String('─', width - Header.Length) + '┐');
            }
            else
            {
                Console.Write('┌' + new String('─', width + 2) + '┐');
            }

            // Rita raderna i sträng-Listan
            for (int j = 0; j < TextRows.Count; j++)
            {
                Console.SetCursorPosition(left, top + j + 1);
                Console.WriteLine('│' + " " + TextRows[j] + new String(' ', width - TextRows[j].Length + 1) + '│');
            }

            // Rita undre delen av fönstret
            Console.SetCursorPosition(left, top + TextRows.Count + 1);
            Console.Write('└' + new String('─', width + 2) + '┘');


            // Kolla vilket som är den nedersta posotion, i alla fönster, som ritats ut
            if (Lowest.LowestPosition < top + TextRows.Count + 2)
            {
                Lowest.LowestPosition = top + TextRows.Count + 2;
            }

            Console.SetCursorPosition(0, Lowest.LowestPosition);
        }
        private int GetWidthPos()
        {

            var textWidth = TextRows.OrderByDescending(s => s.Length).FirstOrDefault().Length;
            if (textWidth < Header.Length + 4)
            {
                textWidth = Header.Length + 4;
            }
            ;
            int boxLength = textWidth + 4;
            int startPoint = (int)(Console.WindowWidth * (WidthPercentage / 100.0));
            if (startPoint - boxLength <= 0)
            {
                return 0;
            }
            else
            {
                return (startPoint - boxLength);
            }
        }
        private int GetHeight()
        {
            int boxLength = TextRows.Count() + 3;
            int startPoint = (int)(Console.WindowHeight * (HeightPercentage / 100.0));
            if(startPoint - boxLength <= 0)
            {
                return 0;
            }
            else
            {
                return (startPoint - boxLength);
            }
        }
    }

    public static class Lowest
    {
        public static int LowestPosition { get; set; }
    }
}
