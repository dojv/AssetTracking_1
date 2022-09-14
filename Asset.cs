using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking_davidnilsson
{
    internal class Asset
    {
        public string ModelName { get; set; }
        public string Brand { get; set; }
        public string AssetClass { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Office Office { get; set; }
        public string Currency { get; set; }
        public double LocalPrice { get; set; }
        public double DollarPrice { get; set; }

        //constructors
        public Asset () { }
        public Asset 
            (
            string modelname, 
            string brand,
            string assetclass, 
            DateTime purchasedate, 
            Office office, 
            string currency, 
            double dollarprice, 
            double localprice
            )
        {
            ModelName = modelname;
            Brand = brand;
            AssetClass = assetclass;
            PurchaseDate = purchasedate;
            Office = office;
            Currency = currency;
            DollarPrice = dollarprice;
            LocalPrice = localprice;
        }

        public Asset
            (
            string modelname,
            string brand,
            string assetclass,
            DateTime purchasedate,
            Office office,
            string currency
            )
        {
            ModelName = modelname;
            Brand = brand;
            AssetClass = assetclass;
            PurchaseDate = purchasedate;
            Office = office;
            Currency = currency;
        }

        public virtual double CalcLocalPrice(User user, double dollarPrice, string currency)
        {
            double localPrice = 0;
            //checks for when currencies match, then gets the rates and calculates the local price
            for (int i = 0; i < user.Currencies.Length; i++)
            {
                if (user.Currencies[i] == currency)
                {
                    localPrice = user.CurrencyRatesFromDollar[i] * dollarPrice;
                    break;
                }
            }
            return localPrice;
        }
        public virtual double CalcDollarPrice(User user, double localPrice, string currency)
        {
            double dollarPrice = 0;
            //checks for when currencies match, then gets the rates and calculates the local price
            for (int i = 0; i < user.Currencies.Length; i++)
            {
                if (user.Currencies[i] == currency)
                {
                    dollarPrice = localPrice / user.CurrencyRatesFromDollar[i];
                    break;
                }
            }
            return dollarPrice;
        }
    }
}
