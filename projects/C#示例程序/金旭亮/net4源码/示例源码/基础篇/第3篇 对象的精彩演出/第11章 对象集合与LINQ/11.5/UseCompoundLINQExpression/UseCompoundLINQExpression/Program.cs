using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseCompoundLINQExpression
{
    public class Student
    {
        public string Name { get; set; }
        public List<int> Scores { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Student> students = new List<Student>
            {
               new Student {Name="张三", Scores= new List<int> {97, 92, 81, 60}},
               new Student {Name="李四", Scores= new List<int> {75, 84, 91, 39}},
               new Student {Name="王五", Scores= new List<int> {88, 94, 65, 85}},
               new Student {Name="赵六", Scores= new List<int> {97, 89, 85, 82}},
               new Student {Name="马七", Scores= new List<int> {35, 72, 91, 70}} 
            };

          //查找任一单科成绩上90的学生信息，注意返回的结果集是如何生成的
            var scoreQuery = from student in students
                             from score in student.Scores
                             where score > 90
                             select new { student.Name, score=student.Scores.Average() };

            
            Console.WriteLine("至少有一门单科成绩在90分以上的学生查询结果:");
            foreach (var student in scoreQuery)
            {
                Console.WriteLine("{0} 平均分: {1}", student.Name, student.score);
            }

            Console.ReadKey();

        }
        
    }
}
