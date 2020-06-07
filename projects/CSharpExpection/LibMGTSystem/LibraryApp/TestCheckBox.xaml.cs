using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryApp
{
    /// <summary>
    /// Interaction logic for TestCheckBox.xaml
    /// </summary>
    public partial class TestCheckBox : Window
    {
        public TestCheckBox()
        {
            InitializeComponent();
            List<Info> infos = new List<Info>()
            {
                new Info(){Code = 'M',Name = "Mr. Brith",Description = "Is Good"},
                new Info(){Code = 'B',Name = "Bath March",Description = "Just So So"}
            };
            dgrdShowInfos.ItemsSource = infos;
        }
    }
    public class Info
    {
        public char Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
