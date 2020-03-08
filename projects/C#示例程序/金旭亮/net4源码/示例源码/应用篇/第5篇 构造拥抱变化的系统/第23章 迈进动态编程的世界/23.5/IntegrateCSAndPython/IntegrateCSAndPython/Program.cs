using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Dynamic;


namespace IntegrateCSAndPython
{
    class Program
    {
        static void Main(string[] args)
        {
            //C#使用IronPython文件中的对象
            ScriptRuntime pythonRuntime = Python.CreateRuntime();
            dynamic pythonFile = pythonRuntime.UseFile("Calculator.py");
            dynamic calc = pythonFile.GetCalculator();
            Console.WriteLine("100+200={0}", calc.Add(100, 200));
            Console.WriteLine("{0} + {1} = \"{2}\"", "Hello", "IronPython", calc.Add("Hello", " IronPython"));
            Console.WriteLine("明天是{0}", calc.Add(DateTime.Today, TimeSpan.FromDays(1)).ToLongDateString());
            Console.ReadKey();

            //C#调用Python引擎执行Python语句
            ScriptEngine engine = pythonRuntime.GetEngine("py");
            ScriptSource source = engine.CreateScriptSourceFromString("dir()");
            dynamic result = source.Execute();
            foreach (var item in result)
                Console.WriteLine(item);
            Console.ReadKey();

            //python调用C#方法
            ScriptScope scope = pythonRuntime.CreateScope();
            scope.SetVariable("InvokeCSMethod", (Func<string, string>)TMethod);
            source = engine.CreateScriptSourceFromString("InvokeCSMethod('From Python')");
            Console.WriteLine(source.Execute<string>(scope));
            Console.ReadKey();

            //python调用对象方法
            scope.SetVariable("CSObject", new MyClass());
            source = engine.CreateScriptSourceFromString("CSObject.SaySomething('Hello')");
            source.Execute(scope);
            
            //python接收C#的Expando对象
            dynamic expando = new ExpandoObject();
            expando.Name = "JinXuLiang";

            scope.SetVariable("ExpandoObject", expando);
            string pythonCode = "print ExpandoObject.Name";
            engine.CreateScriptSourceFromString(pythonCode).Execute(scope);

            Action<string> act = delegate(string str)
            {
                Console.WriteLine(str);
            };
            expando.Fun = act;
            pythonCode = "ExpandoObject.Fun('Test')";
            engine.CreateScriptSourceFromString(pythonCode).Execute(scope);
            Console.ReadKey();

            //python实例化.NET对象，注意每行以\n分隔，不要随意添加空格
            pythonCode = "from System.Collections import *\nh=Hashtable()\ndir(h)";

            result = engine.CreateScriptSourceFromString(pythonCode).Execute(scope);
            foreach (var item in result)
                Console.WriteLine(item);
            Console.ReadKey();

        }

        public static string TMethod(string info)
        {
            return "Hello:" + info;
        }

    }

    public class MyClass
    {
        public void SaySomething(string info)
        {
            Console.WriteLine(info);
        }
    }
}
