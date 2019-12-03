using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event
{
    // 模板方法
    class Program
    {
        static void Main(string[] args)
        {
            ProductFactory productFactory = new ProductFactory();
            WrapFactory wrapFactory = new WrapFactory();
            Func<Product> func1 = new Func<Product>(productFactory.MakePizza);
            Func<Product> func2 = new Func<Product>(productFactory.MakeToyCar);
            Box box1 = wrapFactory.WrapProduct(func1);
            Box box2 = wrapFactory.WrapProduct(func2);
            Console.WriteLine(box1.product.Name);
            Console.WriteLine(box2.product.Name);
            Console.ReadLine();
        }
    }

    public class Product
    {
        public string Name { get; set; }
    }
    public class Box
    {
        public Product product { get; set; }
    }
    public class WrapFactory
    {
        public Box WrapProduct(Func<Product> getProduct)
        {
            Box box = new Box();
            Product product = getProduct.Invoke();
            box.product = product;
            return box;
        }
    }
    public class ProductFactory
    {
        public Product MakePizza()
        {
            Product product = new Product();
            product.Name = "Pizza";
            return product;
        }
        public Product MakeToyCar()
        {
            Product product = new Product();
            product.Name = "ToyCar";
            return product;
        }      
    }
}
