using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImperativeVsDeclarative
{

    public class Student
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    /// <summary>
    /// 存放分组结果
    /// </summary>
    class StudentGroup
    {
        public string City;
        public List<Student> Students = new List<Student>();
    }

    class Program
    {
        /// <summary>
        /// 提取学生清单
        /// </summary>
        /// <returns></returns>
        static IEnumerable<Student> GetStudents()
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
            return students;
        }
        /// <summary>
        /// 按城市分组
        /// </summary>
        static void GroupStudentByCity()
        {
            var students = GetStudents();
            var results = new List<StudentGroup>();

            foreach (Student student in students)
            {
                StudentGroup res = results.Find(item => item.City == student.City);
                if (res != null)
                    res.Students.Add(student);
                else
                {
                    StudentGroup newRes = new StudentGroup() { City = student.City };
                    newRes.Students.Add(student);
                    results.Add(newRes);
                }
            }

            PrintResult(results);
        }

        /// <summary>
        /// 输出结果
        /// </summary>
        /// <param name="Results"></param>
        static void PrintResult(IEnumerable<StudentGroup> Results)
        {

            foreach (StudentGroup res in Results)
            {
                Console.WriteLine("============================");
                Console.WriteLine("{0}的学生有{1}人：", res.City, res.Students.Count);
                foreach (Student stu in res.Students)
                    Console.WriteLine(stu.Name);
                Console.WriteLine();
            }

        }

        /// <summary>
        /// 对LINQ组进行处理
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        static StudentGroup ProcessGroup(IGrouping<string, Student> group)
        {
            StudentGroup result = new StudentGroup { City = group.Key };
            foreach (Student stu in group)
                result.Students.Add(stu);
            return result;
        }

        static void GroupStudentByCityUseLINQ()
        {
            var results = from student in GetStudents()
                          group student by student.City into grp
                          select ProcessGroup(grp);

            PrintResult(results);


        }

        static void Main(string[] args)
        {
            Console.WriteLine("使用命令式编程方式实现：\n");
            GroupStudentByCity();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n使用声明式编程方式实现：\n");
            GroupStudentByCityUseLINQ();
            Console.ReadKey();
        }
    }
}
