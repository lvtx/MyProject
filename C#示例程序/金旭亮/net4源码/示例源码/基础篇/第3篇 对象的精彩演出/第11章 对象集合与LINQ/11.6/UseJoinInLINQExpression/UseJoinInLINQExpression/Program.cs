using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseJoinInLINQExpression
{
    /// <summary>
    /// 员工类
    /// </summary>
    class Person
    {

        public int ID
        {
            get;
            set;
        }
        public int IDRole
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }

    }
    /// <summary>
    /// 岗位角色
    /// </summary>
    class Role
    {

        public int ID
        {
            get;
            set;
        }
        public string RoleDescription
        {
            get;
            set;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //四个员工
            List<Person> people = new List<Person> {
                new Person { ID = 1,IDRole = 1,Name="张三"},
                new Person { ID = 2,IDRole = 2,Name="李四"},
                new Person { ID = 3,IDRole = 2,Name = "王五"},
                new Person { ID = 4,IDRole = 3,Name="赵六"}
            };
            //两种岗位
            List<Role> roles = new List<Role> {
                new Role { ID = 1, RoleDescription = "项目经理" },
                new Role { ID = 2, RoleDescription = "程序员" }
            };
            //内连接：查找指定员工所属的岗位
            Console.WriteLine("内连接:");

            var query = from p in people   //指定数据左集合为people
                        join r in roles  //指定数据右集合为roles
                        on p.IDRole equals r.ID  //指定左右集合元素的匹配属性
                        select new
                        {
                            p.Name,
                            r.RoleDescription
                        };

            foreach (var p in query)
            {
                Console.WriteLine("{0},{1}", p.Name, p.RoleDescription);
            }


            //分组连接：按岗位分组显示员工信息
            Console.WriteLine("\n===================");
            Console.WriteLine("分组连接:");
            var query2 = from r in roles        //指定数据左集合为roles
                         join p in people    //指定数据右集合为people
                        on r.ID equals p.IDRole //指定左右集合元素的匹配属性
                        into pGroup  //当有into子句时，查询结果会按照左集合数据中的关联属性值进行分组，
                         //pGroup代表一个分组，注意这也是一个数据对象的集合，类型为IEnumerable<People>
                         //注意，此处的分组，其类型与group子句生成的分组类型是不一样的，后者类型为IGrouping<TKey, TElement>
                         select new
                         {
                             r.RoleDescription,
                             stuff = pGroup           //将整个集合作为结果返回
                         };

            foreach (var item in query2)
            {
                Console.Write(item.RoleDescription + ":");
                foreach (var p in item.stuff)
                {
                    Console.Write(p.Name + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n===================");

            // 左外部连接：结果包含所有的左集合元素
            //如果右集合中没有与左集合对应的元素，则临时指定一个”新的对应元素"
            Console.WriteLine("\n左外部连接:");

            var query3 = from p in people  //指定数据左集合为people
                         join r in roles //指定数据右集合为roles

                          on p.IDRole equals r.ID //指定左右集合元素的匹配属性

                          into pr  //将分组保存到临时变量中,IEnumerable<Role>类型

                         from r in pr.DefaultIfEmpty(//如果pr为空，则创建一个临时对象Role，并将其转换为一个IEnumable<Role>返回
                             //如果pr不为空，直接返回pr
                         new Role { ID = 3, RoleDescription = "临时工" })
                         select new
                         {
                             p.Name,
                             r.RoleDescription
                         };

            foreach (var p in query3)
            {
                Console.WriteLine("{0},{1}", p.Name, p.RoleDescription);
            }
            Console.ReadKey();
        }
    }
}
