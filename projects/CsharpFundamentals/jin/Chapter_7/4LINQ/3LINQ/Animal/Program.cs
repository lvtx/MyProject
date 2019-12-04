using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal
{
    class Program
    {
        /*编写一个Person类，其中有一个年龄属性。创建一个
          Person对象的集合，然后按照年龄分为五组：老年、中年、
          青年、少年和儿童，然后分组输出。*/
        static void Main(string[] args)
        {
            
            try
            {   
                var peopleGroup = from person in GetPeople()
                                  group person by SortOfPeople(person);                                
                foreach (var people in peopleGroup)
                {
                    Console.WriteLine("============={0}================",people.Key);
                    foreach (var person in people)
                    {
                        Console.Write("{0}, ",person.Age);
                        Console.WriteLine();
                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {

                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
        static List<Person> GetPeople()
        {
            List<Person> people = new List<Person>()
            {
                new Person{ Age = 80},
                new Person{ Age = 10},
                new Person{ Age = 30},
                new Person{ Age = 50},
                new Person{ Age = 40},
                new Person{ Age = 60},
                new Person{ Age = 70},
                new Person{ Age = 20},
                new Person{ Age = 90},

            };
            return people;
        }
        static string SortOfPeople(Person person)
        {
            /*0（初生）-6岁为婴幼儿；7-12岁为少儿；
              13-17岁为青少年；18-45岁为青年；
              46-69岁为中年；>69岁为老年。*/
            if (person.Age > 6 && person.Age < 12)
            {
                return "儿童";
            }
            if (person.Age < 17)
            {
                return "少年";
            }
            if (person.Age < 45)
            {
                return "青年";
            }
            if (person.Age < 69)
            {
                return "中年";
            }
            else
                return "老年";
        }
    }
    class Person
    {
        public int MyProperty { get; set; }
        private int age;
        public int Age
        {
            get { return age; }
            set
            {
                if (value > 120 || value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                  $"{nameof(age)} 大于 0 小于 120.");
                };
                age = value;
            }
        }
    }
}
