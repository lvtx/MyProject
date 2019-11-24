using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Recursion
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private string Story = "";

        //故事主体：
        private void WriteStory()
        {
            Story = "从前有座山，山里有座庙。\n";
            Story += "庙里有两个和尚，在讲故事。\n";
            Story += "讲什么故事呢？……\n";
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            //清空文本
            RichTextBox1.Text = "";
            //老和尚开始没完没了地讲故事
            DoRecursion((int)updnTimes.Value);
        }
        //用于实现递归调用
        private void DoRecursion(int times)
        {

            //结束条件
            if (times == 0)
                return;

            //递归调用，参数减一
            DoRecursion(times - 1);

            //每次递归调用时要完成的工作
            //to do: 如果将以下这两句移到DoRecursion()一句之后，
            //       会发生什么？
            RichTextBox1.AppendText("第 " + times + " 次\n");
            RichTextBox1.AppendText(Story);

            ////递归调用，参数减一
            //DoRecursion(times - 1);

            //////每次递归调用时要完成的工作
            //RichTextBox1.AppendText("第 " + times + " 次\n");
            //RichTextBox1.AppendText(Story);

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //把故事主体写好
            WriteStory();
        }
    }
}
