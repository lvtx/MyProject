using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("123".StringToValue().GetType());
            Console.WriteLine("是否为身份证:{0}","371083199612131017".IsIDcard());
            Console.WriteLine("是否为邮政编码:{0}","123456".IsPostalcode());
            Console.ReadLine();
        }
    }
    static class MyConvert
    {
        public static int StringToValue(this string str)
        {
            return Convert.ToInt32(str);
        }
        public static bool IsIDcard(this string str_idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{17}(?:\d|x)$)|(^\d{15}$)");
        }
        public static bool IsPostalcode(this string str_postalcode)

        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d{6}$");
        }
    }
}

