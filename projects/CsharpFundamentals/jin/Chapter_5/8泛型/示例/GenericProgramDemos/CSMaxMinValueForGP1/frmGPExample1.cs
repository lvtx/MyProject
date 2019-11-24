using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace  frmGPExample1
{
    public partial class frmGPExample1 : Form
    {
        public frmGPExample1()
        {
            InitializeComponent();
        }

#region "变量区"
        //两个将要被排序的数组
        char[] CharArray=new Char[10];
        int[] IntArray=new int[10];

         //整数数组元素中的最大最小值
        private int MaxInt, MinInt;
         //字符数组元素中的最大最小值
        private char MaxChar, MinChar;

#endregion

    

        //用随机数据填充数组,并显示在列表框中
        private void FillArray()
        {
            int i = 0;
            Random ran = new Random();
            lstData.Items.Clear();
            if (rdoInteger.Checked)
            {
                for (i = 0; i <= IntArray.GetUpperBound(0); i++)
                {
                    IntArray[i] = ran.Next(0, 100);
                    lstData.Items.Add(IntArray[i]);
                }
            }
            if (rdoChar.Checked)
            {
                for (i = 0; i <= CharArray.GetUpperBound(0); i++)
                {
                    int charCode = ran.Next(0, 26);
                    CharArray[i] = (char)(charCode + 'A');
                    lstData.Items.Add(CharArray[i] + " :ASC码值＝" 
                        + Convert.ToString((int)(CharArray[i])));
                }
            }
        }
       
    
    //显示结果：
    private  void  ShowResult()
    {
       
        if (rdoInteger.Checked )
        {
            lblMax.Text = "最大值：" + MaxInt;
            lblMin.Text = "最小值：" + MinInt;
        }

        if (rdoChar.Checked )
        {
           
            lblMax.Text = "最大值：" + MaxChar;
            lblMin.Text = "最小值：" + MinChar;
           
        }
    }



#region "普通方法"

    //在整数数组中查找最大最小值 
    private void GetMaxMinValueFromIntArray(int[] datas)
    {
        MaxInt = datas[0];
        MinInt = datas[0];
        for (int i= 1 ;i<= IntArray.GetUpperBound(0);i++)
        {
            if (MaxInt < datas[i]) 
                MaxInt = datas[i];
            if( MinInt > datas[i]) 
                MinInt = datas[i];
        }
    } 

    //在字符数组中查找最大最小值
    private  void GetMaxMinValueFromCharArray(char [] datas  )
    {
        MaxChar = datas[0];
        MinChar = datas[0];
        for (int i  = 1;i<= IntArray.GetUpperBound(0);i++)
        {
            if (MaxChar < datas[i] )
                MaxChar = datas[i];
            if (MinChar > datas[i] )
                MinChar = datas[i];
        }
    }

    private void OnButtonClick1()
{
        //填充数组
        FillArray();
        if (rdoInteger.Checked )
            GetMaxMinValueFromIntArray(IntArray);
        if (rdoChar.Checked )
            GetMaxMinValueFromCharArray(CharArray);
        //显示数据处理结果
        ShowResult();
    }

#endregion

#region "泛型方法版本"

    private void GetMaxMinVauleFromArray<T>(T[] datas  ,
        ref T Max ,ref T Min) where T:IComparable 
    {
        Max = datas[0];
        Min = datas[0];
        for(int i  = 1 ;i<=IntArray.GetUpperBound(0);i++)
        {
            if( Max.CompareTo(datas[i])<0)
                Max = datas[i];
            if (Min.CompareTo(datas[i])>0)
                Min = datas[i];
        }
    }

    private void OnButtonClick2()
    {
        //填充数组
        FillArray();
        //根据用户选项填充对应的数组
        if( rdoInteger.Checked )
            GetMaxMinVauleFromArray<int>(IntArray, ref MaxInt,ref MinInt);
        
        if (rdoChar.Checked )
            GetMaxMinVauleFromArray<char>(CharArray,ref MaxChar, ref MinChar);
      
      //显示数据处理结果
        ShowResult();
    }

#endregion


       private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnFillArray_Click(object sender, EventArgs e)
        {
             OnButtonClick1();//普通版本 
            //OnButtonClick2();//泛型方法版本
        }
    }
}