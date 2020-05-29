using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DownloadWebImages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();

        }
        //当前要下载的Java教学PPT数目为49，这是依据教学网站上的PPT数目而定的
        private const int PPTCOUNT = 1;
        private void Init()
        {
            imageUrls = new List<string>();
            for (int i = 1; i <= PPTCOUNT; i++)
            {
                imageUrls.Add(String.Format("https://imgsa.baidu.com/exp/w=200/sign=3a6d6b1bfc1f3a295ac8d2cea924bce3/c83d70cf3bc79f3d4ee52ef5b6a1cd11738b29eb.jpg", i));
            }
        }
        //用于保存PPT的下载链接
        private List<string> imageUrls = null;
        //用于记录己下载的PPT
        private int DownloadCounter = 0;

        private ObservableCollection<ImageModel> images = new ObservableCollection<ImageModel>();

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            btnDownload.IsEnabled = false;
            DownloadCounter = 0;
            DownloadPPTsFromWeb();
        }
        /// <summary>
        /// 使用TPL并行下载PPT
        /// </summary>
        private void DownloadPPTsFromWeb()
        {
            //为每个PPT图片创建一个并行下载的Task对象
            foreach (var imageUrl in imageUrls)
            {
                WebClient client = new WebClient();
                Uri imagUri = new Uri(imageUrl);
                Task.Run(() =>
                {
                    return client.DownloadData(imagUri);
                }).ContinueWith((t) =>
                {
                    //如果下载过程出错，显示出错信息，注意这是跨线程更新UI控件
                    if (t.Exception != null)
                    {
                        Action info = () =>
                        {
                            btnDownload.IsEnabled = true;
                            tbInfo.Text = t.Exception.Flatten().Message;
                        };
                        Dispatcher.BeginInvoke(info);
                        return;
                    }
                    //下载成功，取出原始数据
                    byte[] data = t.Result;
                    //利用WPF的数据绑定机制，自动更新UI界面
                    Action del = () =>
                    {
                        images.Add(
                            new ImageModel
                            {
                                ImageSource = ByteArrayToBitmapImage(data)
                            }
                            );
                        DownloadCounter++;
                        if (DownloadCounter == imageUrls.Count)
                        {
                            btnDownload.IsEnabled = true;
                        }
                        tbInfo.Text = String.Format("己下载{0},共{1}", DownloadCounter, imageUrls.Count());
                    };
                    Dispatcher.BeginInvoke(del);
                });
            }
        }

        //将图片原始数据，转换为WPF可以使用的位图对象
        public BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(byteArray);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }
            return bmp;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //设定绑定数据源
            lstImages.Items.Clear();
            lstImages.ItemsSource = images;
        }
    }
    /// <summary>
    /// 设计一个支持WPF数据绑定的数据对象
    /// </summary>
    public class ImageModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
