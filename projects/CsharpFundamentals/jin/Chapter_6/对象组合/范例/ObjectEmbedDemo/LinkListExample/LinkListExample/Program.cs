using System;
using System.Collections.Generic;
using System.Text;

namespace LinkListExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //生成链表节点
            LinkNode Head = new LinkNode(){ Data="Head"};
            LinkNode First = new LinkNode() { Data = "First" };
            LinkNode Second = new LinkNode() { Data = "Second" };
            LinkNode Tail = new LinkNode() { Data = "Tail" };
         
            
            //建立链表
            Head.Next = First;
            First.Next = Second;
            Second.Next = Tail;
            Console.WriteLine("链表己创建，其内容为：");
            //访问链表的全部节点
            LinkNode node;
            node = Head;
            while (node != null)
            {
                //从node.Data中取出节点数据，干一些事
                if(node.Next!=null)
                    Console.Write("{0}-->", node.Data);
                else
                    Console.Write("{0}", node.Data);
                node = node.Next;//移往下一个节点
            }

            Console.ReadKey();
        }
    }
}
