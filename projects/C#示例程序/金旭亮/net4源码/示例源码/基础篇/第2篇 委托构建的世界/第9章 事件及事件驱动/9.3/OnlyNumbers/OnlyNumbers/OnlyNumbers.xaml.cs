using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OnlyNumbers
{
    /// <summary>
    /// Interaction logic for NoNumbers.xaml
    /// </summary>

    public partial class OnlyNumbersTextBox : System.Windows.Window
    {

        public OnlyNumbersTextBox()
        {
            InitializeComponent();
        }

        private void pnl_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            short val;
            //如果是非数字，且非小数点，则取消输入
            if (!Int16.TryParse(e.Text, out val) && e.Text!=".")
            {
                e.Handled = true;
                return;
            }
        
            //是小数点
            if (e.Text == ".")
            {
                TextBox txt = e.OriginalSource as TextBox;  //获取当前正在输入的文本框对象引用
                if (txt != null)
                {
                    //查看一下前面是否已经输入过了小数点
                    if (txt.Text.IndexOf(".") != -1)
                        e.Handled = true; //取消输入
                }
            }
        }

        private void pnl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            
        }

    }
}