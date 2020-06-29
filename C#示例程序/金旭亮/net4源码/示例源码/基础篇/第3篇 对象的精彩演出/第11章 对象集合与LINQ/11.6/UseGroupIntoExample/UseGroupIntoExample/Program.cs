using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseGroupIntoExample1
{
    class Program
    {
        static void Main(string[] args)
        {
            //一个单词数组作为数据源
            string[] words = { "blueberry", "chimpanzee", "abacus", "banana", "apple", "cheese", "elephant", "umbrella", "anteater" };

            // 按5个原音字母a、e、i、o、u将单词分组
            var wordGroups =
                from w in words
                group w by w[0] into grps
                where (grps.Key == 'a' || grps.Key == 'e' || grps.Key == 'i'
                       || grps.Key == 'o' || grps.Key == 'u')
                select grps;

            // Execute the query.
            foreach (var wordGroup in wordGroups)
            {
                Console.WriteLine("以原音字母“{0}”开头的单词有：", wordGroup.Key);
                foreach (var word in wordGroup)
                {
                    Console.WriteLine("   {0}", word);
                }
            }

        
            Console.ReadKey();
        }
    }
}
