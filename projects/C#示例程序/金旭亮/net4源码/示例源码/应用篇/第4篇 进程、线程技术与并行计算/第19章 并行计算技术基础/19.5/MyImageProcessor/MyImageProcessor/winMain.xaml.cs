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

namespace MyImageProcessor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class winMain : Window
    {
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
            stride +=  4-stride % 4;
            int ImagePixelDataSize = stride * bmpSource.PixelHeight * bmpSource.Format.BitsPerPixel / 8; ;
            ImagePixelData = new byte[ImagePixelDataSize];
            bmpSource.CopyPixels(ImagePixelData, stride, 0);
        }
        
        BitmapSource bmpSource=null;
        int stride=0;
        byte[] ImagePixelData=null;
        WinForm.OpenFileDialog openFileDialog1 = null;

       /// <summary>
       /// 处理图像
       /// </summary>
        private void ProcessImageData()
        {
            if (ImagePixelData == null)
                return;

            Parallel.For(0, ImagePixelData.Length, (i) =>
                {
                    byte value = ImagePixelData[i];
                    ImagePixelData[i] = (byte)(~value);
                });

            BitmapSource newImageSource = BitmapSource.Create(bmpSource.PixelWidth, bmpSource.PixelHeight,
                bmpSource.DpiX, bmpSource.DpiY, bmpSource.Format, bmpSource.Palette,
                ImagePixelData, stride);
            
            image1.Source = newImageSource;
        }

        private void btnLoadPicture_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == WinForm.DialogResult.OK)
            {
                LoadImage(openFileDialog1.FileName);
                image1.Source = bmpSource;
            }
        }

        private void btnReverseImage_Click(object sender, RoutedEventArgs e)
        {
            ProcessImageData();
        }
    }
}
