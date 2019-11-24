using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exercise
{
    public partial class OnlyInputNum : Form
    {
        /// <summary>
        /// TextBox控件只允许输入数字
        /// 过滤掉其他字符
        /// </summary>
        public OnlyInputNum()
        {
            InitializeComponent();        
        }
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void TextBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)Keys.Back)
            {
                
            }
        }

        
    }
}
