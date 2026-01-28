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
            AddSupplier,
            RemoveSupplier,
            ChangeSupplier,
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

            var addSupplier = new Window("(5)", 30, 60, new List<string> { "Lägg till levrantör" });
            Windows.Add(addSupplier);

            var removeSupplier = new Window("(6)", 30, 70, new List<string> { "Ta bort levrantör" });
            Windows.Add(removeSupplier);

            var changeSupplier = new Window("(7)", 30, 80, new List<string> { "Ändra en levrantör" });
            Windows.Add(changeSupplier);


            var addCategoryW = new Window("(8)", 60, 30, new List<string> { "Lägg till en kategori" });
            Windows.Add(addCategoryW);

            var removeCategoryW = new Window("(9)", 60, 40, new List<string> { "Ta bort en kategori" });
            Windows.Add(removeCategoryW);

            var changeCategorytW = new Window("(10)", 60, 50, new List<string> { "Ändra en kategori" });
            Windows.Add(changeCategorytW);



            var changeInfoAboutUserW = new Window("(11)", 100, 30, new List<string> { "Ändra personliga uppgifter om kund" });
            Windows.Add(changeInfoAboutUserW);

            var changePaymentHistoryW = new Window("(12)", 100, 40, new List<string> { "Ändra beställningshistorik" });
            Windows.Add(changePaymentHistoryW);

            var sendOrderW = new Window("(13)", 100, 50, new List<string> { "Skicka beställningar" });
            Windows.Add(sendOrderW);

            

            var statiticsW = new Window("(14)", 50, 100, new List<string> { "Se statistik" });
            Windows.Add(statiticsW);

            this.Render();
        }
       
    }
}
