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
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            //组合部件
            CompositionInitializer.SatisfyImports(this);
            if (MEFUIPart != null) //在界面上显示部件
                MEFPartControlContainer.Content = MEFUIPart;
        }

        [Import(typeof(UserControl))]
        public UserControl MEFUIPart { get; set; }
    }
}
