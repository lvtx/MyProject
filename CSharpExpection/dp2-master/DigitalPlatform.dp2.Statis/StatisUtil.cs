using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DigitalPlatform.dp2.Statis
{
    // ʵ��static����
    public class StatisUtil
    {
        public static string Int64ToPrice(Int64 v)
        {
#if NO
            // ��������
            Int64 v1 = v / 100;

            // С������
            Int64 v2 = Math.Abs(v % 100);   // 2016/5/19

            return Convert.ToString(v1) + "." + Convert.ToString(v2).PadLeft(2, '0');
#endif
            //NumberFormatInfo nfi = new CultureInfo("zh-CN", false).NumberFormat;
            //nfi.NumberDecimalDigits = 2;

            Decimal d = Convert.ToDecimal(v / (double)100);
            return d.ToString();
        }
    }
}
