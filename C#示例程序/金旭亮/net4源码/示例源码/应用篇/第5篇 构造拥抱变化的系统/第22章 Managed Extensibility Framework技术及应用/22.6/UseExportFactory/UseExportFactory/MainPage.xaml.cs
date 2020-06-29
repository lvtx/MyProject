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

namespace UseExportFactory
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            //组合部件
            CompositionInitializer.SatisfyImports(this);
        }

        [Import(typeof(UserControl))]
        public ExportFactory<UserControl> PartCreator;

        private void btnCreatePart_Click(object sender, RoutedEventArgs e)
        {
            ExportLifetimeContext<UserControl> newPart = PartCreator.CreateExport();
            PartsContainer.Items.Add(newPart.Value);
          
        }
    }
}
