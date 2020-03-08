using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseGroup
{
    public class Student
    {
        public string Name { get; set; }
        public string City{ get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Student> students = new List<Student>
            {
               new Student {Name="张三", City="北京"},
               new Student {Name="李四", City="上海"},
               new Student {Name="王五", City="北京"},
               new Student {Name="赵六", City="重庆"},
               new Student {Name="马七", City="北京"} ,
                new Student {Name="牛八", City="上海"}
            };

            // studnetQuery类型为：IEnumerable<IGrouping<string, Student>>

            var studnetQuery = from student in students
                               group student by student.City;

            //studentGroup类型为：IGrouping<string, Student>
            foreach (var studentGroup in studnetQuery)
            {

                Console.WriteLine("\n=====================");
                Console.WriteLine("在{0}的学生清单：",studentGroup.Key);
                Console.WriteLine("=====================");

                int count = 0;
                foreach (Student stu in studentGroup)
                {
                    count++;
                    Console.WriteLine("{0}:{1}({2})",count, stu.Name, stu.City);
                }
            }

            Console.ReadKey();
                                 
         


        }
    }
}
