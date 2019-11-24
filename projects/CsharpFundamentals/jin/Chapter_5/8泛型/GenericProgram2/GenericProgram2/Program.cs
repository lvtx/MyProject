using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericProgram2
{
    class Program
    {
        static void Main(string[] args)
        {
            MyStack<int> intStack = new MyStack<int>();
            MyStack<string> stringStack = new MyStack<string>();
            intStack.Push(10);
            intStack.Push(6);
            intStack.Push(7);
            intStack.Push(5);
            intStack.Print();
            Console.Read();
        }
    }
    class MyStack<T>
    {
        T[] StackArray;
        const int MaxStack = 10;
        int i = 0;//索引
        public bool IsStackFull
        {
            get { return i >= MaxStack; }
        }
        public bool IsStackEmpty
        {
            get { return i <= 0; }
        }
        public MyStack()
        {
            StackArray = new T[10];
        }
        public void Push(T item)
        {
            if (!IsStackFull)
            {
                StackArray[i++] = item;
            }
        }
        public T Pop()
        {
            if (!IsStackEmpty)
            {
                return StackArray[--i];
            }
            else
                return StackArray[0];
        }
        public void Print()
        {
            for (int i = this.i - 1; i >= 0; i--)
            {
                Console.WriteLine("StackArray[{0}] = {1}",i,StackArray[i]);
            }
        }
    }
}
