using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace DownLoadPageFromWeb
{
    class Program
    {
        /// <summary>
        /// 异步回调函数
        /// </summary>
        /// <param name="ar"></param>
        static void DownloadFinished(IAsyncResult ar)
        {
            try
            {
                DownLoadTask task = ar.AsyncState as DownLoadTask;
            WebResponse response = task.WebRequestObject.EndGetResponse(ar);
                string FileContent="";
                using(StreamReader reader=new StreamReader(response.GetResponseStream(),Encoding.GetEncoding("gb2312")))
                {
                    FileContent = reader.ReadToEnd();
                }

                using (StreamWriter writer = new StreamWriter(new FileStream(task.SaveToFileName, FileMode.Create), Encoding.GetEncoding("gb2312")))
                    {
                        writer.Write(FileContent);
                    }
               
                MessageBox.Show(string.Format("“{0}”下载完成！", task.SaveToFileName));
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        static void Main(string[] args)
        {
            string UserInputUrl = "";
            string FileName = "";
            Console.WriteLine("输入URL启动一个异步下载Web文件任务,输入quit退出。");
            do
            {

                Console.Write("\n输入Web文件的URL：");
                UserInputUrl = Console.ReadLine();
                if (string.IsNullOrEmpty(UserInputUrl))
                {
                    Console.WriteLine("不能输入一个空的URL字串");
                    continue;
                }
                if (UserInputUrl.ToLower() == "quit")
                    break;
                Console.Write("输入要保存的文件名：");
                FileName = Console.ReadLine();
                if (string.IsNullOrEmpty(FileName))
                {
                    Console.WriteLine("不能输入一个空的URL字串");
                    continue;
                }
                if (FileName.ToLower() == "quit")
                    break;

                if (UserInputUrl.ToLower() == "quit")
                    break;
                try
                {
                    Uri webFileUri = new Uri(UserInputUrl);
                    WebRequest webRequest = WebRequest.Create(webFileUri);
                    DownLoadTask task = new DownLoadTask { SaveToFileName = FileName, WebRequestObject = webRequest };
                    Console.WriteLine("已在后台启动下载{0}", FileName);
                    webRequest.BeginGetResponse(DownloadFinished, task);

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }



            } while (true);

            Console.WriteLine("\n敲任意键结束本示例程序...");
         
            Console.ReadKey();
        }
    }
}
