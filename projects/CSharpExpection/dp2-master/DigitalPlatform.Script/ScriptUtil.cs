﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;
using System.Xml;
using System.Reflection;

using DigitalPlatform.Marc;
using DigitalPlatform.Text;
using DigitalPlatform.LibraryClient;
using DigitalPlatform.LibraryClient.localhost;
using DigitalPlatform.Xml;

namespace DigitalPlatform.Script
{
    /// <summary>
    /// 工具性函数
    /// </summary>
    public class ScriptUtil
    {
        public static object InvokeMember(Type classType,
            string strFuncName,
            object target,
            object[] param_list)
        {
            while (classType != null)
            {
                try
                {
                    // 有两个参数的成员函数
                    // 用 GetMember 先探索看看函数是否存在
                    MemberInfo[] infos = classType.GetMember(strFuncName,
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod);
                    if (infos == null || infos.Length == 0)
                    {
                        classType = classType.BaseType;
                        if (classType == null)
                            break;
                        continue;
                    }

                    return classType.InvokeMember(strFuncName,
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod
                        ,
                        null,
                        target,
                        param_list);
                }
                catch (System.MissingMethodException/*ex*/)
                {
                    classType = classType.BaseType;
                    if (classType == null)
                        break;
                }
            }

            return null;
        }

        /// <summary>
        /// 从路径中取出库名部分
        /// </summary>
        /// <param name="strPath">路径。例如"中文图书/3"</param>
        /// <returns>返回库名部分</returns>
        public static string GetDbName(string strPath)
        {
            // 2018/10/24
            if (strPath == null)
                return "";

            int nRet = strPath.LastIndexOf("/");
            if (nRet == -1)
                return strPath;

            return strPath.Substring(0, nRet).Trim();
        }

        // 
        // parammeters:
        //      strPath 路径。例如"中文图书/3"
        /// <summary>
        /// 从路径中取出记录号部分
        /// </summary>
        /// <param name="strPath">路径。例如"中文图书/3"</param>
        /// <returns>返回记录号部分</returns>
        public static string GetRecordID(string strPath)
        {
            // 2018/10/24
            if (strPath == null)
                return "";

            int nRet = strPath.LastIndexOf("/");
            if (nRet == -1)
                return "";

            return strPath.Substring(nRet + 1).Trim();
        }

        // 为了二次开发脚本使用
        public static string MakeObjectUrl(string strRecPath,
            string strUri)
        {
            if (string.IsNullOrEmpty(strUri) == true)
                return strUri;

            if (StringUtil.IsHttpUrl(strUri) == true)
                return strUri;

            if (StringUtil.HasHead(strUri, "uri:") == true)
            {
                strUri = strUri.Substring(4).Trim();
                // 2018/11/2
                if (strUri.IndexOf("@") != -1)
                    return strUri;
            }

            string strDbName = GetDbName(strRecPath);
            string strRecID = GetRecordID(strRecPath);

            ReplaceUri(strUri,
                strDbName,
                strRecID,
                out string strOutputUri);

            return strOutputUri;
        }

