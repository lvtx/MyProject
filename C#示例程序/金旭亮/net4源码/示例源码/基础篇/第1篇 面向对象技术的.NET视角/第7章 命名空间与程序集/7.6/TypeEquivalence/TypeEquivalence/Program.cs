using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary1;
using ClassLibrary2;
using Excel = Microsoft.Office.Interop.Excel;
namespace TypeEquivalence
{
    class Program
    {
        static void Main(string[] args)
        {
            Excel.Application app = null;
            try
            {
                app = new Excel.Application();
                MyClassUseExcel obj1 = new MyClassUseExcel();
                MyOtherClassUseExcel obj2 = new MyOtherClassUseExcel();
                //在EXCEL工作表的[1,1]处插入一个字串
                obj2.InsertIntoExcelWorksheet("Hello,EXCEL!", obj1.CreateWorksheet(app), 1, 1);
                app.Visible = true;
                Console.WriteLine("Finished！");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                app.Quit();
            }

            
        }
    }
}
