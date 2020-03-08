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
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using PartContracts;

namespace DownLoadOnDemandUseMEF
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            App.CatalogService.AddXap("SilverlightSatelliteApp1.xap");
            CompositionInitializer.SatisfyImports(this);
            this.DataContext = ViewModel;
        }

        void deployCata_DownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message);
        }

        [Import]
        public MainPageViewModel ViewModel { get; set; }

       
    }
}
