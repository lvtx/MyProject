using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseCustomizeClass
{
    public class Student
    {
        public string Name
        {
            get;
            set;
        }
        public bool IsFemale
        {
            get;
            set;
        }

        public override string ToString()
        {
            if (IsFemale)
                return Name + ":女";
            else
                return Name + ":男";
        }

        public static string StaticInformation = "我是Student类型所定义的静态字段！";
    }
}
