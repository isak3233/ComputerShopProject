using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;

namespace webbshop
{
    public class InputHelper
    {
        public static int GetIntFromUser(string inputString = "Skriv ett nummer: ", bool returnIfEmpty = false)
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
                    return 0;
                } else
                {
                    Console.WriteLine(inputString);
                }
            }
            
        }
        public static bool GetBoolFromUser(string inputString = "Skriv ja (1) eller nej (2):", string errorString = "input är inte 1 eller 2")
        {
            while (true)
            {
                Console.Write(inputString);
                string? input = Console.ReadLine();
                if(input == "1")
                {
                    return true;
                } 
                else if(input == "2")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine(errorString);
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
        static public decimal GetDecimalFromUser(string inputString = "Skriv ett decimal nummer:", string errorString = "input är tomt")
        {
            while(true)
            {
                Console.Write(inputString);
                string input = Console.ReadLine();
                input = input.Replace('.', ',');


                if (decimal.TryParse(input, out decimal value))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine("Ogiltigt");
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
        static public string? GetGenderFromUser(string inputString = "Kvinna(1), Man(2), Ickebinär(3): ", string errorString = "input är tomt")
        {
            int input = GetIntFromUser("Kvinna(1), Man(2), Ickebinär(3): ");
            string? result = null;
            switch (input)
            {
                case 1:
                    result = "Kvinna";
                    break;
                case 2:
                    result = "Man";
                    break;
                case 3:
                    result = "Ickebinär";
                    break;
                default:
                    break;
            }
            return result;
        }
        static public DateTime GetDateTimeFromUser()
        {
            while (true)
            {

                string year = GetStringFromUser("År: ");
                string month = GetStringFromUser("Månad: ");
                string day = GetStringFromUser("Dag: ");
                try
                {
                    DateTime date = DateTime.ParseExact(
                    year + "-" + month + "-" + day,
                    "yyyy-M-d",
                    CultureInfo.InvariantCulture
                    );
                    return date;
                }
                catch
                {
                    Console.WriteLine("Lyckades inte parse datumet");
                }
            }
        }

    }
}
