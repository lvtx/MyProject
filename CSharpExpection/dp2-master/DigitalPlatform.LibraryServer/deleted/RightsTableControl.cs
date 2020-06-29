using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

// using DigitalPlatform.Drawing;

namespace DigitalPlatform.LibraryServer
{
#if NOOOOOOOOOOOOOOOOO
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:RightsTableControl runat=server></{0}:RightsTableControl>")]
    public class RightsTableControl : WebControl
    {
        public LibraryApplication App = null;

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

#if NOOOOOOOOOOOOOOOOOOOOOOOOOOOO
        protected override void RenderContents(HtmlTextWriter output)
        {
            // output.Write(Text);
            string[] reader_d_paramnames = new string[] { 
                    "�ɽ��ܲ���",
                    "��ԤԼ����", 
                    "��ͣ��������",
                    "����������",
            };

            string[] two_d_paramnames = new string[] { 
                    // "�ɽ��ܲ���",
                    "�ɽ����",
                    "����" ,
                    // "��ԤԼ����",   // 2007/7/8
                    "����ΥԼ������",
                    "��ʧΥԼ������",
                    // "����������",
            };

            if (this.App == null)
            {
                output.Write("App��δ��ʼ��");
                return;
            }

            output.Write("<table border='1'>");

            List<String> readertypes = this.App.GetReaderTypes();

            List<String> booktypes = this.App.GetBookTypes();
            booktypes.Insert(0, "");    // ���ַ�������ֻ�Ͷ����йصĲ���

            // ����
            output.Write("<tr>");
            output.Write("<td>" + "" + "</td>");
            for (int j = 0; j < booktypes.Count; j++)
            {
                output.Write("<td>" + booktypes[j] + "</td>");
            }
            output.Write("</tr>");

            for (int i = 0; i < readertypes.Count; i++)
            {
                output.Write("<tr>");

                // ��ߵ�һ��
                output.Write("<td>" + readertypes[i] + "</td>");

                // ��ߵڶ��У�ֻ�Ͷ���������صĲ���

                for (int j = 0; j < booktypes.Count; j++)
                {
                    string strContent = "";
                    string strError = "";

                    if (j == 0)
                    {
                        for (int k = 0; k < reader_d_paramnames.Length; k++)
                        {
                            string strParamName = reader_d_paramnames[k];

                            string strParamValue = "";
                            string strStyle = "";
                            MatchResult matchresult;
                            int nRet = this.App.GetLoanParam(readertypes[i],
                                booktypes[j],   // ʵ����Ϊ��
                                strParamName,
                                out strParamValue,
                                out matchresult,
                                out strError);
                            if (nRet == -1)
                            {
                                strStyle = "STYLE=\"background-color:blue;font-weight:bold\"";
                                strContent += "<div " + strStyle + ">" + strParamName + ":" + strError + "</div>";
                            }
                            else
                            {
                                int r = 200;
                                int g = 200;
                                int b = 200;


                                if ((matchresult & MatchResult.BookType) == MatchResult.BookType)
                                {
                                    g += 55;
                                }
                                if ((matchresult & MatchResult.ReaderType) == MatchResult.ReaderType)
                                {
                                    r += 55;
                                }
                                Color color = Color.FromArgb(r, g, b);

                                strStyle = "STYLE=\"background-color:" + ColorUtil.Color2String(color) + "\"";

                                strContent += "<div " + strStyle + ">" + strParamName + ": " + strParamValue + "</div>";
                            }
                        } // end of for
                    }
                    else
                    {
                        for (int k = 0; k < two_d_paramnames.Length; k++)
                        {
                            string strParamName = two_d_paramnames[k];

                            string strParamValue = "";
                            string strStyle = "";
                            MatchResult matchresult;
                            int nRet = this.App.GetLoanParam(readertypes[i], booktypes[j], strParamName,
                                out strParamValue,
                                out matchresult,
                                out strError);
                            if (nRet == -1)
                            {
                                strStyle = "STYLE=\"background-color:blue;font-weight:bold\"";
                                strContent += "<div " + strStyle + ">" + strParamName + ":" + strError + "</div>";
                            }
                            else
                            {
                                int r = 200;
                                int g = 200;
                                int b = 200;


                                if ((matchresult & MatchResult.BookType) == MatchResult.BookType)
                                {
                                    g += 55;
                                }
                                if ((matchresult & MatchResult.ReaderType) == MatchResult.ReaderType)
                                {
                                    r += 55;
                                }
                                Color color = Color.FromArgb(r, g, b);

                                strStyle = "STYLE=\"background-color:" + ColorUtil.Color2String(color) + "\"";

                                strContent += "<div " + strStyle + ">" + strParamName + ": " + strParamValue + "</div>";
                            }
                        } // end of for
                    }
                    output.Write("<td>" + strContent + "</td>");
                }

                output.Write("<tr>");
            }

            output.Write("<tr>");

            output.Write("<td colspan=" + (booktypes.Count + 1).ToString() + ">");

            {
                string strStyle = "";
                string strContent = "";
                string strText = "";

                strContent = "<div>" + "ͼ��:" + "</div>";
                output.Write(strContent);

                strText = "����ȱʡֵ";
                Color color = Color.FromArgb(200, 200, 200);
                strStyle = "STYLE=\"background-color:" + ColorUtil.Color2String(color) + "\"";
                strContent = "<div " + strStyle + ">" + strText + "</div>";
                output.Write(strContent);

                strText = "��ƥ���˶�������";
                color = Color.FromArgb(255, 200, 200);
                strStyle = "STYLE=\"background-color:" + ColorUtil.Color2String(color) + "\"";
                strContent = "<div " + strStyle + ">" + strText + "</div>";
                output.Write(strContent);

                strText = "��ƥ����ͼ������";
                color = Color.FromArgb(200, 255, 200);
                strStyle = "STYLE=\"background-color:" + ColorUtil.Color2String(color) + "\"";
                strContent = "<div " + strStyle + ">" + strText + "</div>";
                output.Write(strContent);

                strText = "ͬʱƥ���˶��ߺ�ͼ������";
                color = Color.FromArgb(255, 255, 200);
                strStyle = "STYLE=\"background-color:" + ColorUtil.Color2String(color) + "\"";
                strContent = "<div " + strStyle + ">" + strText + "</div>";
                output.Write(strContent);
            }

            output.Write("</td>");
            output.Write("<tr>");


            output.Write("</table>");

        }

#endif

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.App == null)
            {
                output.Write("App��δ��ʼ��");
                return;
            }
            string strHtml = "";
            string strError = "";
            int nRet = this.App.GetRightTableHtml(
                "",
                out strHtml,
                out strError);
            if (nRet == -1)
                output.Write(HttpUtility.HtmlEncode(strError));
            else
                output.Write(strHtml);
        }

    }
#endif
}
