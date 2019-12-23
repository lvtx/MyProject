using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MulticastDelegate
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass myClass = new MyClass();
            myClass.UseSinglecastDelegate();
        }
    }
    class MyClass
    {
        //单播委托
        public void UseSinglecastDelegate()
        {
            Student student1 = new Student() { ID = 1, PenColor = ConsoleColor.Red };
            Student student2 = new Student() { ID = 2, PenColor = ConsoleColor.Green };
            Student student3 = new Student() { ID = 3, PenColor = ConsoleColor.Cyan };
            Action action1 = new Action(student1.DoHomeWork);
            Action action2 = new Action(student2.DoHomeWork);
            Action action3 = new Action(student3.DoHomeWork);
            action1.Invoke();
            action2.Invoke();
            action3.Invoke();
        }
        public void UseMulticastDelegate()
        {
            Student student1 = new Student() { ID = 1, PenColor = ConsoleColor.Red };
            Student student2 = new Student() { ID = 2, PenColor = ConsoleColor.Green };
            Student student3 = new Student() { ID = 3, PenColor = ConsoleColor.Cyan };
            Action action1 = new Action(student1.DoHomeWork);
            Action action2 = new Action(student2.DoHomeWork);
            Action action3 = new Action(student3.DoHomeWork);
            action1 += action2;
            action1 += action3;

            action1.Invoke();
            Console.ReadLine();
        }
    }
    class Student
    {
        public int ID { get; set; }
        public ConsoleColor PenColor { get; set; }
        public void DoHomeWork()
        {
            Console.ForegroundColor = this.PenColor;
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Student {0} doing homework {1} hours", this.ID, i);
                Thread.Sleep(1000);
            }
        }
    }
}
