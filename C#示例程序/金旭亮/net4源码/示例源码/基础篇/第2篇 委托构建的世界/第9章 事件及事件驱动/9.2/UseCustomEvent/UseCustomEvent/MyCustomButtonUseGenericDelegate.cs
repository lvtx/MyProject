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
    public partial class MyCustomButtonUseGenericDelegate :Button
    {
        public MyCustomButtonUseGenericDelegate()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 定义自定义事件，使用.NET预定义的泛型委托
        /// </summary>
        public event EventHandler<MyClickEventArgs> MyClick;

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
}
