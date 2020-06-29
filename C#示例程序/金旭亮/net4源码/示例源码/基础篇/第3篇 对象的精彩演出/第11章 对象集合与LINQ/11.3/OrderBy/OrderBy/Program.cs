using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderBy
{
    class Pet
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Pet[] pets = { new Pet { Name="Barley", Age=8 },
                   new Pet { Name="Boots", Age=4 },
                   new Pet { Name="Whiskers", Age=1 } };

            IEnumerable<Pet> query = pets.OrderBy(pet => pet.Age);

            foreach (Pet pet in query)
                Console.WriteLine("{0} - {1}", pet.Name, pet.Age);

            Console.ReadKey();

        }
    }
}
