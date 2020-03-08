using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace TimeProgressBar
{
    public  delegate void TimeIsUpDelegate();

    /// <summary>
    /// 一个可定时并显示时间流逝的进度条
    /// </summary>
    public partial class TimeProgressBar : UserControl
    {
        public TimeProgressBar()
        {
            InitializeComponent();
            //将计时处理程序与闹钟控件绑定
            timer1.Tick += new EventHandler(Begin);
            timer1.Enabled = false;  //先将闹钟停下
        }

        /// <summary>
        /// 时间到事件
        /// </summary>
        public event TimeIsUpDelegate TimeIsUp;

        private double  TotalSeconds=0;//计时总时间(按秒)，用于计算完成比例
        private TimeSpan TotalTime;//计时总时间
        private DateTime BeginTime; //计时起始时间

        /// <summary>
        /// 设置计时时间长度
        /// </summary>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        public void SetTimeSpan(int hour,int minute,int second)
        {
            TotalTime = new TimeSpan(hour, minute, second);
            TotalSeconds =TotalTime.TotalSeconds ;
            Initialize();
        }
        //private long CalculateSeconds(int hour, int minute, int second)
        //{
        //    return second + minute * 60 + hour * 60 * 60;
        //}
        //重新初始化
        private void Initialize()
        {
            BeginTime = DateTime.Now;  //计时起点
            progressBar1.Value = 0; //进度条回到起点
            timer1.Enabled = true;  //开始计时


        }
        //开始计时
        private void Begin(object sender, EventArgs e)
        {
            TimeSpan ElapsedTime = (DateTime.Now - BeginTime);//时间过去了多久？
            //已过去时间（％）＝已过去时间/总时间
            int temp=(int)(ElapsedTime.TotalSeconds / TotalSeconds*100);
            //设置进度条刻度以显示时间流逝状态
            if (temp>progressBar1.Maximum )
                temp=progressBar1.Maximum ;
            progressBar1.Value = temp;
            //时间用完
            if (ElapsedTime >= TotalTime)
            {
                timer1.Enabled = false;//停止计时
                if(TimeIsUp!=null)
                    TimeIsUp(); //激发事件
            }
        }     
    }
}