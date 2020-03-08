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
    [Export(typeof(UserControl))]
    public partial class SilverlightControl1 : UserControl
    {
        public SilverlightControl1()
        {
            InitializeComponent();
        }
    }
}
