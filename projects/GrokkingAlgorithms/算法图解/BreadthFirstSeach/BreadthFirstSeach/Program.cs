using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadthFirstSeach
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleGraph search_graph = new SimpleGraph();
            string[] you = { "alice", "bob", "claire" };
            string[] bob = {"anuj", "peggy"};
            string[] alice = { "peggy" };
            string[] claire = { "thom", "jonny" };
            string[] anuj = { };
            string[] peggy = { };
            string[] thom = { };
            string[] jonny = { };

            search_graph.edges.Add("you", you);
            search_graph.edges.Add("bob", bob);
            search_graph.edges.Add("alice", alice);
            search_graph.edges.Add("claire", claire);
            search_graph.edges.Add("anuj", anuj);
            search_graph.edges.Add("peggy", peggy);
            search_graph.edges.Add("thom", thom);
            search_graph.edges.Add("jonny", jonny);

            //string[] a = search_graph.edges["you"];
            //for (int i = 0; i < a.Length; i++)
            //{
            //    Console.WriteLine(a[i]);
            //}
            Queue queue = new Queue();
            queue.Enqueue(search_graph.edges["you"]);
            Console.WriteLine("queue.count = {0}", queue.Count);
            foreach (string[] item in queue)
            {    
                for (int i = 0; i < item.Length; i++)
                {
                    queue.Enqueue(search_graph.edges[item[i]]);
                    string a = item[i];
                    Console.Write(a + " " + '\n'); 
                }
                queue.Dequeue();
            }
            Console.WriteLine("queue.count = {0}",queue.Count);
            Console.Read();
            //while (queue.Count != 0)
            //{
            //    string[] current = ((string[])queue.Dequeue())[queue.Count];
            //}
        }

        private static void BreadthFirstSearch(SimpleGraph graph, string start)
        {
            Queue queue = new Queue();
            queue.Enqueue(start);

            while (queue.Count != 0)
            {
                string[] current = (string[])queue.Dequeue();
            }
        }
    }

    class SimpleGraph
    {
        public SimpleGraph()
        {
            edges = new Dictionary<string, string[]>();
        }
        public Dictionary<string, string[]> edges;

        public string[] neighbors(string id)
        {
            return edges[id];
        }
    } 
}
