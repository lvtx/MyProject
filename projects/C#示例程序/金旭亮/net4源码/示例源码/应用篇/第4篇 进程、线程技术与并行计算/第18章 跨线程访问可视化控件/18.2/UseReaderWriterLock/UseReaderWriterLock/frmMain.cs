using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace UseReaderWriterLock
{
    public delegate void ShowInfoDelegate(Label lbl,string info);

    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            Init();
        }
        //初始化系统
        private void Init()
        {
            reader1 = new Thread(ReaderThreadMethod);
            reader2 = new Thread(ReaderThreadMethod);
            reader3 = new Thread(ReaderThreadMethod);

        }


        //读取者线程函数
        private void ReaderThreadMethod(Object WhichOne)
        {
            while (true)
            {
                switch((int)WhichOne)
                {
                    case 1:
                        DoWithReader(mre1,lblReader1);
                        break;
                    case 2:
                        DoWithReader(mre2,lblReader2);
                        break;
                    case 3:
                        DoWithReader(mre3,lblReader3);
                        break;
                }
           }
        }
        //完成读取数据并显示的任务
        private void DoWithReader(ManualResetEvent mre,Label  lbl)
        {
            mre.WaitOne();//等待写线程通知
            rwl.AcquireReaderLock(-1);//无限等待，直到获取了读锁
            //显示信息
            lbl.Invoke(new ShowInfoDelegate(ShowInfo), new object[] { lbl, res.str });
            rwl.ReleaseReaderLock();//释放读锁
            mre.Reset();
        }
          //在标签上显示信息
        private void ShowInfo(Label lbl,string info)
        {
            lbl.Text = info;
            lbl.Refresh();
        }


        //写入者线程函数
        private void WriterThreadMethod(Object info)
        {
            rwl.AcquireWriterLock(-1); //无限等待,直到获取了写锁
            res.str = info.ToString(); //写入共享资源
            rwl.ReleaseWriterLock(); //释放写锁
            //通知读线程可以读
            mre1.Set();
            mre2.Set();
            mre3.Set();
        }

        //共享的资源
        private static SharedResource res=new SharedResource();
        //同步信号
        private ManualResetEvent mre1 = new ManualResetEvent(false);
        private ManualResetEvent mre2 = new ManualResetEvent(false);
        private ManualResetEvent mre3 = new ManualResetEvent(false);
        
        
        //单写多读锁
        private ReaderWriterLock rwl = new ReaderWriterLock();
        
        //三个读线程，一个写线程
        private Thread reader1, reader2, reader3, writer;

        private void btnWriter_Click(object sender, EventArgs e)
        {
            //创建写线程
            writer = new Thread(WriterThreadMethod);
            writer.Start(txtNumber.Text);
            //启动读线程
            if(reader1.IsAlive==false )
                    reader1.Start(1);
            if (reader2.IsAlive == false)
                    reader2.Start(2);
            if (reader3.IsAlive == false)
                    reader3.Start(3);
        }

      
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            reader1.Abort();
            reader2.Abort();
            reader3.Abort();
        }


    }

    //共享资源
    public class SharedResource
    {
        public string str="";
    }
}