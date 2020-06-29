using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeProject
{
    public static class Sort
    {
        /// <summary>
        /// 选择排序
        /// 依次找出数组中最小的元素
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<int> SelectionSort(List<int> items)
        {
            int count = items.Count;
            for (int i = 0; i < count; i++)
            {
                //找出最小的元素
                int smallest = items[i];
                int t = 0;
                //寻找比当前元素小的元素
                for (int j = i; j < count; j++)
                {
                    if (smallest > items[j])
                    {
                        t = smallest;
                        smallest = items[j];
                        items[j] = t;
                    }
                }
                //如果没有找到直接放到末尾
                items[i] = smallest;
            }
            return items;
        }
    }
}
