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
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices.Automation;

namespace IntegratePowerpoint
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            Files = new ObservableCollection<PicFileInfo>();
            lstPictures.ItemsSource = Files;
            if (App.Current.IsRunningOutOfBrowser == false)
            {
                string info = "由于此应用程序需要操作本地文件和PowerPoint，请在浏览器中右击，从弹出菜单中选择命令将其安装到本地计算机。";
                MessageBox.Show(info);
                btnChoosePicFile.IsEnabled = false;
                btnInsertToPPT.IsEnabled = false;
            }
        }

        private ObservableCollection<PicFileInfo> Files = null;

        private void btnChoosePicFile_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "所有图像文件|*.jpg;*.png;*.gif|*.jpg|*.jpg|*.png|*.png|*.gif|*.gif|*.*|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    PicList.Visibility = Visibility.Visible;
                    foreach (FileInfo file in dialog.Files)
                    {

                        PicFileInfo obj = new PicFileInfo();
                        obj.FilePath = file.FullName;
                        BitmapImage bmp = new BitmapImage();
                        bmp.SetSource(file.OpenRead());
                        obj.PictureImage = bmp;
                        Files.Add(obj);

                    }
                }
                catch (System.Security.SecurityException)
                {
                    String info = "本应用程序需要提升的信任，并且只能处理位于用户文件夹（比如“我的文档”和“我的图片”）下的文件。";
                    MessageBox.Show(info);

                }
                catch (Exception ex)
                {
                    String info = string.Format("{0}:{1}", ex.GetType(), ex.Message);
                    MessageBox.Show(info);
                }
            }
        }

        private void btnInsertToPPT_Click(object sender, RoutedEventArgs e)
        {
            string progID = "PowerPoint.Application";
            dynamic powerpoint = null;
            this.btnInsertToPPT.IsEnabled = false;
            try
            {
                //检测一下powerpoint是否已经运行？
                powerpoint = AutomationFactory.GetObject(progID);
            }
            catch  //没有正在运行的powerpoint实例，就启动它
            {
                powerpoint = AutomationFactory.CreateObject(progID);
            }
            //显示powerpoint主界面
            powerpoint.Visible = true;


            dynamic presentation = powerpoint.Presentations.Add();
            powerpoint.ActiveWindow.Activate();

            foreach (PicFileInfo pic in Files)
            {

                dynamic slide =
                  presentation.Slides.Add(
                  presentation.Slides.Count + 1, 1);
               slide.Shapes.AddPicture(pic.FilePath,
                  false,
                  true,
                 0, 0, pic.PictureImage.PixelHeight, pic.PictureImage.PixelHeight);

            }
            this.btnInsertToPPT.IsEnabled = true;

        }
    }
}
