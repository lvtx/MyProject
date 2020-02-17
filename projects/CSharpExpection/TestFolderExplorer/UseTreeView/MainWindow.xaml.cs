using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UseTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ObservableCollection<Country> countries = GetCountries();
            tvwCountries.ItemsSource = countries;
        }
        ObservableCollection<Country> GetCountries()
        {
            ObservableCollection<Town> ShanDongTowns = new ObservableCollection<Town>()
            {
                new Town() { TownName = "烟台" },
                new Town() { TownName = "威海" },
                new Town() { TownName = "济宁" },
                new Town() { TownName = "日照" }
            };
            ObservableCollection<Town> HuBeiTowns = new ObservableCollection<Town>()
            {
                new Town() { TownName = "武汉" },
                new Town() { TownName = "襄阳" },
                new Town() { TownName = "鄂州" },
                new Town() { TownName = "荆门" }
            };
            ObservableCollection<City> Chinacities = new ObservableCollection<City>()
            {
                new City() { CityName = "山东", Towns = ShanDongTowns },
                new City() { CityName = "湖北", Towns = HuBeiTowns }
            };
            ObservableCollection<Country> countries = new ObservableCollection<Country>()
            {
                new Country() { CountryName = "中国", Cities = Chinacities },
                new Country() { CountryName = "美国"}
            };
            return countries;
        }
    }
}
