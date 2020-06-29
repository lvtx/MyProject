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
using System.IO;

namespace DownloadOnDemand
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BeginDownLoad();
        }

        private void BeginDownLoad()
        {
            string uri = Application.Current.Host.Source.AbsoluteUri;
            int index = uri.IndexOf("/ClientBin");
            uri = uri.Substring(0, index) + "/ClientBin/MyResourceLibrary.dll";
            WebClient client = new WebClient();
            client.OpenReadCompleted += client_OpenReadCompleted;
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            client.OpenReadAsync(new Uri(uri));
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            tbInfo.Text = string.Format("正在下载，已接收{0}字节", e.TotalBytesToReceive);
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.tbInfo.Text = e.Error.Message;
            }
            else
            {
                try
                {
                   
  AssemblyPart assemblypart = new AssemblyPart();
                assemblypart.Load(e.Result);
                MediaPlayer.Source = new Uri("/MyResourceLibrary;component/Wildlife.wmv", UriKind.Relative);
                MediaPlayer.Play();
                tbInfo.Text += ",下载完成!";
                }
                catch (Exception ex )
                {

                    tbInfo.Text = ex.Message;
                }
              
            }
        }
    }
}
