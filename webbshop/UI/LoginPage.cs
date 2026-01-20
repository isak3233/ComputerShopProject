using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.UI
{
    public class LoginPage : Page
    {
        public enum Buttons
        {
            HomePage,
            Email,
            Password,
            Login,
            Register

        }
        public LoginPage(string? email = null, string? password = null)
        {

            var backToHomePage = new Window("(1)", 0, 0, new List<string> { "<- Gå tillbaka till startsidan" });
            Windows.Add(backToHomePage);

            



            if (email == null)
            {
                var emailW = new Window("(2)", 50, 30, new List<string> { "Email" });
                Windows.Add(emailW);
            } else
            {
                var emailW = new Window("(2)", 50, 30, new List<string> { email });
                Windows.Add(emailW);
            }

            if (password == null)
            {
                var passwordW = new Window("(3)", 50, 40, new List<string> { "Lösenord" });
                Windows.Add(passwordW);
            }
            else
            {
                var passwordW = new Window("(3)", 50, 40, new List<string> { password });
                Windows.Add(passwordW);
            }


            var login = new Window("(4)", 50, 50, new List<string> { "Logga in" });
            Windows.Add(login);

            var registerW = new Window("(5)", 50, 60, new List<string> { "Registera ny användare" });
            Windows.Add(registerW);
        }
    }
}
