using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyAsyncThread
{
    public delegate int[] CreateIntArrayDelegate();
    public delegate void ShowArrayDelegate(int[] arr);
    public partial class MainWindow : Window
    {
        private void btnArgThread_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("btnArgThread_Click Start {0} {1}",
                Thread.CurrentThread.ManagedThreadId.ToString(),DateTime.Now.ToString());
            MethodHelper argu = new MethodHelper() { x = 10,y = 20};
            AddFunc func = new AddFunc();
            Thread thread = new Thread(func.Add);
            thread.Start(argu);
            thread.Join();
            Console.WriteLine(argu.returnValue);
            Console.WriteLine("btnArgThread_Click End {0} {1}",
                Thread.CurrentThread.ManagedThreadId.ToString(),DateTime.Now.ToString());
        }
        #region "btnArrayThread_Click"
        public void DoWithArray(object obj)
        {
            ThreadMethodHelper argu = obj as ThreadMethodHelper;
            for (int i = 0; i < argu.arr.Length; i++)
            {
                if (argu.arr[i] > argu.MaxValue)
                {
                    argu.MaxValue = argu.arr[i];
                }
                if (argu.arr[i] < argu.MinValue)
                {
                    argu.MinValue = argu.arr[i];
                }
                argu.Sum += argu.arr[i];
            }
            argu.Averge = argu.Sum / argu.arr.Length;
        }
        #endregion

        private void btnArrayThread_Click(object sender, RoutedEventArgs e)
        {
            ThreadMethodHelper argu = new ThreadMethodHelper();
            argu.arr = new int[] { -1, 9, 100, 78, 23, 54, -90 };
            Thread thread = new Thread(DoWithArray);
            thread.Start(argu);
            thread.Join();
            Console.WriteLine("MaxValue = {0}",argu.MaxValue);
            Console.WriteLine("MinValue = {0}",argu.MinValue);
            Console.WriteLine("Sum = {0}",argu.Sum);
            Console.WriteLine("Averge = {0}",argu.Averge);
        }
        private void btnArraySort_Click(object sender, RoutedEventArgs e)
        {
            SortArrayThread obj = new SortArrayThread();
            obj.createArr = CreateIntArray;
            obj.showArr = ShowArray;
            Thread thread = new Thread(obj.SortArray);
            thread.Start();
            //thread.Join();
            Console.ReadKey();
        }
        #region "btnArraySort_Click"
        public int[] CreateIntArray()
        {
            int[] arr = new int[10];
            Random ran = new Random();
            for (int i = 0; i < 10; i++)
            {
                arr[i] = ran.Next(1, 100);
            }
            return arr;
        }
        public void ShowArray(int[] arr)
        {
            foreach (int elem in arr)
            {
                Console.Write(elem.ToString() + " ");
            }
        }
        #endregion
    }
    #region "btnArraySort_Click"
    class SortArrayThread
    {
        public CreateIntArrayDelegate createArr;
        public ShowArrayDelegate showArr;
        public void SortArray()
        {
            int[] arr = createArr();
            Console.WriteLine("原始数组:");
            showArr(arr);
            int temp = 0;
            for (int i = 0; i < arr.Length-1; i++)
            {
                for (int j = i+1; j < arr.Length; j++)
                {
                    if (arr[i] > arr[j])
                    {
                        temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
            Console.WriteLine("\n排序之后");
            showArr(arr);
        }
    }
    #endregion

    #region "btnArgThread_Click"
    class AddFunc
    {
        public void Add(object argu)
        {
            int x = (argu as MethodHelper).x;
            int y = (argu as MethodHelper).y;
            (argu as MethodHelper).returnValue = x + y;
        }
    }
    class MethodHelper
    {
        public int x;
        public int y;
        public int returnValue;
    }
    #endregion

    #region "btnArrayThread_Click"
    class ThreadMethodHelper
    {
        public int[] arr;
        public int MaxValue = 0;
        public int MinValue = 0;
        public long Sum = 0;
        public double Averge = 0;
    }
    #endregion
}
