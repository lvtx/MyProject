using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassLib
{
    public class StringUtils
    {
        /// <summary>
        /// 反转一个字符串中的所有英文单词数目
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String Reverse(string value)
        {
            var words = Regex.Split(value,@"\s+")
                .Reverse().ToArray();

            return string.Join(" ",words);

        }
    }
}
