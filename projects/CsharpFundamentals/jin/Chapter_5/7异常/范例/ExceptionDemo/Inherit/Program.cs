using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inherit
{
    class Program
    {
        static void Main(string[] args)
        {
            Apple apple = new Apple();
            apple.ShowColour();
            Console.WriteLine(apple.myColour);
            Fruit fruit = new Fruit("yellow");
            fruit.ShowColour();
            Console.ReadKey();
        }
        public class Fruit
        {
            public string myColour;
            public Fruit(string colour)
            {
                myColour = colour;
            }   
            public virtual void ShowColour()
            {
                Console.WriteLine(myColour);
            }
        }
        public class Apple:Fruit
        {
            //public string appleColour;
            public Apple(string colour):base(colour)
            {
                
            }
            public Apple():base("this is red")
            {

            }
            public override void ShowColour()
            {
                Console.WriteLine(myColour);
            }
        }
    }
}
