using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue_
{
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Queue queue = new Queue();

    //        queue.Enqueue('A');
    //        queue.Enqueue('M');
    //        queue.Enqueue('G');
    //        queue.Enqueue('W');

    //        Console.WriteLine("Current queue: ");
    //        foreach (char c in queue)
    //        {
    //            Console.Write(c + " ");
    //        }
    //        Console.WriteLine();
    //        queue.Enqueue('V');//向Queue的末尾添加一个对象
    //        queue.Enqueue('H');
    //        Console.WriteLine("After using Enqueue ");
    //        foreach (var c in queue)
    //        {
    //            Console.Write(c + " ");
    //        }
    //        Console.WriteLine();

    //        Console.WriteLine("Removing some values ");
    //        char ch = (char)queue.Dequeue();
    //        Console.WriteLine("The removed value: {0}", ch);
    //        ch = (char)queue.Dequeue();
    //        Console.WriteLine("The removed value: {0}", ch);
    //        foreach (var c in queue)
    //        {
    //            Console.Write(c + " ");
    //        }
    //        Console.WriteLine();
    //        Console.WriteLine("元素个数{0}", queue.Count);
    //        Console.ReadKey();
    //    }
    //}
    class SimpleGraph
    {
        public SimpleGraph()
        {
            // 初始化边表
            edges = new Dictionary<char, char[]>();
        }

        public Dictionary<char, char[]> edges;

        public char[] neighbors(char id)
        {
            return edges[id];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 初始化图
            SimpleGraph example_graph = new SimpleGraph();
            char[] A = { 'B' };
            char[] B = { 'A', 'C', 'D' };
            char[] C = { 'A' };
            char[] D = { 'E', 'A' };
            char[] E = { 'B' };
            example_graph.edges.Add('A', A);
            example_graph.edges.Add('B', B);
            example_graph.edges.Add('C', C);
            example_graph.edges.Add('D', D);
            example_graph.edges.Add('E', E);

            breadthFirstSearch(example_graph, 'A');

            // 防止退出
            Console.ReadKey();
        }

        private static void breadthFirstSearch(SimpleGraph graph, char start)
        {
            // 初始化队列
            Queue queue = new Queue();
            queue.Enqueue(start);
            Dictionary<char, bool> visited = new Dictionary<char, bool>();
            visited[start] = true;

            while (queue.Count != 0)
            {
                char current = (char)queue.Dequeue();
                Console.WriteLine("Visiting: " + current);
                foreach (char next in graph.neighbors(current))
                {
                    if (!visited.ContainsKey(next))
                    {
                        queue.Enqueue(next);
                        visited[next] = true;
                    }
                }
            }
        }
    }
}
