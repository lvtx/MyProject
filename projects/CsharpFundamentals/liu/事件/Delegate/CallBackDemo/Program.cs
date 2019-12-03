using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event
{
    // 回调
    class Program
    {
        static void Main(string[] args)
        {
            ProductFactory productFactory = new ProductFactory();
            Logger logger = new Logger();
            WrapFactory wrapFactory = new WrapFactory();
            Func<Product> func1 = new Func<Product>(productFactory.MakePizza);
            Func<Product> func2 = new Func<Product>(productFactory.MakeToyCar);
            Action<Product> log = new Action<Product>(logger.Log);
            Box box1 = wrapFactory.WrapProduct(func1,log);
            Box box2 = wrapFactory.WrapProduct(func2,log);
            Console.WriteLine(box1.product.Name);
            Console.WriteLine(box2.product.Name);
            Console.ReadLine();
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }
    /// <summary>
    /// 产品记录
    /// </summary>
    public class Logger
    {
        public void Log(Product product)
        {
            Console.WriteLine("Product {0} create at {1} piece is {2}"
                ,product.Name,DateTime.UtcNow,product.Price);
        }
    }
    public class Box
    {
        public Product product { get; set; }
    }
    /// <summary>
    /// 模板方法
    /// </summary>
    public class WrapFactory
    {
        public Box WrapProduct(Func<Product> getProduct,Action<Product> logCallback)
        {
            Box box = new Box();
            Product product = getProduct.Invoke();
            //回调
            if (product.Price > 15)
            {
                logCallback.Invoke(product);
            }
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
            product.Price = 16;
            return product;
        }
        public Product MakeToyCar()
        {
            Product product = new Product();
            product.Name = "ToyCar";
            product.Price = 14;
            return product;
        }
    }
}

