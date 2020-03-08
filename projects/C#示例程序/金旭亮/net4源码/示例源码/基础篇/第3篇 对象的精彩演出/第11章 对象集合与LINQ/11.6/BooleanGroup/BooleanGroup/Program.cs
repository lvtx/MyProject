using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooleanGroup
{

    public class Student
    {
        public string Name { get; set; } //姓名
        public List<int> Scores;//成绩
    }

    class Program
    {
        /// <summary>
        /// 创建示例数据
        /// </summary>
        /// <returns></returns>
        public static List<Student> GetStudents()
        {

            List<Student> students = new List<Student>
        {
           new Student {Name="张三", Scores= new List<int> {97, 72, 81, 60}},
           new Student {Name="李四",  Scores= new List<int> {75, 84, 91, 39}},
           new Student {Name="王五", Scores= new List<int> {99, 89, 91, 95}},
           new Student {Name="赵六", Scores= new List<int> {72, 81, 65, 84}},
           new Student {Name="马七", Scores= new List<int> {97, 89, 85, 82}} 
        };

            return students;

        }
        /// <summary>
        /// 判断成绩单中是否有“挂红灯”的课程
        /// </summary>
        /// <param name="Scores"></param>
        /// <returns></returns>
        public static bool HasFailed(List<int> Scores)
        {
            foreach (int score in Scores)
            {
                if (score < 60)
                    return true;
            }
            return false;
        }

        static void Main(string[] args)
        {

            List<Student> students = GetStudents();

            // 按true或false分组
            // 查询结果类型为：IEnumerable<IGrouping<bool, Student>>
            var booleanGroupQuery =
                from student in students
                group student by HasFailed(student.Scores);

            //输出查询结果
            foreach (var studentGroup in booleanGroupQuery)
            {
                Console.WriteLine();
                Console.WriteLine(studentGroup.Key == true ? "有不及格课程的学生" : "成绩优秀的学生");
                foreach (var student in studentGroup)
                {
                    Console.WriteLine("   {0}，平均分：{1}", student.Name, student.Scores.Average());
                }
            }


            Console.ReadKey();

        }
    }
}
