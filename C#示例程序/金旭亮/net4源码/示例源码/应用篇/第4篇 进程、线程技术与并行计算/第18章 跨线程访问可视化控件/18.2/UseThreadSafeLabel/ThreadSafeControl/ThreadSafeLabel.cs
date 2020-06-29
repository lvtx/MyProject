using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ThreadSafeControl
{
    public class ThreadSafeLabel : Label
    {
       //覆盖基类的Text属性
        public override string Text
        {
            get
            {
                if (InvokeRequired) //如果是跨线程调用
                {
                    Func<string> getDel = delegate()
                    {
                        return base.Text;
                    };
                    return (string)Invoke(getDel);
                }
                else  //普通调用
                    return base.Text;
            }
            set
            {
                if (InvokeRequired) //如果是跨线程调用
                {
                    Action<string> setDel = delegate(string text)
                    {
                        base.Text = text;
                    };

                    Invoke(setDel, new object[] { value });
                }
                else //普通调用
                    base.Text = value;
            }
        }
    }
}
