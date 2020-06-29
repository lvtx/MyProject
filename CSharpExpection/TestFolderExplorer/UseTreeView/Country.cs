using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace UseTreeView
{
    class Country
    {
        public string CountryName { get; set; }
        public ObservableCollection<City> Cities { get; set; }
        //public override string ToString()
        //{
        //    return CountryName;
        //}
    }
    class City
    {
        public string CityName { get; set; }
        public ObservableCollection<Town> Towns { get; set; }
        //public override string ToString()
        //{
        //    return CityName;
        //}
    }
    class Town
    {
        public string TownName { get; set; }
        //public override string ToString()
        //{
        //    return TownName;
        //}
    }
    
}
