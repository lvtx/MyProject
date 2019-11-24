using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_SingleObjectSerialization
{
    [Serializable]
    class CollegeStudent
    {
        public String Name = "空";
        public bool IsMale = true;
        public int ScoreForEntranceExamination = 0;//入学考试成绩
    }
}
