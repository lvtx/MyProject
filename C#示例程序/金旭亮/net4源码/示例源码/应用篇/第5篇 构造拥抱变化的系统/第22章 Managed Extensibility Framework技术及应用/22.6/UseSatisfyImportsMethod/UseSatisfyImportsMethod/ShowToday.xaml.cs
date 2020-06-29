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

namespace UseSatisfyImportsMethod
{
    [Export(typeof(UserControl))]
    public partial class ShowToday : UserControl
    {
        public ShowToday()
        {
            InitializeComponent();
            tbInfo.Text = "当前时间："+DateTime.Now.ToShortDateString();
        }
    }
}
