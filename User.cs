using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking_davidnilsson
{
    internal class User
    {
        public string Name { get; set; }

        //the user stores all data. every office has a list of assets assigned to that office
        public List<Office> Offices = new List<Office>();

        //this exists to be able to add a new type of asset, or currency
        public string[] AssetClasses = new string[3] { "computer", "phone", "tablet" };
        public string[] Currencies = new string[6] { "SEK", "DKK", "EUR", "USD", "RUB", "RMB" };
        public double[] CurrencyRatesFromDollar = new double[6] { 10.5, 7.5, 1, 1, 60, 7 };
        //( ^ how many of x currency do you need to get 1 dollar )

        //this is to keep track of what short-term choices the user makes
        public Office ChosenOffice { get; set; }
        public string ChosenAssetClass { get; set; }
        public string ChosenCurrency { get; set; }

        //list of random brands
        public string[] Brands = new string[] { "ASUS", "Motorola", "Nokia", "Lenovo", "Samsung", "LG", "Logitech", "Sony", "Apple", "Huawei", "Toshiba", "Panasonic", "Microsoft"};

        //fills app with data at the start
        public User AutoFill(User user)
        {
            Random roll = new Random();

            //creates office-objects and adds them to the user
            Office malmo = new Office("Malmo", "SEK");
            Office copenhagen = new Office("Copenhagen", "DKK");
            Office berlin = new Office("Berlin", "EUR");
            user.Offices.Add(malmo);
            user.Offices.Add(copenhagen);
            user.Offices.Add(berlin);

            for (int i = 0; i < user.Offices.Count(); i++)
            {
                //each block works the same: creates random Asset-objects and adds them to each office
                Computer newComp = new Computer();
                newComp.ModelName = user.RandomModelName(user);
                newComp.Brand = user.Brands[roll.Next(0, user.Brands.Length)];
                newComp.AssetClass = "computer";
                newComp.PurchaseDate = user.RandomDate();
                newComp.Office = user.Offices[i];
                newComp.Currency = newComp.Office.Currency;
                newComp.DollarPrice = roll.Next(2000, 3000);
                newComp.LocalPrice = newComp.CalcLocalPrice(user, newComp.DollarPrice, newComp.Currency);

                Phone newPhone = new Phone();
                newPhone.ModelName = user.RandomModelName(user);
                newPhone.Brand = user.Brands[roll.Next(0, user.Brands.Length)];
                newPhone.AssetClass = "phone";
                newPhone.PurchaseDate = user.RandomDate();
                newPhone.Office = user.Offices[i];
                newPhone.Currency = newPhone.Office.Currency;
                newPhone.DollarPrice = roll.Next(250, 1000);
                newPhone.LocalPrice = newPhone.CalcLocalPrice(user, newPhone.DollarPrice, newPhone.Currency);

                Tablet newTablet = new Tablet();
                newTablet.ModelName = user.RandomModelName(user);
                newTablet.Brand = user.Brands[roll.Next(0, user.Brands.Length)];
                newTablet.AssetClass = "tablet";
                newTablet.PurchaseDate = user.RandomDate();
                newTablet.Office = user.Offices[i];
                newTablet.Currency = newTablet.Office.Currency;
                newTablet.DollarPrice = roll.Next(500, 1500);
                newTablet.LocalPrice = newTablet.CalcLocalPrice(user, newTablet.DollarPrice, newTablet.Currency);

                user.Offices[i].Assets.Add(newComp);
                user.Offices[i].Assets.Add(newPhone);
                user.Offices[i].Assets.Add(newTablet);
            }

            Console.WriteLine("Successfully added data to the program.");
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            return user;
        }
        public DateTime RandomDate()
        {
            Random roll = new Random();
            int daysInThreeYears = 365 * 3;
            int daysInTwoYears = 365 * 2;

            //purchasedate = sometime between now and three years ago
            DateTime purchasedate = DateTime.Now.AddDays(roll.Next(daysInThreeYears * -1, daysInTwoYears * -1));
            return purchasedate;
        }
        public string RandomModelName(User user)
        {
            Random roll = new Random();

            int char1 = roll.Next(65, 91); //65-90 ASCII is capitol letters
            int char2 = roll.Next(65, 91); 
            int char3 = roll.Next(65, 91);
            int num = roll.Next(0, 1000);

            //converting ints into chars and building string, desired format: XXX-000
            string productName = 
                String.Concat
                    (
                    Convert.ToChar(char1).ToString() + 
                    Convert.ToChar(char2).ToString() + 
                    Convert.ToChar(char3).ToString() +
                    "-" + 
                    num.ToString()
                    );
            return productName;
        }
    }
}