        // "object/1"
        // "1/object/1"
        // "库名/1/object/1"
        // return:
        //		false	没有发生替换
        //		true	替换了
        static bool ReplaceUri(string strUri,
            string strCurDbName,
            string strCurRecID,
            out string strOutputUri)
        {
            strOutputUri = strUri;
            string strTemp = strUri;
            // 看看第一部分是不是object
            string strPart = StringUtil.GetFirstPartPath(ref strTemp);
            if (strPart == "")
                return false;

            if (strTemp == "")
            {
                strOutputUri = strCurDbName + "/" + strCurRecID + "/object/" + strPart;
                return true;
            }

            if (strPart == "object")
            {
                strOutputUri = strCurDbName + "/" + strCurRecID + "/object/" + strTemp;
                return true;
            }

            string strPart2 = StringUtil.GetFirstPartPath(ref strTemp);
            if (strPart2 == "")
                return false;

            if (strPart2 == "object")
            {
                strOutputUri = strCurDbName + "/" + strPart + "/object/" + strTemp;
                return false;
            }

            string strPart3 = StringUtil.GetFirstPartPath(ref strTemp);
            if (strPart3 == "")
                return false;

            if (strPart3 == "object")
            {
                strOutputUri = strPart + "/" + strPart2 + "/object/" + strTemp;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获得封面图像 URL
        /// 优先选择中等大小的图片
        /// </summary>
        /// <param name="strMARC">MARC机内格式字符串</param>
        /// <param name="strPreferredType">优先使用何种大小类型</param>
        /// <returns>返回封面图像 URL。空表示没有找到</returns>
        public static string GetCoverImageUrl(string strMARC,
            string strPreferredType = "MediumImage")
        {
            string strLargeUrl = "";
            string strMediumUrl = "";   // type:FrontCover.MediumImage
            string strUrl = ""; // type:FronCover
            string strSmallUrl = "";

            MarcRecord record = new MarcRecord(strMARC);
            MarcNodeList fields = record.select("field[@name='856']");
            foreach (MarcField field in fields)
            {
                string x = field.select("subfield[@name='x']").FirstContent;
                if (string.IsNullOrEmpty(x) == true)
                    continue;
                Hashtable table = StringUtil.ParseParameters(x, ';', ':');
                string strType = (string)table["type"];
                if (string.IsNullOrEmpty(strType) == true)
                    continue;

                string u = field.select("subfield[@name='u']").FirstContent;
                // if (string.IsNullOrEmpty(u) == true)
                //     u = field.select("subfield[@name='8']").FirstContent;

                // . 分隔 FrontCover.MediumImage
                if (StringUtil.HasHead(strType, "FrontCover." + strPreferredType) == true)
                    return u;

                if (StringUtil.HasHead(strType, "FrontCover.SmallImage") == true)
                    strSmallUrl = u;
                else if (StringUtil.HasHead(strType, "FrontCover.MediumImage") == true)
                    strMediumUrl = u;
                else if (StringUtil.HasHead(strType, "FrontCover.LargeImage") == true)
                    strLargeUrl = u;
                else if (StringUtil.HasHead(strType, "FrontCover") == true)
                    strUrl = u;

            }

            if (string.IsNullOrEmpty(strLargeUrl) == false)
                return strLargeUrl;
            if (string.IsNullOrEmpty(strMediumUrl) == false)
                return strMediumUrl;
            if (string.IsNullOrEmpty(strUrl) == false)
                return strUrl;
            return strSmallUrl;
        }

        [Flags]
        public enum BuildObjectHtmlTableStyle
        {
            None = 0,
            HttpUrlHitCount = 0x01,     // 是否对 http:// 地址进行访问计数
            FrontCover = 0x02,   // 是否包含封面图像事项
            Template = 0x04,    // 是否利用模板机制对 $u 进行自动处理 2017/12/19
            TemplateMultiHit = 0x08,    // 使用模板功能时候，是否允许多重匹配命中 item 元素。如果不允许，则命中第一个时候就停止了
        }

        // 兼容以前的版本
        public static string BuildObjectHtmlTable(string strMARC,
    string strRecPath,
    BuildObjectHtmlTableStyle style = BuildObjectHtmlTableStyle.HttpUrlHitCount)
        {
            return BuildObjectHtmlTable(strMARC,
                strRecPath,
                null,
                style);
        }

        public static string BuildObjectHtmlTable(string strMARC,
    string strRecPath,
    XmlElement maps_container,
    string strMarcSyntax)
        {
            return BuildObjectHtmlTable(strMARC,
    strRecPath,
    maps_container,
    BuildObjectHtmlTableStyle.HttpUrlHitCount | BuildObjectHtmlTableStyle.Template,
    strMarcSyntax);
        }

        // 创建 OPAC 详细页面中的对象资源显示局部 HTML。这是一个 <table> 片段
        // 前导语 $3
        // 链接文字 $y $f
        // URL $u
        // 格式类型 $q
        // 对象ID $8
        // 对象尺寸 $s
        // 公开注释 $z
        public static string BuildObjectHtmlTable(string strMARC,
            string strRecPath,
            XmlElement maps_container,
            BuildObjectHtmlTableStyle style = BuildObjectHtmlTableStyle.HttpUrlHitCount | BuildObjectHtmlTableStyle.Template,
            string strMarcSyntax = "unimarc")
        {
            // Debug.Assert(false, "");

            MarcRecord record = new MarcRecord(strMARC);
            MarcNodeList fields = record.select("field[@name='856']");

            if (fields.count == 0)
                return "";

            StringBuilder text = new StringBuilder();

            text.Append("<table class='object_table'>");
            text.Append("<tr class='column_title'>");
            text.Append("<td class='type' style='word-break:keep-all;'>材料</td>");
            text.Append("<td class='hitcount'></td>");
            text.Append("<td class='link' style='word-break:keep-all;'>链接</td>");
            text.Append("<td class='mime' style='word-break:keep-all;'>媒体类型</td>");
            text.Append("<td class='size' style='word-break:keep-all;'>尺寸</td>");
            text.Append("<td class='bytes' style='word-break:keep-all;'>字节数</td>");
            text.Append("</tr>");

            int nCount = 0;
            foreach (MarcField field in fields)
            {
                string x = field.select("subfield[@name='x']").FirstContent;

                Hashtable table = StringUtil.ParseParameters(x, ';', ':');
                string strType = (string)table["type"];

                // TODO:
                if (strType == null) strType = "";

                if (string.IsNullOrEmpty(strType) == false
                    && (style & BuildObjectHtmlTableStyle.FrontCover) == 0
                    && (strType == "FrontCover" || strType.StartsWith("FrontCover.") == true))
                    continue;

                string strSize = (string)table["size"];
                string s_q = field.select("subfield[@name='q']").FirstContent;  // 注意， FirstContent 可能会返回 null

                List<Map856uResult> u_list = new List<Map856uResult>();
                {
                    string u = field.select("subfield[@name='u']").FirstContent;
                    // Hashtable parameters = new Hashtable();
                    if (maps_container != null
                        && (style & BuildObjectHtmlTableStyle.Template) != 0)
                    {
                        // return:
                        //     -1  出错
                        //     0   没有发生宏替换
                        //     1   发生了宏替换
                        int nRet = Map856u(u,
                            strRecPath,
                            maps_container,
                            // parameters,
                            style,
                            out u_list, // strUri,
                            out string strError);
                        //if (nRet == -1)
                        //    strUri = "!error:" + strError;
                        if (nRet == -1)
                            u_list.Add(new Map856uResult { Result = "!error: 对 858$u 内容 '" + u + "' 进行映射变换时出错: " + strError });
                    }
                    else
                    {
                        u_list.Add(new Map856uResult { Result = u });   // WrapUrl == true ?
                    }
                }

                foreach (Map856uResult result in u_list)
                {
                    string strUri = MakeObjectUrl(strRecPath, result.Result);
                    Hashtable parameters = result.Parameters;

                    string strSaveAs = "";
                    if (string.IsNullOrEmpty(s_q) == true
                        // || StringUtil.MatchMIME(s_q, "text") == true
                        || StringUtil.MatchMIME(s_q, "image") == true)
                    {

                    }
                    else
                    {
                        strSaveAs = "&saveas=true";
                    }
                    string strHitCountImage = "";
                    string strObjectUrl = strUri;
                    string strPdfUrl = "";
                    string strThumbnailUrl = "";
                    if (result.WrapUrl == true)
                    {
                        if (StringUtil.IsHttpUrl(strUri) == false)
                        {
                            // 内部对象
                            strObjectUrl = "./getobject.aspx?uri=" + HttpUtility.UrlEncode(strUri) + strSaveAs;
                            strHitCountImage = "<img src='" + strObjectUrl + "&style=hitcount' alt='hitcount'></img>";
                            if (s_q == "application/pdf")
                            {
                                strPdfUrl = "./viewpdf.aspx?uri=" + HttpUtility.UrlEncode(strUri);
                                strThumbnailUrl = "./getobject.aspx?uri=" + HttpUtility.UrlEncode(strUri + "/page:1,format=jpeg,dpi:24");
                            }
                        }
                        else
                        {
                            // http: 或 https: 的情形，即外部 URL
                            if ((style & BuildObjectHtmlTableStyle.HttpUrlHitCount) != 0)
                            {
                                strObjectUrl = "./getobject.aspx?uri=" + HttpUtility.UrlEncode(strUri) + strSaveAs + "&biblioRecPath=" + HttpUtility.UrlEncode(strRecPath);
                                strHitCountImage = "<img src='" + strObjectUrl + "&style=hitcount&biblioRecPath=" + HttpUtility.UrlEncode(strRecPath) + "' alt='hitcount'></img>";
                            }
                        }
                    }
                    else
                    {
                        strObjectUrl = strUri;
                        strHitCountImage = "";
                    }

                    string linkText = "";

                    if (strMarcSyntax == "unimarc")
                        linkText = field.select("subfield[@name='2']").FirstContent;
                    else
                        linkText = field.select("subfield[@name='y']").FirstContent;

                    string f = field.select("subfield[@name='f']").FirstContent;

                    string urlLabel = "";
                    if (string.IsNullOrEmpty(linkText) == false)
                        urlLabel = linkText;
                    else
                        urlLabel = f;
                    if (string.IsNullOrEmpty(urlLabel) == true)
                        urlLabel = strType;

                    // 2015/11/26
                    string s_z = field.select("subfield[@name='z']").FirstContent;
                    if (string.IsNullOrEmpty(urlLabel) == true
                        && string.IsNullOrEmpty(s_z) == false)
                    {
                        urlLabel = s_z;
                        s_z = "";
                    }

                    if (string.IsNullOrEmpty(urlLabel) == true)
                        urlLabel = strObjectUrl;

                    //
                    if (StringUtil.StartsWith(strUri, "!error:"))
                        urlLabel += strUri;

                    // 2018/11/5
                    urlLabel = Map856uResult.MacroAnchorText(result.AnchorText, urlLabel);

                    if (string.IsNullOrEmpty(strPdfUrl) == false && string.IsNullOrEmpty(urlLabel) == false)
                        strPdfUrl += "&title=" + HttpUtility.UrlEncode(urlLabel);

                    string urlTemp = "";
                    if (String.IsNullOrEmpty(strObjectUrl) == false)
                    {
                        string strParameters = "";
                        if (parameters != null)
                        {
                            foreach (string name in parameters.Keys)
                            {
                                strParameters += HttpUtility.HtmlAttributeEncode(name) + "='" + HttpUtility.HtmlAttributeEncode(parameters[name] as string) + "' "; // 注意，内容里面是否有单引号？
                            }
                        }
                        urlTemp += "<a class='link' href='" + strObjectUrl + "' " + strParameters.Trim() + " >";
                        urlTemp += HttpUtility.HtmlEncode(
                            (string.IsNullOrEmpty(strSaveAs) == false ? "下载 " : "")
                            + urlLabel);
                        urlTemp += "</a>";

                        if (string.IsNullOrEmpty(strPdfUrl) == false)
                        {
#if NO
                        // 预览 按钮
                        urlTemp += "<br/><a href='" + strPdfUrl + "' target='_blank'>";
                        urlTemp += HttpUtility.HtmlEncode("预览 " + urlLabel);
                        urlTemp += "</a>";
#endif

                            // 缩略图 点按和预览按钮效果相同
                            urlTemp += "<br/><a class='thumbnail' href='" + strPdfUrl + "' target='_blank' alt='" + HttpUtility.HtmlEncode("在线阅读 " + urlLabel) + "'>";
                            urlTemp += "<img src='" + strThumbnailUrl + "' alt='" + HttpUtility.HtmlEncode("在线阅读 " + urlLabel) + "'></img>";
                            urlTemp += "</a>";
                        }
                    }
                    else
                        urlTemp = urlLabel;

                    // Different parts of the item are electronic, using subfield $3 to indicate the part (e.g., table of contents accessible in one file and an abstract in another)
                    // 意思就是，如果有多种部分是电子资源，用 $3 指明当前 856 针对的哪个部分。这时候有多个 856，每个 856 中的 $3 各不相同
                    string s_3 = field.select("subfield[@name='3']").FirstContent;
                    string s_s = field.select("subfield[@name='s']").FirstContent;

                    text.Append("<tr class='content'>");
                    text.Append("<td class='type'>" + HttpUtility.HtmlEncode(s_3 + " " + strType) + "</td>");
                    text.Append("<td class='hitcount' style='text-align: right;'>" + strHitCountImage + "</td>");
                    text.Append("<td class='link' style='word-break:break-all;'>" + urlTemp + "</td>");
                    text.Append("<td class='mime'>" + HttpUtility.HtmlEncode(s_q) + "</td>");
                    text.Append("<td class='size'>" + HttpUtility.HtmlEncode(strSize) + "</td>");
                    text.Append("<td class='bytes'>" + HttpUtility.HtmlEncode(s_s) + "</td>");
                    text.Append("</tr>");

                    if (string.IsNullOrEmpty(s_z) == false)
                    {
                        text.Append("<tr class='comment'>");
                        text.Append("<td colspan='6'>" + HttpUtility.HtmlEncode(s_z) + "</td>");
                        text.Append("</tr>");
                    }
                    nCount++;
                }
            }

            if (nCount == 0)
                return "";

            text.Append("</table>");

            return text.ToString();
        }

        #region 856 maps function

        public class Map856uResult
        {
            // 模板兑现宏以后的结果
            public string Result { get; set; }
            // 环境参数
            public Hashtable Parameters { get; set; }
            // 锚点文字附加部分
            public string AnchorText { get; set; }

            bool _wrapUrl = true;
            public bool WrapUrl
            {
                get { return _wrapUrl; }
                set { _wrapUrl = value; }
            }

            public static string MacroAnchorText(string template, string old_text)
            {
                if (template == null)
                    return old_text;
                return template.Replace("{text}", old_text);
            }
        }

        /*
<856_maps>
<item type="cxstar" template="http://www.cxstar.com:5000/Book/Detail?pinst=1ca53a3a0001390bce&ruid=%uri%" />
<item type="default" template="http://localhost:8081/dp2OPAC/getobject.aspx?uri=%object_id%" />
<item type="default" template="%getobject_module%?uri=%object_path%" />
</856_maps>
 * 
 * */
        // return:
        //     -1  出错
        //     0   没有发生宏替换
        //     1   发生了宏替换
        public static int Map856u(string u,
            string strBiblioRecPath,
            XmlElement container,
            // Hashtable parameters,
            BuildObjectHtmlTableStyle style,
            out List<Map856uResult> results,
            out string strError)
        {
            strError = "";

            results = new List<Map856uResult>();

            if (string.IsNullOrEmpty(u))
            {
                results.Add(new Map856uResult { Result = u });
                return 0;
            }

            if (StringUtil.HasHead(u, "uri:") == false)
            {
                results.Add(new Map856uResult { Result = u, WrapUrl = true });
                return 0;
            }

            u = u.Substring(4).Trim();

            List<string> parts = StringUtil.ParseTwoPart(u, "@");
            string uri = parts[0];
            string type = parts[1];

            XmlNodeList items = null;
            if (string.IsNullOrEmpty(type))
            {
                items = container.SelectNodes("item[@type='default']");
                if (items.Count == 0)
                {
                    strError = "配置文件中没有配置 type='default' 的 856_maps/item 元素";
                    return -1;
                }
            }
            else
            {
                items = container.SelectNodes("item[@type='" + type + "']");
                if (items.Count == 0)
                {
                    strError = "配置文件中没有配置 type='" + type + "' 的 856_maps/item 元素";
                    return -1;
                }
            }

            foreach (XmlElement item in items)
            {
                string template = item.GetAttribute("template");
                if (string.IsNullOrEmpty(template))
                {
                    strError = "配置文件中元素 " + item.OuterXml + " 没有配置 template 属性";
                    return -1;
                }

                Hashtable parameters = new Hashtable();

                // 取得 _xxxx 属性值
                if (parameters != null)
                {
                    foreach (XmlAttribute attr in item.Attributes)
                    {
                        if (StringUtil.StartsWith(attr.Name, "_"))
                            parameters[attr.Name.Substring(1)] = attr.Value;
                    }
                }

                string object_path = MakeObjectUrl(strBiblioRecPath, uri);

                string result = template.Replace("{encoded_object_path}", HttpUtility.UrlEncode(object_path));
                result = result.Replace("{encoded_uri}", HttpUtility.UrlEncode(uri));
                result = result.Replace("{object_path}", object_path);
                result = result.Replace("{uri}", uri);
                result = result.Replace("{getobject_module}", "./getobject.aspx");

                string anchorText = item.HasAttribute("anchorText") ? item.GetAttribute("anchorText") : null;
                bool wrapUrl = DomUtil.IsBooleanTrue(item.GetAttribute("wrapUrl"), true);
                results.Add(new Map856uResult
                {
                    Result = result,
                    Parameters = parameters,
                    AnchorText = anchorText,
                    WrapUrl = wrapUrl
                });

                if ((style & BuildObjectHtmlTableStyle.TemplateMultiHit) == 0)
                    break;
            }

            return 1;
        }

#if NO
        /*
<856_maps>
<item type="cxstar" template="http://www.cxstar.com:5000/Book/Detail?pinst=1ca53a3a0001390bce&ruid=%uri%" />
<item type="default" template="http://localhost:8081/dp2OPAC/getobject.aspx?uri=%object_id%" />
<item type="default" template="%getobject_module%?uri=%object_path%" />
</856_maps>
         * 
         * */
        // return:
        //     -1  出错
        //     0   没有发生宏替换
        //     1   发生了宏替换
        public static int Map856u(string u,
            string strBiblioRecPath,
            XmlElement container,
            Hashtable parameters,
            out string result,
            out string strError)
        {
            strError = "";

            result = u;
            if (string.IsNullOrEmpty(u))
                return 0;

            if (StringUtil.HasHead(u, "uri:") == false)
                return 0;

            u = u.Substring(4).Trim();

            List<string> parts = StringUtil.ParseTwoPart(u, "@");
            string uri = parts[0];
            string type = parts[1];

            XmlElement item = null;
            if (string.IsNullOrEmpty(type))
            {
                item = container.SelectSingleNode("item[@type='default']") as XmlElement;
                if (item == null)
                {
                    strError = "配置文件中没有配置 type='default' 的 856_maps/item 元素";
                    return -1;
                }
            }
            else
            {
                item = container.SelectSingleNode("item[@type='" + type + "']") as XmlElement;
                if (item == null)
                {
                    strError = "配置文件中没有配置 type='" + type + "' 的 856_maps/item 元素";
                    return -1;
                }
            }

            string template = item.GetAttribute("template");
            if (string.IsNullOrEmpty(template))
            {
                strError = "配置文件中元素 " + item.OuterXml + " 没有配置 template 属性";
                return -1;
            }

            // 取得 _xxxx 属性值
            if (parameters != null)
            {
                foreach (XmlAttribute attr in item.Attributes)
                {
                    if (attr.Name.StartsWith("_"))
                        parameters[attr.Name.Substring(1)] = attr.Value;
                }
            }

            string object_path = MakeObjectUrl(strBiblioRecPath, uri);

            result = template.Replace("{object_path}", HttpUtility.UrlEncode(object_path));
            result = result.Replace("{uri}", HttpUtility.UrlEncode(uri));
            result = result.Replace("{getobject_module}", "./getobject.aspx");
            return 1;
        }
#endif

        #endregion

        // 创建 table 中的对象资源局部 XML。这是一个 <table> 片段
        // 前导语 $3
        // 链接文字 $y $f
        // URL $u
        // 格式类型 $q
        // 对象ID $8
        // 对象尺寸 $s
        // 公开注释 $z
        public static string BuildObjectXmlTable(string strMARC,
            // string strRecPath,
            BuildObjectHtmlTableStyle style = BuildObjectHtmlTableStyle.None,
            string strMarcSyntax = "unimarc",
            string strRecPath = null,
            XmlElement maps_container = null)
        {
            // Debug.Assert(false, "");

            MarcRecord record = new MarcRecord(strMARC);
            MarcNodeList fields = record.select("field[@name='856']");

            if (fields.count == 0)
                return "";

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<table/>");

            int nCount = 0;
            foreach (MarcField field in fields)
            {
                string x = field.select("subfield[@name='x']").FirstContent;

                Hashtable table = StringUtil.ParseParameters(x, ';', ':');
                string strType = (string)table["type"];

                if (strType == null)
                    strType = "";

                if (string.IsNullOrEmpty(strType) == false
                    && (style & BuildObjectHtmlTableStyle.FrontCover) == 0
                    && (strType == "FrontCover" || StringUtil.StartsWith(strType, "FrontCover.") == true))
                    continue;

                string strSize = (string)table["size"];
                string s_q = field.select("subfield[@name='q']").FirstContent;  // 注意， FirstContent 可能会返回 null

                List<Map856uResult> u_list = new List<Map856uResult>();
                {
                    string u = field.select("subfield[@name='u']").FirstContent;

                    // 2018/10/24
                    // Hashtable parameters = new Hashtable();
                    if (maps_container != null
                        && (style & BuildObjectHtmlTableStyle.Template) != 0)
                    {
                        // string strUri = MakeObjectUrl(strRecPath, u);
                        // return:
                        //     -1  出错
                        //     0   没有发生宏替换
                        //     1   发生了宏替换
                        int nRet = Map856u(u,
                            strRecPath,
                            maps_container,
                            style,
                            // parameters,
                            out u_list,
                            out string strError);
                        if (nRet == -1)
                            u_list.Add(new Map856uResult { Result = "!error: 对 858$u 内容 '" + u + "' 进行映射变换时出错: " + strError });
                    }
                    else
                    {
                        u_list.Add(new Map856uResult { Result = u });   // WrapUrl == true ?
                    }
                }

                string strSaveAs = "";
                if (string.IsNullOrEmpty(s_q) == true   // 2016/9/4
                    || StringUtil.MatchMIME(s_q, "text") == true
                    || StringUtil.MatchMIME(s_q, "image") == true)
                {

                }
                else
                {
                    strSaveAs = "true";
                }

#if NO
                string y = field.select("subfield[@name='y']").FirstContent;
                string f = field.select("subfield[@name='f']").FirstContent;

                string urlLabel = "";
                if (string.IsNullOrEmpty(y) == false)
                    urlLabel = y;
                else
                    urlLabel = f;
                if (string.IsNullOrEmpty(urlLabel) == true)
                    urlLabel = strType;
#endif
                string linkText = "";

                if (strMarcSyntax == "unimarc")
                    linkText = field.select("subfield[@name='2']").FirstContent;
                else
                    linkText = field.select("subfield[@name='y']").FirstContent;

                string f = field.select("subfield[@name='f']").FirstContent;

                string urlLabel = "";
                if (string.IsNullOrEmpty(linkText) == false)
                    urlLabel = linkText;
                else
                    urlLabel = f;
                if (string.IsNullOrEmpty(urlLabel) == true)
                    urlLabel = strType;

                // 2015/11/26
                string s_z = field.select("subfield[@name='z']").FirstContent;
                if (string.IsNullOrEmpty(urlLabel) == true
                    && string.IsNullOrEmpty(s_z) == false)
                {
                    urlLabel = s_z;
                    s_z = "";
                }


#if NO
                string urlTemp = "";
                if (String.IsNullOrEmpty(strObjectUrl) == false)
                {
                    urlTemp += "<a href='" + strObjectUrl + "'>";
                    urlTemp += urlLabel;
                    urlTemp += "</a>";
                }
                else
                    urlTemp = urlLabel;
#endif

                string s_3 = field.select("subfield[@name='3']").FirstContent;
                string s_s = field.select("subfield[@name='s']").FirstContent;

                foreach (Map856uResult u in u_list)
                {
                    XmlElement line = dom.CreateElement("line");
                    dom.DocumentElement.AppendChild(line);

                    string strTypeString = (s_3 + " " + strType).Trim();
                    if (string.IsNullOrEmpty(strTypeString) == false)
                        line.SetAttribute("type", strTypeString);

                    string currentUrlLabel = urlLabel;
                    if (string.IsNullOrEmpty(currentUrlLabel) == true)
                    {
                        if (u.AnchorText != null)
                            currentUrlLabel = Map856uResult.MacroAnchorText(u.AnchorText, currentUrlLabel);
                        else
                            currentUrlLabel = u.Result;
                    }
                    else
                    {
                        if (u.AnchorText != null)
                            currentUrlLabel = Map856uResult.MacroAnchorText(u.AnchorText, urlLabel);
                    }

                    if (string.IsNullOrEmpty(currentUrlLabel) == false)
                        line.SetAttribute("urlLabel", currentUrlLabel);

                    if (string.IsNullOrEmpty(u.Result) == false)
                        line.SetAttribute("uri", u.Result);

                    if (u.Parameters != null && u.Parameters.Count > 0)
                        line.SetAttribute("uriEnv", StringUtil.BuildParameterString(u.Parameters, ',', '=', "url"));

                    if (string.IsNullOrEmpty(s_q) == false)
                        line.SetAttribute("mime", s_q);

                    if (string.IsNullOrEmpty(strSize) == false)
                        line.SetAttribute("size", strSize);

                    if (string.IsNullOrEmpty(s_s) == false)
                        line.SetAttribute("bytes", s_s);

                    if (string.IsNullOrEmpty(strSaveAs) == false)
                        line.SetAttribute("saveAs", strSaveAs);

                    if (string.IsNullOrEmpty(s_z) == false)
                        line.SetAttribute("comment", s_z);
                    nCount++;
                }
            }

            if (nCount == 0)
                return "";

            return dom.DocumentElement.OuterXml;
        }
    }

    /// <summary>
    /// 对 LibraryChannel 的扩展
    /// </summary>
    public static class LibraryChannelExtension2
    {
        // 获得指定一期的封面图片 URI
        // parameters:
        //      strBiblioPath   书目记录路径
        //      strQueryString  检索词。例如 “2005|1|1000|50”。格式为 年|期号|总期号|卷号。一般为 年|期号| 即可。
        public static int GetIssueCoverImageUri(this LibraryChannel channel,
            DigitalPlatform.Stop stop,
            string strBiblioRecPath,
            string strQueryString,
            string strPreferredType,
            out string strUri,
            out string strError)
        {
            strUri = "";
            strError = "";

            string strBiblioRecordID = StringUtil.GetRecordId(strBiblioRecPath);
            string strStyle = "query:父记录+期号|" + strBiblioRecordID + "|" + strQueryString;
            DigitalPlatform.LibraryClient.localhost.EntityInfo[] issueinfos = null;
            long lRet = channel.GetIssues(stop,
                strBiblioRecPath,
                0,
                1,
                strStyle,
                channel.Lang,
                out issueinfos,
                out strError);
            if (lRet == -1)
                return -1;
            if (lRet == 0)
                return 0;   // not found

            EntityInfo info = issueinfos[0];
            string strXml = info.OldRecord;
            string strIssueRecordPath = info.OldRecPath;

            if (string.IsNullOrEmpty(strXml))
            {
                strError = "期记录 '" + strIssueRecordPath + "' 的 strXml 为空";
                return -1;
            }

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(strXml);
            }
            catch (Exception ex)
            {
                strError = "期记录 '" + strIssueRecordPath + "' XML 装入 DOM 时出错: " + ex.Message;
                return -1;
            }

            string strObjectID = dp2StringUtil.GetCoverImageIDFromIssueRecord(dom, strPreferredType);
            if (string.IsNullOrEmpty(strObjectID))
                return 0;

            strUri = strIssueRecordPath + "/object/" + strObjectID;
            return 1;
        }

        // 根据 query string，获得指定一期的期记录数量
        // query string 是调用前从册记录中 volume 等字段综合取得的
        // parameters:
        //      strBiblioPath   书目记录路径
        //      strQueryString  检索词。例如 “2005|1|1000|50”。格式为 年|期号|总期号|卷号。一般为 年|期号| 即可。
        public static int GetIssueCount(this LibraryChannel channel,
            DigitalPlatform.Stop stop,
            string strBiblioRecPath,
            string strQueryString,
            out string strError)
        {
            strError = "";

            string strBiblioRecordID = StringUtil.GetRecordId(strBiblioRecPath);
            string strStyle = "query:父记录+期号|" + strBiblioRecordID + "|" + strQueryString;
            DigitalPlatform.LibraryClient.localhost.EntityInfo[] issueinfos = null;
            long lRet = channel.GetIssues(stop,
                strBiblioRecPath,
                0,
                1,
                strStyle,
                channel.Lang,
                out issueinfos,
                out strError);
            if (lRet == -1)
                return -1;
            return (int)lRet;
        }
    }
}
