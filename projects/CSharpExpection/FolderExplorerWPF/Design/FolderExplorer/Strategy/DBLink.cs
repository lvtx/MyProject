using System;
using System.Collections.Generic;
using FolderExplorer.Filesystem;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderExplorer.Strategy
{
    public class DbNode<T>
    {
        /// <summary>
        /// 用于存储数据
        /// </summary>
        public T e { get; set; }
        public DbNode<T> Prev { get; set; }
        public DbNode<T> Next { get; set; }
    }

    /// <summary>
    /// 用于前后导航的双向链表
    /// </summary>
    public class DbLink<T>
    {
        public DbLink()
        {
            headNode = null;
        }
        DbNode<T> headNode;
        DbNode<T> currentNode = new DbNode<T>();

        private T currElement;

        public T CurrElement
        {
            get
            {
                return currElement;
            }
        }

        private bool isCurrentAfterLast = false;
        /// <summary>
        /// 判断链表是否前进到开始第一个
        /// </summary>
        public bool IsCurrentAfterLast
        {
            get
            {
                return isCurrentAfterLast;
            }
        }
        private bool isCurrentBeforeFirst = false;
        /// <summary>
        /// 判断链表是否后移到最后一个
        /// </summary>
        public bool IsCurrentBeforeFirst
        {
            get
            {
                return isCurrentBeforeFirst;
            }
        }

        /// <summary>
        /// 加入一个新节点
        /// </summary>
        /// <param name="e"></param>
        public void AddNode(T e)
        {
            DbNode<T> newNode = new DbNode<T>();
            if (headNode == null)
            {
                headNode = newNode;
                newNode.Prev = null;
            }
            else
            {
                currentNode.Next = newNode;
                newNode.Prev = currentNode;
            }
            newNode.e = e;
            newNode.Next = null;
            currentNode = newNode;
        }
        /// <summary>
        /// 节点前移
        /// </summary>
        public void GoFoward()
        {
            if (currentNode.Next != null)
            {
                currentNode = currentNode.Next;
                currElement = currentNode.e;
                isCurrentBeforeFirst = false;
                if (currentNode.Next == null)
                    isCurrentAfterLast = true;
            }
        }
        /// <summary>
        /// 节点后移
        /// </summary>
        public void GoBack()
        {
            if (currentNode.Prev != null)
            {
                currentNode = currentNode.Prev;
                currElement = currentNode.e;
                isCurrentAfterLast = false;
                if (currentNode.Prev == null)
                    isCurrentBeforeFirst = true;
            }
        }
    }
}
