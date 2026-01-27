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
            AdminPanel,
            MostSoldInCountry,
            MostProfitableProducts,
            SalesPerCategory,
            GenderBuyProduct,
            BestCustomers,
            CountriesWithBestCustomers,
            AverageAgeProducts,
            AccountSecurity,
            ProductAdded
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

            var mostSoldInCountryW = new Window("(2)", 40, 30, new List<string> { "Mest sålda produkter gruperade i land" });
            Windows.Add(mostSoldInCountryW);

            var mostProfitableProductsW = new Window("(3)", 40, 40, new List<string> { "Mest lönsamma produkter" });
            Windows.Add(mostProfitableProductsW);

            var salesPerCategoryW = new Window("(4)", 40, 50, new List<string> { "Försäljning per kategori" });
            Windows.Add(salesPerCategoryW);

            var genderBuyProductW = new Window("(5)", 40, 60, new List<string> { "Produkternas kön förhållande" });
            Windows.Add(genderBuyProductW);

            var bestCustomersW = new Window("(6)", 40, 70, new List<string> { "Top 10 mest spenderande kunderna" });
            Windows.Add(bestCustomersW);

            var countryWithBestCustomersW = new Window("(7)", 40, 80, new List<string> { "Länder med bäst kunder" });
            Windows.Add(countryWithBestCustomersW);

            var averageAgeOnProductsW = new Window("(8)", 40, 90, new List<string> { "Medelålrder per produkt" });
            Windows.Add(averageAgeOnProductsW);

            var accountSecurityW = new Window("(9)", 60, 30, new List<string> { "Konto säkerhet" });
            Windows.Add(accountSecurityW);

            var productAddedInfoW = new Window("(10)", 60, 40, new List<string> { "Produkt loggning" });
            Windows.Add(productAddedInfoW);
            this.Render();
        }
    }
}
