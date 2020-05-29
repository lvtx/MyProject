﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DigitalPlatform.LibraryClient;
using DigitalPlatform.OPAC.Server;
using DigitalPlatform.OPAC.Web;

public partial class ViewPdf : MyWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (WebUtil.PrepareEnvironment(this,
ref app,
ref sessioninfo) == false)
            return;

        string strError = "";

        if (string.IsNullOrEmpty(sessioninfo.UserID))
        {
            sessioninfo.UserID = "public";
            sessioninfo.IsReader = false;
        }

        string strTitle = Request.QueryString["title"];
        this.TitleParam.Value = strTitle;
        if (string.IsNullOrEmpty(strTitle) == false)
            this.Title = strTitle;

        string strURI = Request.QueryString["uri"]; // uri 参数里面是到对象这一级的 URI。更深部分由程序自动生成
        if (string.IsNullOrEmpty(strURI))
        {
            strError = "缺乏 uri 参数";
            goto ERROR1;
        }

        this.Uri.Value = strURI;

        // 参数里面可以主动带上 pagecount 参数，这样后面就不用专门用一次 API 去获取页数了
        string strPageCount = Request.QueryString["pagecount"];
        if (string.IsNullOrEmpty(strPageCount) == false)
        {
            this.PageCount.Value = strPageCount;

            this.TailPage.Text = ">| " + this.PageCount.Value;
        }

        string strPageNo = Request.QueryString["page"];
        if (string.IsNullOrEmpty(strPageNo))
            strPageNo = "1";

        Int32.TryParse(strPageNo, out int nPageNo);
        SetPageNo(nPageNo);
#if NO
        this.PageNo.Value = strPageNo;

        string strURI = Request.QueryString["uri"]; // uri 参数里面是到对象这一级的 URI。更深部分由程序自动生成
        this.Image1.ImageUrl = "./getobject.aspx?uri=" + strURI + "/page:"+strPageNo+",format:jpeg,dpi:75";

        Int32.TryParse(strPageNo, out int nPageNo);
        this.PrevPage.PostBackUrl = "./viewpdf.aspx?uri=" + strURI + "&page=" + (nPageNo - 1);
        this.NextPage.PostBackUrl = "./viewpdf.aspx?uri=" + strURI + "&page=" + (nPageNo - 1);
#endif
        return;
        ERROR1:
        this.SetErrorInfo(strError);
    }

    protected void PrevPage_Click(object sender, EventArgs e)
    {
        string strPageNo = this.PageNo.Value;
        Int32.TryParse(strPageNo, out int nPageNo);
        SetPageNo(nPageNo);
    }

    protected void NextPage_Click(object sender, EventArgs e)
    {
        int nPageCount = GetPageCount();

        string strPageNo = this.PageNo.Value;
        Int32.TryParse(strPageNo, out int nPageNo);

        if (nPageNo <= nPageCount)
            SetPageNo(nPageNo);
        else
            SetPageNo(nPageCount);
    }

    protected void FirstPage_Click(object sender, EventArgs e)
    {
        SetPageNo(1);
    }

    protected void TailPage_Click(object sender, EventArgs e)
    {
#if NO
        string strUri = this.Uri.Value;

        string strPageCount = this.PageCount.Value;
        if (string.IsNullOrEmpty(strPageCount))
        {
            strPageCount = GetPageCount(strUri).ToString();
            this.PageCount.Value = strPageCount;
        }

        this.TailPage.Text = ">| " + this.PageCount.Value;

        Int32.TryParse(strPageCount, out int nPageCount);
        SetPageNo(nPageCount);
#endif
        int nPageCount = GetPageCount();
        if (nPageCount == -1)
        {
            SetErrorInfo("GetPageCount() error");
            return;
        }
        SetPageNo(nPageCount);
    }

    int GetPageCount()
    {
        string strUri = this.Uri.Value;
        if (string.IsNullOrEmpty(strUri))
            return -1;
        string strPageCount = this.PageCount.Value;
        if (string.IsNullOrEmpty(strPageCount))
        {
            strPageCount = GetPageCount(strUri).ToString();
            this.PageCount.Value = strPageCount;
        }

        this.TailPage.Text = ">| " + this.PageCount.Value;

        if (Int32.TryParse(strPageCount, out int nPageCount) == false)
            return -1;
        return nPageCount;
    }

    void SetPageNo(int nPageNo)
    {
        this.PageNo.Value = nPageNo.ToString();
        this.LabelPageNo.Text = nPageNo.ToString();

        string strURI = Request.QueryString["uri"]; // uri 参数里面是到对象这一级的 URI。更深部分由程序自动生成
        this.Image1.ImageUrl = "./getobject.aspx?uri=" + strURI + "/page:" + nPageNo.ToString() + ",format:jpeg,dpi:75";

        if (nPageNo > 1)
        {
            this.PrevPage.PostBackUrl = "./viewpdf.aspx?uri=" + strURI + "&page=" + (nPageNo - 1);
            this.PrevPage.Enabled = true;
        }
        else
            this.PrevPage.Enabled = false;

        int nPageCount = GetPageCount();
        if (nPageCount != -1 && nPageNo >= nPageCount)
            this.NextPage.Enabled = false;
        else
        {
            this.NextPage.PostBackUrl = "./viewpdf.aspx?uri=" + strURI + "&page=" + (nPageNo + 1);
            this.NextPage.Enabled = true;
        }
    }

    int GetPageCount(string strUri)
    {
        WebPageStop stop = new WebPageStop(this);

        LibraryChannel channel = sessioninfo.GetChannel(true);

        try
        {
            // 只获得媒体类型
            long lRet = channel.GetRes(
                stop,
                strUri + "/page:?",
                0,
                0,
                "data", // 但其实什么对象数据都不获取
                out byte[] baContent,
                out string strMetaData,
                out string strOutputPath,
                out byte[] baOutputTimeStamp,
                out string strError);
            return (int)lRet;
        }
        finally
        {
            sessioninfo.ReturnChannel(channel);
        }
    }

    void SetErrorInfo(string strText)
    {
        this.LabelErrorInfo.Text = strText;
        this.LabelErrorInfo.Visible = !string.IsNullOrEmpty(strText);
    }
}
