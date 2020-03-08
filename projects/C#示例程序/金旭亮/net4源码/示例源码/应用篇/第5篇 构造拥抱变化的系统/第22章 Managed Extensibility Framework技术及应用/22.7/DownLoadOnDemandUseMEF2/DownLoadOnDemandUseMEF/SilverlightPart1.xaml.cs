using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using PartContracts;
using System.ComponentModel.Composition.Hosting;

namespace DownLoadOnDemandUseMEF
{
    [Export(typeof(IUIPart))]
    public partial class SilverlightPart1 : UserControl,IUIPart
    {
        public SilverlightPart1()
        {
            InitializeComponent();
        }

        private void btnDownloadXap_Click(object sender, RoutedEventArgs e)
        {
            App.CatalogService.AddXap("SilverlightSatelliteApp2.xap");   
        }
       
    }
}
