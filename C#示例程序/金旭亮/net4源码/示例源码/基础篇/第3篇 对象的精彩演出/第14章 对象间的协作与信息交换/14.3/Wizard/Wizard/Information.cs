using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wizard
{

    /// <summary>
    /// 此类用于存放用户通过向导设置的各种信息
    /// </summary>
    public class Information
    {

        // 姓名
        public string Name { get; set; }
        //性别
        public bool IsMale { get; set; }
        //学历
        public string EduBackground { get; set; }
        //编程语言
        public string ProgrameLanguage { get; set; }
    }
}
