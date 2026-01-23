using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.UI
{
    internal class AdminPage : Page
    {
        public enum Buttons
        {
            HomePage,
            AddProduct,
            RemoveProduct,
            ChangeProduct,
            AddCategory,
            RemoveCategory,
            ChangeCategory,
            ChangeUserInfo,
            ChangePaymentHistory,
            SendOrder,
            Stats
        }
        public AdminPage()
        {
            Update();
        }
        public void Update()
        {
            Windows = new List<Window>();
            var backToHomePageW = new Window("(1)", 0, 0, new List<string> { "<- Tillbaka till startsidan" });
            Windows.Add(backToHomePageW);



            var addProductW = new Window("(2)",30, 30, new List<string> { "Lägg till en produkt" });
            Windows.Add(addProductW);

            var removeProductW = new Window("(3)", 30, 40, new List<string> { "Ta bort en produkt" });
            Windows.Add(removeProductW);

            var changeProductW = new Window("(4)", 30, 50, new List<string> { "Ändra en produkt" });
            Windows.Add(changeProductW);



            var addCategoryW = new Window("(5)", 60, 30, new List<string> { "Lägg till en kategori" });
            Windows.Add(addCategoryW);

            var removeCategoryW = new Window("(6)", 60, 40, new List<string> { "Ta bort en kategori" });
            Windows.Add(removeCategoryW);

            var changeCategorytW = new Window("(7)", 60, 50, new List<string> { "Ändra en kategori" });
            Windows.Add(changeCategorytW);



            var changeInfoAboutUserW = new Window("(8)", 100, 30, new List<string> { "Ändra personliga uppgifter om kund" });
            Windows.Add(changeInfoAboutUserW);

            var changePaymentHistoryW = new Window("(9)", 100, 40, new List<string> { "Ändra beställningshistorik" });
            Windows.Add(changePaymentHistoryW);

            var sendOrderW = new Window("(10)", 100, 50, new List<string> { "Skicka beställningar" });
            Windows.Add(sendOrderW);

            var statiticsW = new Window("(11)", 50, 80, new List<string> { "Se statistik" });
            Windows.Add(statiticsW);

            this.Render();
        }
       
    }
}
