using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiGroup
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
                new Student {Name="张三", Scores= new List<int> {97, 93, 92, 90}},
                new Student {Name="李四",  Scores= new List<int> {75, 84, 91, 39}},
                new Student {Name="王五", Scores= new List<int> {99, 89, 91, 95}},
                new Student {Name="赵六", Scores= new List<int> {72, 45, 65, 62}},
                new Student {Name="马七", Scores= new List<int> {97, 89, 85, 82}} ,
                new Student {Name="牛八", Scores= new List<int> {30, 50, 67, 71}} ,
            };
            return students;
        }

        public static string GroupKey(List<int> Scores)
        {
            int avg = (int)Scores.Average(); //计算平均分，并取整
           
            string ret = "";
            switch (avg / 10)  //整除以10
            {
                case 10:
                case 9:
                    ret = "优";
                    break;
                case 8:
                    ret = "良";
                    break;
                case 7:
                    ret = "中";
                    break;
                case 6:
                    ret = "及格";
                    break;
                default:
                    ret = "不及格";
                    break;
            }
            return ret;




        }
        static void Main(string[] args)
        {
            List<Student> students = GetStudents();

        
            // 查询结果类型为：IEnumerable<IGrouping<sgring, Student>
            var booleanGroupQuery =
                from student in students
                group student by GroupKey(student.Scores);

            //输出查询结果
            foreach (var studentGroup in booleanGroupQuery)
            {
                Console.WriteLine();
                Console.WriteLine(studentGroup.Key+"：");
                foreach (var student in studentGroup)
                {
                    Console.WriteLine("   {0}，平均分：{1}", student.Name, student.Scores.Average());
                }
            }


            Console.ReadKey();

        }
    }
}
