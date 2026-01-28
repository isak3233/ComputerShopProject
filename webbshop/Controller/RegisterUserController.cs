using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;
using webbshop.UI;
using static System.Net.Mime.MediaTypeNames;
using static webbshop.UI.RegisterPage;

namespace webbshop.Controller
{
    internal class RegisterUserController : IController
    {
        public async Task<IController> ActivateController()
        {
            User newUser = new User();
            RegisterPage page = new RegisterPage(newUser);
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
                        case Buttons.HomePage:
                            return new HomePageController();
                        case Buttons.Firstname:
                            newUser.FirstName = InputHelper.GetStringFromUser("Förnamn: ");
                            page.Update(newUser);
                            break;
                        case Buttons.Lastname:
                            newUser.LastName = InputHelper.GetStringFromUser("Efternamn: ");
                            page.Update(newUser);
                            break;
                        case Buttons.Gender:
                            string? gender = InputHelper.GetGenderFromUser();
                            if(gender != null)
                            {
                                newUser.Gender = gender;
                            }
                            page.Update(newUser);
                            break;
                        case Buttons.Email:
                            newUser.Email = InputHelper.GetStringFromUser("Email: ").ToLower();
                            page.Update(newUser);
                            break;
                        case Buttons.Password:
                            newUser.Password = InputHelper.GetStringFromUser("Password: ");
                            page.Update(newUser);
                            break;
                        case Buttons.StreetName:
                            newUser.StreetName = InputHelper.GetStringFromUser("Gatuadress: ");
                            page.Update(newUser);
                            break;
                        case Buttons.City:
                            
                            Country selectedCountry = await GetCountry();
                            City selectedCity = await GetCity(selectedCountry);
                            
                            newUser.City = selectedCity;
                            newUser.CityId = selectedCity.Id;
                            newUser.City.Country = selectedCountry;
                            page.Update(newUser);
                            break;
                        case Buttons.PhoneNumber:
                            newUser.PhoneNumber = InputHelper.GetStringFromUser("Telefonnummer: ");
                            page.Update(newUser);
                            break;
                        case Buttons.BirthDate:
                            DateTime date = InputHelper.GetDateTimeFromUser();
                            newUser.BirthDate = date;
                            

                            page.Update(newUser);
                            break;
                        case Buttons.Register:
                            
                            if (HasNull(newUser) == false)
                            {
                                try
                                {
                                    await RegisterUser(newUser);
                                    await LoginController.RegisterLogin(newUser, newUser.Email);
                                    Cookie.User = newUser;
                                    WebShop.Cts = new CancellationTokenSource();
                                    LoginController.StartUpdateLoginSessionLoop(WebShop.Cts.Token);
                                    Console.WriteLine("Användaren registerad");
                                    Console.ReadLine();
                                    return new HomePageController();
                                } catch(Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }

                            } else
                            {
                                Console.WriteLine("Alla värden är inte inskrivna");
                            }
                                break;
                        default:
                            page.Render();
                            break;
                    }
                }

            }
        }
        private bool HasNull(User user)
        {
            bool result = false;
            if(
                user.FirstName == null || 
                user.LastName == null ||
                user.Email == null ||
                user.StreetName == null ||
                user.CityId == null ||
                user.PhoneNumber == null ||
                user.Password == null ||
                user.BirthDate.Year == 1
                )
            {
                result = true;
            }
            return result;
        }
        private async Task RegisterUser(User user)
        {
            using(var db = new ShopDbContext())
            {
                if (await db.Users.AnyAsync(u => u.Email == user.Email))
                {
                    throw new InvalidOperationException("Email används redan");
                }
                    
                user.City = null;
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
        }
        static public async Task<City> GetCity(Country country)
        {
            City[] cities = await GetCities(country);
            while (true)
            {
                
                foreach (var city in cities)
                {
                    Console.WriteLine(city.Id + ": " + city.Name);
                }
                int? cityId = InputHelper.GetIntFromUser("Välj stad: ");
                City? selectedCity = cities.Where(c => c.Id == cityId).FirstOrDefault();
                if (selectedCity == null)
                {
                    Console.WriteLine("Det nummret finns inte i listan");
                    Console.ReadLine();
                } else
                {
                    return selectedCity;
                }
            }
        }
        static public async Task<Country> GetCountry()
        {
            Country[] countries = await GetCountries();
            while (true)
            {
                
                foreach (var country in countries)
                {
                    Console.WriteLine(country.Id + ": " + country.Name);
                }
                int? countryId = InputHelper.GetIntFromUser("Välj land: ");
                Country? selectedCountry = countries.Where(c => c.Id == countryId).FirstOrDefault();
                if (selectedCountry == null)
                {
                    Console.WriteLine("Det nummret finns inte i listan");
                    Console.ReadLine();
                }
                else
                {
                    return selectedCountry;
                }
            }
        }
        public static async Task<Country[]> GetCountries()
        {
            using(var db = new ShopDbContext())
            {

                return await db.Countries.ToArrayAsync();

            }
        }
        static public async Task<City[]> GetCities(Country country)
        {
            using(var db = new ShopDbContext())
            {
                return await db.Cities.Where(ci => ci.CountryId == country.Id).ToArrayAsync();
            }
        }

    }
}
