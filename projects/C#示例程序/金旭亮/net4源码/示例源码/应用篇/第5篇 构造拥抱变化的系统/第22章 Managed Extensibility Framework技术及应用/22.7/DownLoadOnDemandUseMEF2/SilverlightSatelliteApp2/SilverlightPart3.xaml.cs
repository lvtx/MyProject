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

namespace SilverlightSatelliteApp2
{
    [Export(typeof(IUIPart))]
    public partial class SilverlightPart3 : UserControl
    {
        public SilverlightPart3()
        {
            InitializeComponent();
        }
    }
}
