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
using MyMathLibrary;

namespace DownLoadAssemblyOnDemand
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private bool IsReady = false;

        private void btnEquals_Click(object sender, RoutedEventArgs e)
        {
            if (IsReady)
            {
                Calculate();
            }
            else
            {
                WebClient downloader = new WebClient();
                downloader.OpenReadCompleted += new OpenReadCompletedEventHandler(downloader_OpenReadCompleted);
                downloader.OpenReadAsync(new Uri("MyMathLibrary.dll", UriKind.Relative));
                btnEquals.IsEnabled = false;
            }
        }

        private void Calculate()
        {
            double Num1 = Convert.ToDouble(txtNum1.Text);
            double Num2 = Convert.ToDouble(txtNum2.Text);
            lblResult.Text = MyMath.Add(Num1, Num2).ToString();
        }

        void downloader_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if ((e.Error == null) && (e.Cancelled == false))
            {
                AssemblyPart part = new AssemblyPart();
                part.Load(e.Result);
                IsReady = true;
                btnEquals.IsEnabled = true;
                Calculate();
            }
        }
    }
}
