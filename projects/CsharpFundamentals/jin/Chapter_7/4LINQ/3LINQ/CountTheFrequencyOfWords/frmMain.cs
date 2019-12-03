using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CountTheFrequencyOfWords
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        public int AllCounter { get; set; }
        public Dictionary<string, int> SameCounter { get; set; } = new Dictionary<string, int>();
        private void frmMain_Load(object sender, EventArgs e)
        {
            lblInfo.Text = " ";
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            string myString = txtBox.Text;
            CountDuplicateWords(myString);
            lblInfo.Text = string.Format("文章中单词的总数为{0}\n", AllCounter);
            var wordsFrequency = from word in SameCounter
                                 orderby word.Value descending
                                 where (!string.IsNullOrEmpty(word.Key))
                                 select word;

            foreach (var item in wordsFrequency)
            {
                if (!string.IsNullOrEmpty(item.Key))
                {
                    lblInfo.Text += string.Format("单词{0}出现的频率{1}%\n",
                        item.Key, (item.Value * 100.0) / SameCounter.Count());
                }              
            }
        }

        #region "统计单词个数"
        /// <summary>
        /// 统计单词个数
        /// </summary>
        /// <param name="txt"></param>
        private string[] SplitText(string txt)
        {
            string[] words = txt.Split(',', ' ', '\t', '\n', '\r', '.');
            AllCounter = words.Length;
            //去除多余空格
            foreach (var word in words)
            {
                if (string.IsNullOrEmpty(word))
                    AllCounter--;
            }
            return words;
        }
        #endregion

        #region "统计单词出现频率"
        /// <summary>
        /// 相同单词出现次数
        /// </summary>
        private void CountDuplicateWords(string txt)
        {
            string[] words = SplitText(txt);
            for (int i = 0; i < words.Length; i++)
            {
                if (SameCounter.ContainsKey(words[i]))
                {
                    SameCounter[words[i]]++;
                }
                else
                {
                    SameCounter[words[i]] = 1;
                }
            }
        }
        #endregion
    }
}
