using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        public static int AllCounter { get; set; }
        static void Main(string[] args)
        {
            string text1 = Console.ReadLine();
            string text2 = Console.ReadLine();
            string text = text1 + text2;
            string[] words = SplitText(text);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("单词总数为:{0}",AllCounter);
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var word in words)
            {
                Console.WriteLine("============{0}===========",word);
                foreach (var letter in word)
                {
                    Console.Write("{0} ",(char)letter);
                }
                foreach (var letter in word)
                {
                    Console.Write("{0} ", (int)letter);
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        //统计单词个数
        private static string[] SplitText(string txt)
        {
            string[] words = txt.Split(',', ' ', '\t', '\n', '\r', '.');
            AllCounter = words.Length;
            //去除多余空格
            foreach (var word in words)
            {
                if (string.IsNullOrEmpty(word))
                    AllCounter--;
            }
            ////解决结尾标点问题
            //if (!char.IsLetter(txt[txt.Length - 1]))
            //    AllCounter--;
            return words;
        }
    }
}