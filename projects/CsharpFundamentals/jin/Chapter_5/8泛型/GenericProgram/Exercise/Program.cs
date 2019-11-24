using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    class Program
    {
        static void Main(string[] args)
        {
            List<char> array = new List<char>() { 'b','a','e','g','f'};
            Sort(array);
            foreach (var item in array)
            {
                Console.Write(item + ", ");
            }
            Console.ReadLine();   
        }
        static void Sort<T>(List<T> array)
            where T:IComparable
        {
            T temp = default(T);//给temp赋初值
            int mark = 0;
            for (int i = 0; i < array.Count; i++)
            {
                mark = 0;
                temp = array[i];             
                for (int j = i; j < array.Count; j++)
                {
                    //不要忘记等号要与自身比较一次
                    if (temp.CompareTo(array[j]) >= 0)
                    {
                        temp = array[j];
                        mark = j;
                    }
                }
                array[mark] = array[i];
                array[i] = temp;
            }
        }
    }
}
