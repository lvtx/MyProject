using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsParallelAndAsSequential
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Student> students = GetStudents();
            Console.WriteLine("全并行版本\n");
            int counter = 0;//计数器

            //去掉下面的AsParallel()，才能得到正确的结果
            var query =
                from student in students.AsParallel()
                where student.Score > 60              //分数大于60
                orderby student.Score descending      //按成绩降序排序
                select new                            //返回学生信息
                {
                    TempID = ++counter,    //名次
                    student.Name,
                    student.Score
                };
            //输出处理结果
            foreach (var item in query)
            {
                Console.WriteLine("{0}: 姓名=\"{1}\" 成绩={2}", item.TempID, item.Name, item.Score);
            }

            Console.WriteLine("\n===================================\n");

            Console.WriteLine("敲任意键运行并行/串行混合版本\n");
            Console.ReadKey(true);
            counter = 0;//复原计数器
            //修正版，直接使用扩展方法而非PLINQ查询语句
            var query2 = students.AsParallel()        //使用并行查询
                .Where(student => student.Score > 60) //分数大于60
                .OrderByDescending(stu => stu.Score)  //按成绩降序排序
                .AsSequential()                       //强制转换为串行执行
                .Select(studentInfo =>
                new
                {
                    TempID = ++counter,  //名次
                    studentInfo.Name,
                    studentInfo.Score
                });
            //输出处理结果
            foreach (var item in query2)
            {
                Console.WriteLine("{0}: 姓名=\"{1}\" 成绩={2}", item.TempID, item.Name, item.Score);
            }
            Console.WriteLine("\n敲任意键结束\n");
            Console.ReadKey();
        }

        /// <summary>
        /// 创建示例学生数据
        /// </summary>
        /// <returns></returns>
        static List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            Random ran = new Random();
            for (int i = 0; i < 100; i++)
            {
                students.Add(new Student
                {
                    Name = "学生" + i.ToString(),
                    Score = ran.Next(1, 100)
                });
            }

            return students;
        }


    }

    /// <summary>
    /// 学生信息
    /// </summary>
    class Student
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name;
        /// <summary>
        /// 考试成绩
        /// </summary>
        public int Score;
    }
}
