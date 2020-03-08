using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel=Microsoft.Office.Interop.Excel;
namespace ClassLibrary1
{
    public class MyClassUseExcel
    {
        /// <summary>
        /// 创建一个EXCEL工作表返回给外界
        /// </summary>
        /// <returns></returns>
        public Excel.Worksheet CreateWorksheet(Excel.Application app)
        {
            Excel.Workbook wbk=app.Workbooks.Add();
            return wbk.Sheets.Add();//添加一个工作表
        }
    }
}
