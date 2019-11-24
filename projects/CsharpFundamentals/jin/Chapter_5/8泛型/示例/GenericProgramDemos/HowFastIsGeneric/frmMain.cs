using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace UseGenericExampleForCS
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        long beginTime, endTime, usedTime;//代码运行时间


        //用于测试的集合
        ArrayList arr1 = new ArrayList();
        List<long> arr2 = new List<long>();

        //加入集合的元素个数
        private const int ElementCounters = 5000000;

        //提示信息
        String strInfo = "{0},运行时间：{1} ticks" + Environment.NewLine;

        private void AddElement()
        {
            //起始时间
            beginTime = Environment.TickCount;


            //清除原有数据
            arr1.Clear();
            arr2.Clear();
            //向集合中加入元素
           
            for (int i = 0; i < ElementCounters; i++)
                if (rdoArrayList.Checked)
                    arr1.Add(i);  //非泛型版本
                else
                    arr2.Add(i);

            endTime = Environment.TickCount;

            usedTime = endTime - beginTime;
            if (rdoArrayList.Checked)
                txtInfo.Text += String.Format(strInfo, "向集合中加入元素：" + ElementCounters + "个" , usedTime);
            else
                txtInfo.Text += String.Format(strInfo, "向泛型集合中加入元素：" + ElementCounters + "个", usedTime);

        }


        private void SumElement()
        {
         //起始时间
            beginTime = Environment.TickCount;

        //清除原有数据
        arr1.Clear();
        arr2.Clear();
        //向集合中加入元素
         int i ;
        for (i = 0 ;i< ElementCounters;i++)
            if (rdoArrayList.Checked )
                arr1.Add(i);  //非泛型版本
            else
                arr2.Add(i);
        
        //求和
        long ret=0;
        for (i = 0;i< ElementCounters;i++)
            if (rdoArrayList.Checked )
                ret += Convert.ToInt64( arr1[i]);  //非泛型版本
            else
                ret += arr2[i];

        endTime =Environment.TickCount;

        usedTime = endTime - beginTime;
        if (rdoArrayList.Checked)
            txtInfo.Text += String.Format(strInfo, "对集合" + ElementCounters.ToString() + "个元素求和：" + ret , usedTime);
        else
            txtInfo.Text += String.Format(strInfo, "对泛型集合" + ElementCounters.ToString() + "个元素求和：" + ret , usedTime);
            
            }

        private void btnAddElement_Click(object sender, EventArgs e)
        {
            AddElement();
        }

        private void btnSumElement_Click(object sender, EventArgs e)
        {
            SumElement();
        }

    }
}