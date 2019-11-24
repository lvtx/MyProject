using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Array
{
    //CHS是simplified Chinese的缩写
    //将一个整数转换为汉字读法字符串。
    //比如“1123”转换为“一千一百二十三”。
    class NumConvertToCHS
    {
        IsZeroStruct zero;   
        private static string userInput;
        //简体中文的数位
        private string[] chsDigit = { "零", "零", "十", "百", "千", "万", "亿" };
        private string[] chsNumber1To9 = { "零", "一", "二", "三",
            "四", "五", "六", "七", "八", "九" };
        private string chsNumber = "";
        public void Initialization()
        {
            Zero.one = false;
            Zero.ten = false;
            Zero.hundred = false;
            Zero.thousand = false;
        }
        public static void UserInput()
        {
            Console.WriteLine("请输入一个数字");
            userInput = Console.ReadLine();
        }
        public static void ConsoleOutput()
        {
            int thisnum = userInput[1] - '0';
            Console.WriteLine(thisnum);
        }
        public string ConvertNum()//将数字转换为简体中文
        {
            int thisnum = 0;
            string tempChsNumber = "";
            //int count = 0;
            for (int i = 0; i < userInput.Length; i++)
            {
                thisnum = userInput[i] - '0';
                tempChsNumber += MappingDigit(i + 1, thisnum);
                if(i != 0)//处理个位时不用执行重复
                    tempChsNumber += chsNumber1To9[thisnum];
            }


            return chsNumber;
        }
        private string MappingDigit(int count, int thisnum)//对应好自己的数位
        {
            if (count == 1)//处理个位
            {
                if (thisnum == 0)//当个位为0时
                {
                    zero.one = true;
                    return "";
                }    
                else//不为0
                    return chsNumber1To9[thisnum];
            }
            if(count == 2)//处理十位
            {
                if (thisnum == 0)
                {
                    if(zero.one == false)//当十位为0个位不为0，例1n01
                    {
                        zero.ten = true;
                        return chsDigit[0];
                    }
                    else//当十位为0个位为0，例1n00
                    {
                        zero.ten = true;
                        return "";
                    }
                }
                //当十位不为0个位为随意，例1n10，1n11
                else
                    return chsDigit[count];             
            }
            /*--------------百位分割线---------------*/
            if(count == 3)//处理百位
            {
                if (thisnum == 0)//百位为0
                {
                    if(zero.ten == true)//十位为0
                    {
                        if (zero.one == true)//个位为0，例1000
                            return "";
                        else//个位不为0，例如1001
                        {
                            return chsDigit[0];
                        }
                    }
                    else//十位不为0，例1010，1011
                    {
                        return chsDigit[0];
                    }
                }
                else//百位不为0，例11nm
                {
                    if(zero.ten == true)//十位为0
                    {
                        if(zero.one == true)//个位为0，例1100
                        {
                            return chsDigit[3];
                        }
                    }
                }
            }
        }
    }
}
