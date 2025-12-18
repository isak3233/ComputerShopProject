using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop
{
    public class InputHelper
    {
        public static int? GetIntFromUser(string inputString = "Skriv ett nummer: ", bool returnIfEmpty = false)
        {
            while(true)
            {
                Console.Write(inputString);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int number))
                {
                    return number;
                } else if(input == "")
                {
                    return null;
                } else
                {
                    Console.WriteLine("Skriv in ett nummer");
                }
            }
            
        }
    }
}
