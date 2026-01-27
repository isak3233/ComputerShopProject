using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;

namespace webbshop.UI
{
    public class RegisterPage : Page
    {
        public enum Buttons
        {
            HomePage,
            Firstname,
            Lastname,
            Gender,
            Email,
            Password,
            StreetName,
            City,
            PhoneNumber,
            BirthDate,
            Register,
            
        }
        public RegisterPage(User user)
        {
            Update(user);
        }
        public void Update(User user)
        {
            Windows = new List<Window>();
            var backToHomePage = new Window("(1)", 0, 0, new List<string> { "<- Gå tillbaka till startsidan" });
            Windows.Add(backToHomePage);

            var firstnameW = new Window("(2)", 30, 25, new List<string> { user.FirstName == null ? "Förnamn" : user.FirstName });
            Windows.Add(firstnameW);

            var lastnameW = new Window("(3)", 30, 35, new List<string> { user.LastName == null ? "efternamn" : user.LastName });
            Windows.Add(lastnameW);

            var genderW = new Window("(4)", 30, 45, new List<string> { user.Gender == null ? "kön" : user.Gender });
            Windows.Add(genderW);

            var emailW = new Window("(5)", 30, 55, new List<string> { user.Email == null ? "email" : user.Email });
            Windows.Add(emailW);

            var passwordW = new Window("(6)", 30, 65, new List<string> { user.Password == null ? "lösenord" : user.Password });
            Windows.Add(passwordW);

            var streetNameW = new Window("(7)", 30, 75, new List<string> { user.StreetName == null ? "adress" : user.StreetName });
            Windows.Add(streetNameW);

            var cityW = new Window("(8)", 30, 85, new List<string> { user.City == null ? "land/stad" : user.City.Country.Name + "/" + user.City.Name });
            Windows.Add(cityW);

            var phoneNumberW = new Window("(9)", 60, 25, new List<string> { user.PhoneNumber == null ? "telefonnummer" : user.PhoneNumber });
            Windows.Add(phoneNumberW);

            var birthDateW = new Window("(10)", 60, 35, new List<string> { user.BirthDate.Year == 1 ? "födelsedatum" : user.BirthDate.Year + "-" + user.BirthDate.Month + "-" + user.BirthDate.Day });
            Windows.Add(birthDateW);

            var registerW = new Window("(11)", 60, 45, new List<string> { "Registrera" });
            Windows.Add(registerW);
            this.Render();
        }
    }
}
