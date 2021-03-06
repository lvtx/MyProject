﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SortArray
{
    class Program
    {
        static void Main(string[] args)
        {
            MyThread obj = new MyThread();
            obj.createArr = CreateIntArray;
            obj.showArr = ShowArray;
            Thread th = new Thread(obj.SortArray);
            th.Start();
          
            System.Console.ReadKey();

        }

        //创建数组，以随机数填充
        static int[] CreateIntArray()
        {
            int[] arr=new int[10];
            Random ran=new Random();
            for(int i=0;i<10;i++)
                arr[i]=ran.Next(1,100); //生成1～100间的随机数
            return arr;
        }
        
        //显示所有数组元素
        static void ShowArray(int[] arr)
        {
            foreach (int elem in arr)
                System.Console.Write(elem.ToString() + "  ");
        }

    }

    


    class MyThread
    {
        //由外界负责提供的数组生成函数和显示数组内容的函数
        public Func<int[]> createArr;
        public Action<int[]> showArr;

        public void SortArray()
        {
            //创建一个数组
            int[] arr=createArr();
            System.Console.WriteLine("原始数组：");
            //显示数组原先的元素内容
            showArr(arr);
            //使用冒泡法对数组元素排序（升序）
            int temp = 0;
            for(int i=0;i<arr.Length-1 ;i++)
                for(int j=i+1;j<arr.Length;j++)
                    if (arr[i] > arr[j]) //交换两个数组元素
                    {   
                        temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
            System.Console.WriteLine("\n排序之后：");
            //显示排序之后的数组元素
            showArr(arr);
        }

    }
}
