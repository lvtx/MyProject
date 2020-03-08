using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ExamineRoutedEvents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //标题
            textHeadings.Inlines.Add(new Underline(
                new Run(string.Format
                (strFormat, "Routed Event", "sender", "Source", "OriginalSource"))));
            ResponseToEvent();
        }
        /// <summary>
        /// 格式化
        /// </summary>
        const string strFormat = "{0,-30} {1,-15} {2,-15} {3,-15}";
        /// <summary>
        /// 一个字体族
        /// </summary>
        static readonly FontFamily fontfam = new FontFamily("Lucida Console");
        /// <summary>
        /// 挂接事件的响应
        /// </summary>
        private void ResponseToEvent()
        {
            UIElement[] els = { btn, root, win };
            foreach (var el in els)
            {
                // 键盘
                el.PreviewKeyDown += AllPurposeEventHandler;
                el.PreviewKeyUp += AllPurposeEventHandler;
                el.PreviewTextInput += AllPurposeEventHandler;
                el.KeyDown += AllPurposeEventHandler;
                el.KeyUp += AllPurposeEventHandler;
                el.TextInput += AllPurposeEventHandler;

                // 鼠标
                el.MouseDown += AllPurposeEventHandler;
                el.MouseUp += AllPurposeEventHandler;
                el.PreviewMouseDown += AllPurposeEventHandler;
                el.PreviewMouseUp += AllPurposeEventHandler;

                // 单击
                el.AddHandler(Button.ClickEvent,
                    new RoutedEventHandler(AllPurposeEventHandler));
            }
        }
        /// <summary>
        /// 事件响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AllPurposeEventHandler(object sender, RoutedEventArgs args)
        {
            TextBlock text = new TextBlock();
            text.FontFamily = fontfam;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.Text = string.Format(strFormat,
                args.RoutedEvent.Name,
                TypeWithoutNamespace(sender),
                TypeWithoutNamespace(args.Source),
                TypeWithoutNamespace(args.OriginalSource));
            stackOutput.Children.Add(text);
            (stackOutput.Parent as ScrollViewer).ScrollToBottom();
        }
        //去掉命名空间
        string TypeWithoutNamespace(object obj)
        {
            string[] astr = obj.GetType().ToString().Split('.');
            return astr[astr.Length - 1];
        }
    }
}
