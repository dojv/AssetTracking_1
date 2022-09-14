using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking_davidnilsson
{
    internal class Office
    {
        public string Name { get; set; }
        public string Currency { get; set; } 
        public List<Asset> Assets = new List<Asset>();

        //constructors
        public Office() { }
        public Office(string name, string currency) 
        {
            Name = name;
            Currency = currency;
        }
    }
}
