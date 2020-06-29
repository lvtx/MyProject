using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[] { 10, 5, 2, 3, 6, 9 , 1};
            Sort sort = new Sort();
            array = sort.QuickSort(array,0,array.Length - 1);
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine(array[i]);
            }
            //Console.WriteLine("array length = {0}", array.Length);
        }
    }

    class Sort
    {
        //public int[] Array { get; set; }

        //public Sort(int[] array)
        //{
        //    Array = array;
        //}

        public int[] QuickSort(int[] array,int head,int tail)
        {
            int q;
            if(head < tail)
            {
                q = Partition(array, head, tail);
                QuickSort(array, head, q - 1);
                QuickSort(array, q + 1, tail);
            }
            return array;
        }

        private int Partition(int[] array,int head,int tail)//将数组划分为左小右大
        {
            //int mid = (head + tail) / 2;
            //初始的store不能直接用,第一个元素小于基准值store+1,元素位置不动
            //当有其他元素小于基准值store继续+1完成左右划分
            //循环结束所有元素都大于基准值store+1基准值移动到开头
            int store = head - 1;
            int pivot = array[tail];//基准值比它小的元素移到左边大的移到右边
            int t = 0;//交换数组元素位置时临时存储元素用的变量
            for (int i = head; i < tail; i++)
            {
                if(array[i] <= pivot)
                {
                    store += 1;
                    t = array[i];
                    array[i] = array[store];
                    array[store] = t;
                }
            }
            t = array[store + 1];//将基准值移动到分界处
            array[store + 1] = array[tail];
            array[tail] = t;
            return store + 1;//数组完成划分，记录下基准值的位置
        }
    }
}
