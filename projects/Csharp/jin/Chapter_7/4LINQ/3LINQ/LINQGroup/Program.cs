using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQGroup
{
    public class Student
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 考试成绩
        /// </summary>
        public List<int> Scores { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //IntroduceGroup();
            //UseGroupInto();
            //TestBooleanGroup();
            TestMultiGroup();
            Console.ReadLine();
        }
        #region "1.认识分组"
        static void IntroduceGroup()
        {
            List<Student> students = new List<Student>
        {
            new Student {Name="张三", City="北京"},
            new Student {Name="李四", City="上海"},
            new Student {Name="王五", City="北京"},
            new Student {Name="赵六", City="重庆"},
            new Student {Name="马七", City="北京"},
            new Student {Name="牛八", City="上海"}
        };
            var studentQuery = from student in students
                               group student by student.City;
            foreach (var studentGroup in studentQuery)
            {
                Console.WriteLine("============来自{0}的学生=============", studentGroup.Key);
                foreach (var student in studentGroup)
                {
                    Console.WriteLine("{0}", student.Name);
                }
            }
        }
        #endregion

        #region "2.分组后进一步处理"
        static void UseGroupInto()
        {
            //一个单词数组作为数据源
            string[] words = { "blueberry", "chimpanzee",
                                 "abacus", "banana",
                                 "apple",     "cheese",
                                 "elephant", "umbrella",
                                 "anteater" };
            var wordGroups = from w in words
                             orderby w[0]
                             group w by w[0] into grop
                             where (grop.Key != 'a')
                             select grop;

            foreach (var wordGroup in wordGroups)
            {
                Console.WriteLine("============以{0}开头的单词有============", wordGroup.Key);
                foreach (var word in wordGroup)
                {
                    Console.WriteLine(word);
                }
            }
        }
        #endregion

        #region "3.“非此即彼”的分组"
        static void TestBooleanGroup()
        {
            var StudentGroup = from student in GetStudents()
                               group student by HasFailed(student.Scores);
            foreach (var students in StudentGroup)
            {
                Console.WriteLine("================={0}================",
                    students.Key == true ? "有不及格课程":"及格");
                foreach(var studnet in students)
                {
                    Console.WriteLine("{0}平均成绩:{1}",studnet.Name,studnet.Scores.Average());
                }
            }
        }
        /// <summary>
        /// 判断成绩单中是否有“挂红灯”的课程
        /// </summary>
        /// <param name="Scores"></param>
        /// <returns></returns>
        static bool HasFailed(List<int> Scores)
        {
            foreach (var score in Scores)
            {
                if (score < 60)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region "4.多段分组"
        static void TestMultiGroup()
        {
            var studentGroup = from student in GetStudents()
                               group student by GroupKey(student.Scores);
            foreach (var students in studentGroup)
            {
                Console.WriteLine("==============成绩{0}的学生==============",students.Key);
                foreach (var student in students)
                {
                    Console.WriteLine("{0}的成绩:{1}",student.Name,student.Scores.Average());
                }
            }

        }

        public static string GroupKey(List<int> Scores)
        {
            //计算平均分，并取整
            int avg = (int)Scores.Average();

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
        #endregion
        static List<Student> GetStudents()
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
    }
}
