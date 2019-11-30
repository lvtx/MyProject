using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQGroupExamples
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
            Console.ReadKey();
        }
        #region "认识分组"
        static void IntroduceGroup()
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
                Console.WriteLine("在{0}的学生清单：", studentGroup.Key);
                Console.WriteLine("=====================");

                int count = 0;
                foreach (Student stu in studentGroup)
                {
                    count++;
                    Console.WriteLine("{0}:{1}({2})", count,
                        stu.Name, stu.City);
                }
            }
        }

        #endregion

        #region "分组后进一步处理"

        static void UseGroupInto()
        {
            //一个单词数组作为数据源
            string[] words = { "blueberry", "chimpanzee", 
                                 "abacus", "banana", 
                                 "apple",     "cheese", 
                                 "elephant", "umbrella", 
                                 "anteater" };

            // 按5个原音字母a、e、i、o、u将单词分组
            var wordGroups =
                from w in words
                group w by w[0] into grps
                where (grps.Key == 'a' || grps.Key == 'e'
                    || grps.Key == 'i' || grps.Key == 'o' || grps.Key == 'u')
                select grps;

            // 执行查询
            foreach (var wordGroup in wordGroups)
            {
                Console.WriteLine("以原音字母“{0}”开头的单词有：", wordGroup.Key);
                foreach (var word in wordGroup)
                {
                    Console.WriteLine("   {0}", word);
                }
            }
        }
        #endregion

        #region "“非此即彼”的分组"
        static void TestBooleanGroup()
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
                Console.WriteLine(studentGroup.Key ==
                    true ? "有不及格课程的学生" : "成绩优秀的学生");
                foreach (var student in studentGroup)
                {
                    Console.WriteLine("   {0}，平均分：{1}",
                        student.Name, student.Scores.Average());
                }
            }

        }
        /// <summary>
        /// 创建示例数据
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 判断成绩单中是否有“挂红灯”的课程
        /// </summary>
        /// <param name="Scores"></param>
        /// <returns></returns>
        static bool HasFailed(List<int> Scores)
        {
            foreach (int score in Scores)
            {
                if (score < 60)
                    return true;
            }
            return false;
        }
        #endregion

        #region "多段分组"

        static void TestMultiGroup()
        {
            List<Student> students = GetStudents();


            // 查询结果类型为：IEnumerable<IGrouping<string, Student>
            var booleanGroupQuery =
                from student in students
                group student by GroupKey(student.Scores);

            //输出查询结果
            foreach (var studentGroup in booleanGroupQuery)
            {
                Console.WriteLine();
                Console.WriteLine(studentGroup.Key + "：");
                foreach (var student in studentGroup)
                {
                    Console.WriteLine("   {0}，平均分：{1}", student.Name, student.Scores.Average());
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
    }


}
