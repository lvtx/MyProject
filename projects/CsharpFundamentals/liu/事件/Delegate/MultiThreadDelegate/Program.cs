using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadDelegate
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Student student1 = new Student() { ID = 1, PenColor = ConsoleColor.Red };
            Student student2 = new Student() { ID = 2, PenColor = ConsoleColor.Green };
            Student student3 = new Student() { ID = 3, PenColor = ConsoleColor.Cyan };
            #region "同步调用"
            //直接调用
            //student1.DoHomeWork();
            //student2.DoHomeWork();
            //student3.DoHomeWork();
            ////单播委托的间接调用
            //MyClass myClass = new MyClass();           
            //myClass.UseSinglecastDelegate();
            ////多播委托的间接调用
            //myClass.UseMulticastDelegate();
            #endregion

            #region "异步调用"
            Action action1 = new Action(student1.DoHomeWork);
            Action action2 = new Action(student2.DoHomeWork);
            Action action3 = new Action(student3.DoHomeWork);

            action1.BeginInvoke(null, null);
            action2.BeginInvoke(null, null);
            action3.BeginInvoke(null, null);
            #endregion

            for (int i = 0; i < 10; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("主线程{0}", i);
                Thread.Sleep(1000);
            }

            Console.WriteLine("按任意键结束");
            Console.ReadLine();
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
        //多播委托
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
        }
    }
    class Student
    {
        public int ID { get; set; }
        public ConsoleColor PenColor { get; set; }
        public void DoHomeWork()
        {            
            for (int i = 0; i < 5; i++)
            {
                Console.ForegroundColor = this.PenColor;
                Console.WriteLine("Student {0} doing homework {1} hours", this.ID, i);
                Thread.Sleep(100);
            }
        }
    }
}
