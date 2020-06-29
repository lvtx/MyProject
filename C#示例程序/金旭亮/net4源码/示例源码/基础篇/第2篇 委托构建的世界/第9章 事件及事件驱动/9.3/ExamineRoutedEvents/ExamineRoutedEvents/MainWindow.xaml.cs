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

            textHeadings.Inlines.Add(new Underline(new Run(
                String.Format(strFormat,
                "Routed Event", "sender", "Source", "OriginalSource"))));

            ResponseToEvent();

        }

        /// <summary>
        /// 挂接事件响应
        /// </summary>
        private void ResponseToEvent()
        {

            UIElement[] els = { win, root, btn };

            foreach (UIElement el in els)
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
        DateTime dtLast;
        const string strFormat = "{0,-30} {1,-15} {2,-15} {3,-15}";
        static readonly FontFamily fontfam = new FontFamily("Lucida Console");
        void AllPurposeEventHandler(object sender, RoutedEventArgs args)
        {
            // 依据时间添加空行
            DateTime dtNow = DateTime.Now;
            if (dtNow - dtLast > TimeSpan.FromMilliseconds(100))
                stackOutput.Children.Add(new TextBlock(new Run(" ")));
            dtLast = dtNow;

            // 显示事件的信息
            TextBlock text = new TextBlock();
            text.FontFamily = fontfam;
            text.Text = String.Format(strFormat,
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
