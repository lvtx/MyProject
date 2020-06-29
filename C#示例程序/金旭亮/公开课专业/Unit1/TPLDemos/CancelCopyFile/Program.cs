using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CancelCopyFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = @"c:\data.zip";
            string target = @"c:\test\"+Path.GetRandomFileName()+".bak";
            Console.WriteLine("Target:"+target);
            using(var inStream=File.OpenRead(source))
            using(var outStream=File.OpenWrite(target))
            using (var cts = new CancellationTokenSource())
            {
                var task = inStream.CopyToAsync(outStream, 4096, cts.Token);
                Console.WriteLine("Copying.Press c to cancel");
                char key = Console.ReadKey().KeyChar;
                if (key == 'c')
                {
                    Console.WriteLine("cancelling");
                    cts.Cancel();
                }
                Console.WriteLine("Waiting....");
                task.ContinueWith(t => { }).Wait();
                Console.WriteLine("Status:"+task.Status);
            }
            Console.ReadKey();

        }
    }
}
