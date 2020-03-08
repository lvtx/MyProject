using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using WinForm = System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyImageProcessor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class winMain : Window
    {

        BitmapSource bmpSource = null;
        int stride = 0;
        byte[] ImagePixelData = null;  //用于保存图像的像素数据，将用于图像处理
        byte[] ImagePixelDataBackup = null; //图像的像素数据备份，用于还原
        WinForm.OpenFileDialog openFileDialog1 = null;

        public winMain()
        {
            InitializeComponent();
            openFileDialog1 = new WinForm.OpenFileDialog();
            openFileDialog1.Filter = "图像文件|*.jpg;*.gif;*.png;*.jpeg|所有文件|*.*";

        }


        /// <summary>
        /// 装入图像的像素数据到字节数组中。
        /// </summary>
        /// <param name="ImagePath"></param>
        private void LoadImage(string ImagePath)
        {
            bmpSource = new BitmapImage(new Uri(ImagePath));
            stride = bmpSource.PixelWidth * bmpSource.Format.BitsPerPixel / 8;
            stride += 4 - stride % 4;
            int ImagePixelDataSize = stride * bmpSource.PixelHeight * bmpSource.Format.BitsPerPixel / 8; ;
            ImagePixelData = new byte[ImagePixelDataSize];

            bmpSource.CopyPixels(ImagePixelData, stride, 0);
            //备份数据
            ImagePixelDataBackup = new byte[ImagePixelDataSize];
            bmpSource.CopyPixels(ImagePixelDataBackup, stride, 0);
        }

       

        /// <summary>
        /// 处理图像
        /// </summary>
        private void ProcessImageData()
        {
            if (ImagePixelData == null)
                return;
            long UsedTime = 0;
           
           
            

            if (chkSingleThread.IsChecked == true)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start(); //启动计时
                
               
                for(int i=0;i<ImagePixelData.Length;i++)
                {
                    byte value = ImagePixelData[i];
                    ImagePixelData[i] = (byte)(Math.Sin(~value) * 255);
                }

                sw.Stop();//停止计时
                //获取算法执行时间
                UsedTime = sw.ElapsedMilliseconds;
            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Start(); //启动计时
                Parallel.For(0, ImagePixelData.Length, (i) =>
                    {
                        byte value = ImagePixelData[i];
                        ImagePixelData[i] = (byte)(Math.Sin(~value)*255);
                    });
                sw.Stop();//停止计时
                //获取算法执行时间
                UsedTime = sw.ElapsedMilliseconds;
            }
            ShowImageFromPixelData(ImagePixelData,image1);
            lblTime.Text = UsedTime.ToString();
        }

        /// <summary>
        /// 将字节数组中保存的图像像素数据显示在Image控件中
        /// </summary>
        /// <param name="imagePixelData">图像像素数据数组</param>
        /// <param name="imgControl">用于显示图像Image控件</param>
        private void ShowImageFromPixelData(byte[] imagePixelData, Image imgControl)
        {
            BitmapSource newImageSource = BitmapSource.Create(bmpSource.PixelWidth, bmpSource.PixelHeight,
                bmpSource.DpiX, bmpSource.DpiY, bmpSource.Format, bmpSource.Palette,
                imagePixelData, stride);

            imgControl.Source = newImageSource;
        }

        private void btnLoadPicture_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == WinForm.DialogResult.OK)
            {
                LoadImage(openFileDialog1.FileName);
                image1.Source = bmpSource;
            }
        }

       

        private void btnRestoreImage_Click(object sender, RoutedEventArgs e)
        {
            Array.Copy(ImagePixelDataBackup, ImagePixelData, ImagePixelDataBackup.Length);
            ShowImageFromPixelData(ImagePixelData, image1);
        }

        private void btnProcessImage_Click(object sender, RoutedEventArgs e)
        {
            ProcessImageData();
        }
    }
}
