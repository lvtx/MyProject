#if NO

using System;
using System.Drawing;

using DigitalPlatform;

namespace DigitalPlatform.Drawing
{
	// ��д��:���ӻ�
	public class ColorUtil
	{
		//����#XXXXXX��ʽ�ַ����õ�Color
		public static Color String2Color(string strColor)
		{
			string strR = strColor.Substring (1,2);
			int nR = ConvertUtil.S2Int32(strR,16);

			string strG = strColor.Substring(3,2);
			int nG = ConvertUtil.S2Int32(strG,16);

			string strB = strColor.Substring (5,2);
			int nB = ConvertUtil.S2Int32(strB,16);

			return Color.FromArgb(nR,nG,nB);
		}

		//��Colorת����#XXXXXX��ʽ�ַ���
		public static string Color2String(Color color)
		{
			int nR = color.R ;
			string strR = Convert.ToString (nR,16);
			if (strR == "0")
				strR = "00";

			int nG = color.G ;
			string strG = Convert.ToString (nG,16);
			if (strG == "0")
				strG = "00";

			int nB = color.B ;
			string strB = Convert.ToString (nB,16);
			if (strB == "0")
				strB = "00";

			return "#" + strR + strG + strB;
		}
	}
}

#endif
