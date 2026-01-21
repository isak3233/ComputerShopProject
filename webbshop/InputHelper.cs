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
                } else if(returnIfEmpty && input == "")
                {
                    return null;
                } else
                {
                    Console.WriteLine(inputString);
                }
            }
            
        }
        public static string GetStringFromUser(string inputString = "Skriv något:", string errorString = "input är tomt")
        {
            while(true)
            {
                Console.Write(inputString);
                string? input = Console.ReadLine();
                if(input != "" && input != null)
                {
                    return input;
                } else
                {
                    Console.WriteLine(errorString);
                }
            }
        }
        static public int[] GetCartProductOptionFromUser()
        {
            while (true)
            {

                Console.WriteLine("Ta bort från kundvagn (1)");
                Console.WriteLine("Ändra antal produkter (2)");
                int? option = InputHelper.GetIntFromUser("Skriv alternativ: ", false);
                switch (option)
                {
                    case 1:
                        return new int[2] { 1, 0 };
                    case 2:
                        Console.WriteLine("Hur mycket av denna produkt vill du ha?");
                        int? amount = InputHelper.GetIntFromUser("Antal: ", false);
                        if (amount != null)
                        {
                            return new int[2] { 2, amount.Value };
                        }
                        return new int[2] { 2, 0 };
                    default:
                        Console.WriteLine("Alternativet finns inte");
                        break;
                }

            }

        }

    }
}
