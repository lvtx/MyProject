using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace ClassLibrary2
{
    public class MyOtherClassUseExcel
    {
        public void InsertIntoExcelWorksheet(string Message, Excel.Worksheet sheet, int rowIndex, int colIndex)
        {
            sheet.Cells[rowIndex, colIndex] = Message; //插入数据
         
        }
    }
}
