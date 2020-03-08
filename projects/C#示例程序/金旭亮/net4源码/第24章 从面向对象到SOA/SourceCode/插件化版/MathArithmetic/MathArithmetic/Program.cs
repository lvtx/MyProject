using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MathArithmetic
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //扫描装载算法插件
            AlgorithmObjects.FindAllAlgorithmObjs();
            Application.Run(new frmMain());
        }

        public static AlgorithmPlugIns AlgorithmObjects = new AlgorithmPlugIns();
       
    }
}
