using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseCustomEvent
{
    public partial class MyCustomButton : Button
    {
        public MyCustomButton()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 自定义事件
        /// </summary>
        public event MyClickEventHandler MyClick;

        /// <summary>
        /// 点击次数
        /// </summary>
        private int ClickCount = 0; 
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ClickCount++;
            //激发自定义事件
            if (MyClick != null)
                MyClick(this, new MyClickEventArgs(ClickCount));
        }

        
    }

   
    /// <summary>
    /// MyClick事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MyClickEventHandler(Object sender,  MyClickEventArgs e);

    /// <summary>
    /// MyClick事件参数
    /// </summary>
    public class MyClickEventArgs : EventArgs
    {
        public int ClickCount = 0;  //单击次数
        public MyClickEventArgs(int ClickCountValue)
            : base()
        {
            ClickCount = ClickCountValue;
        }
    }
}
