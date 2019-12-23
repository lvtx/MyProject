using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAssociation
{
    class Program
    {
        static void Main(string[] args)
        {
            //FetchFirstPerson();

            FetchFirstPersonUseInclude();

            Console.ReadKey();
        }
        static void FetchFirstPerson()
        {
            using (var context = new MyDBContext())
            {
                context.Database.Log = Console.WriteLine;
                Person person = context.People.FirstOrDefault();
                if (person != null)
                {
                    if (person.IdentityCard != null)
                    {
                        Console.WriteLine("{0}身份证号:{1}",person.Name,person.IdentityCard.IDNumber);
                    }
                    else
                    {
                        Console.WriteLine("姓名:{0},身份证号:无可用身份证号",person.Name);
                    }
                }
            }
        }
        static void FetchFirstPersonUseInclude()
        {
            using (var context = new MyDBContext())
            {
                context.Database.Log = Console.WriteLine;
                Person person = context.People.
                    Include("IdentityCard").FirstOrDefault();
                if (person != null)
                {
                    if (person.IdentityCard != null)
                    {
                        Console.WriteLine("{0}:身份证号{1}",
                            person.Name,person.IdentityCard);
                    }
                    else
                    {
                        Console.WriteLine("姓名：{0} 身份证号:无可用信息",
                            person.Name);
                    }
                }
            }
        }
    }
}
    
