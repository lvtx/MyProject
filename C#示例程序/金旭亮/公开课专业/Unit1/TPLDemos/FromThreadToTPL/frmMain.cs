using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;


namespace CalculateVarianceOfPopulation
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 数据总个数（注意要能被ThreadCount整除)
        /// </summary>
        private const int DataSize = 90000000;


        /// <summary>
        /// 测试数据
        /// </summary>
        private double[] Data = new double[DataSize];


        #region "生成测试数据"
        private void btnGenerateData_Click(object sender, EventArgs e)
        {
            ShowInfo("正在生成测试数据，请稍候...");
            Thread th = new Thread(GeneratePopulation);
            th.IsBackground = true;
            th.Start();
        }

        /// <summary>
        /// 生成测试数据
        /// </summary>
        private void GeneratePopulation()
        {
            Random ran = new Random();
            for (int i = 0; i < DataSize; i++)
                Data[i] = ran.NextDouble() * 100;
                
            string info = string.Format("\n已生成{0}个测试数据\n", DataSize);
            ShowInfo(info);
            EnableDisableControl();
        }

        #endregion


        #region "串行处理"
        /// <summary>
        /// 计算数据平均值(串行算法)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private double CalcuateMeanInSequence(double[] data)
        {
            double sum = 0;
            for (int i = 0; i < data.Length; i++)
                sum += data[i];
            return sum / data.Length;
        }

        /// <summary>
        /// 计算方差(串行算法）
        /// </summary>
        /// <param name="mean">要统计数据的平均值</param>
        private double CalculateVarianceInSequence(double mean)
        {

            double sum = 0;
            double SubtractionValue = 0;
            for (int i = 0; i < Data.Length; i++)
            {
                //保存数据项与平均值的差
                SubtractionValue = Data[i] - mean;
                //求平方和
                sum += SubtractionValue * SubtractionValue;
            }
            return sum / Data.Length;
        }

        private void btnUseSequence_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(DoWorkInSequence);
            th.IsBackground = true;
            th.Start();
        }
        /// <summary>
        /// 串行完成整个工作
        /// </summary>
        private void DoWorkInSequence()
        {
            string str = "";
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ShowInfo("\n=======串行算法========\n");
            ShowInfo("正在计算数据的平均值...\n");
            double mean = CalcuateMeanInSequence(Data);
            str = string.Format("测试数据的平均值为：{0}\n", mean);
            ShowInfo(str);
            str = "\n开始计算方差...\n";
            ShowInfo(str);

            double variance = CalculateVarianceInSequence(mean);

            str = string.Format("测试数据的方差为:{0}\n", variance);
            ShowInfo(str);
            str = string.Format("\n串行算法用时:{0}毫秒\n", sw.ElapsedMilliseconds);
            ShowInfo(str);




        }

        #endregion


        #region "并行处理（使用线程）"

       

        /// <summary>
        /// 每个数据与平均值的差值的平方和
        /// </summary>
        private double SquareSumUsedByThread = 0;

        /// <summary>
        /// 用于等待工作线程运行结束
        /// </summary>
        private CountdownEvent counterForThread = new CountdownEvent(Environment.ProcessorCount);

        /// <summary>
        /// 用于互斥访问线程共享变量SquareSum
        /// </summary>
        private object SquareSumLockObject = new object();

        /// <summary>
        /// 线程安全的显示信息函数
        /// </summary>
        /// <param name="Info"></param>
        private void ShowInfo(string Info)
        {

            if (InvokeRequired)
            {
                Action<string> del = (str) => { rtfInfo.AppendText(str); };
                this.BeginInvoke(del, Info);
            }
            else
                rtfInfo.AppendText(Info);
        }

        /// <summary>
        /// 激活用于统计数据的控件，灰掉“生成数据”的控件
        /// </summary>
        private void EnableDisableControl()
        {
            Action del = () =>
            {
                groupBox1.Enabled = true;
                btnGenerateData.Enabled = false;
            };
            this.Invoke(del);
        }

        /// <summary>
        /// 使用多线程并行计算每个数据与平均值的差值的平方和
        /// </summary>
        /// <param name="ThreadArguObject"></param>

        private void CalculateSquareSumInParallelWithThread(object ThreadArguObject)
        {
            ThreadArgu argu = ThreadArguObject as ThreadArgu;

            double sum = 0;
            double SubtractionValue = 0;
            for (int i = 0; i < argu.Count; i++)
            {
                //保存数据项与平均值的差
                SubtractionValue = Data[argu.StartIndex + i] - argu.Mean;
                //求平方和
                sum += SubtractionValue * SubtractionValue;
            }
            lock (SquareSumLockObject)
            {
                SquareSumUsedByThread += sum;
            };
            //通知别的线程自己已经完成工作
            counterForThread.Signal();
            string str = string.Format("\n工作线程{0}已完成工作\n", Thread.CurrentThread.ManagedThreadId);
            ShowInfo(str);

        }

        /// <summary>
        /// 使用线程并行计算方差
        /// </summary>
        private void DoWorkInParalleUseThread()
        {
            string str = "";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            counterForThread.Reset();
            SquareSumUsedByThread = 0;
            ShowInfo("\n=======使用线程的并行算法========\n");
            ShowInfo("正在计算数据的平均值...\n");
            double mean = CalcuateMeanInSequence(Data);
            str = string.Format("测试数据的平均值为：{0}\n", mean);
            ShowInfo(str);
            //获取CPU核心数，作为并行线程数
            int ThreadCount = Environment.ProcessorCount;
            str = string.Format("\n现在启动{0}个工作线程开始计算每个数据与平均值的差值的平方和...\n", ThreadCount);

            //计算每个线程需要处理的数据项数
            int workload = DataSize / ThreadCount;

            //启动ThreadCount个工作线程并行执行工作
            for (int i = 0; i < ThreadCount; i++)
            {
                ThreadArgu argu = new ThreadArgu();
                argu.Count = workload;
                argu.Mean = mean;
                argu.StartIndex = workload * i;

                Thread th = new Thread(CalculateSquareSumInParallelWithThread);
                th.IsBackground = true;
                th.Start(argu);

            }

            //主控线程等待工作线程完成工作
            counterForThread.Wait();

            str = "\n所有工作线程都已经完成工作，现在可以计算方差...\n";
            ShowInfo(str);


            double variance = SquareSumUsedByThread / Data.Length;
            str = string.Format("测试数据的方差为:{0}\n", variance);
            ShowInfo(str);

            str = string.Format("\n使用线程的并行算法用时:{0}毫秒\n", sw.ElapsedMilliseconds);
            ShowInfo(str);

        }

        private void btnUseThread_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(DoWorkInParalleUseThread);
            th.IsBackground = true;
            th.Start();
        }


        #endregion


        #region "并行处理（使用TPL）"

        /// <summary>
        /// 针对特定索引范围内的元素，根据处理器个数分区，然后并行对每个分区并行执行一个数据处理函数
        /// </summary>
        /// <param name="fromInclusive"></param>
        /// <param name="toExclusive"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static ParallelLoopResult ForRange(int fromInclusive, int toExclusive, Action<int, int> body)
        {
           
           // 依据本机所包容的处理器个数来决定并行处理的任务数
            int numberOfPartitions = System.Environment.ProcessorCount;

            // 获取要计算的数据范围
            int range = toExclusive - fromInclusive;

            //计算出每个并行任务要计算的数据个数
            int stride = range / numberOfPartitions;
            if (range == 0) numberOfPartitions = 0;
            
            //并行执行计算任务
            return Parallel.For(0, numberOfPartitions, i =>
            {
                int start = i * stride;
                int end = (i == numberOfPartitions - 1) ? toExclusive : start + stride;
                body(start, end);
            });
        }

        /// <summary>
        /// 使用TPL并行计算方差
        /// </summary>
        private void DoWorkInParallelUseTPL()
        {

            string str = "";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ShowInfo("\n=======并行算法（使用TPL）========\n");
            SquareSumUsedByThread = 0;

            str = "\n计算平均值\n";
            ShowInfo(str);
            double mean = CalcuateMeanInSequence(Data);
            str = string.Format("测试数据的平均值为：{0}\n", mean);
            ShowInfo(str);

            ForRange(0, Data.Length, (start, end) =>
            {
                double sum = 0;
                double temp = 0;
                for (int i = start; i < end; i++)
                {
                    temp = Data[i] - mean;

                    sum += temp * temp;
                }
                //保存结果
                lock (SquareSumLockObject)
                {
                    SquareSumUsedByThread += sum;
                };
            });

            str = string.Format("\n测试数据的方差为:{0}\n", SquareSumUsedByThread / DataSize);
            ShowInfo(str);

            str = string.Format("\n并行算法用时:{0}毫秒\n", sw.ElapsedMilliseconds);
            ShowInfo(str);


           


        }

        private void btnUseTPL_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(DoWorkInParallelUseTPL);
            th.IsBackground = true;
            th.Start();
        }

        #endregion

    }

    /// <summary>
    /// 向工作线程传入的参数
    /// </summary>
    public class ThreadArgu
    {
        /// <summary>
        /// 数组中元素的开始索引
        /// </summary>
        public int StartIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 要计算的元素个数
        /// </summary>
        public int Count
        {
            get;
            set;
        }
        /// <summary>
        /// 数据的平均值
        /// </summary>
        public double Mean
        {
            get;
            set;
        }
    }


}
