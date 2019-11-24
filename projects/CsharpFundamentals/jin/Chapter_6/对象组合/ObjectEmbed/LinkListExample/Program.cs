using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkListExample
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkNode head = new LinkNode() { Data = "head" };
            LinkNode first = new LinkNode() { Data = "first" };
            LinkNode second = new LinkNode() { Data = "second" };
            LinkNode tail = new LinkNode() { Data = "tail" };

            head.next = first;
            first.next = second;
            second.next = tail;
            LinkNode node = head;
            while(node != null)
            {
                if (node.next != null)
                {
                    Console.Write(node.Data + "->");
                }
                else
                    if (node.next == null)
                {
                    Console.WriteLine(node.Data);
                }
                node = node.next;
            }
            Console.ReadLine();
        }
    }
}
