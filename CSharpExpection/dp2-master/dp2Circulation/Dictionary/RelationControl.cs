﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dp2Circulation
{
    /// <summary>
    /// 用于显示一个对照关系的控件
    /// </summary>
    public partial class RelationControl : UserControl
    {
        Color _titleBackColor = Color.LightGray;
        public Color TitleBackColor
        {
            get
            {
                return this._titleBackColor;
            }
            set
            {
                this._titleBackColor = value;
                this.Invalidate();
            }
        }

        // 标题字符串。用于指出变换方向
        string _titleText = "";
        public string TitleText
        {
            get
            {
                return this._titleText;
            }
            set
            {
                this._titleText = value;
                SetSize();
                this.Invalidate();
            }
        }

        bool _selected = false;
        public bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
                this.Invalidate();
            }
        }

        List<string> _hitCounts = new List<string>();
        public List<string> HitCounts
        {
            get
            {
                return this._hitCounts;
            }
            set
            {
                this._hitCounts = value;
                this.Invalidate();
            }
        }

        // 源分类号的字体高度
        int _sourceFontSize = 24;
        public int SourceFontSize
        {
            get
            {
                return this._sourceFontSize;
            }
            set
            {
                this._sourceFontSize = value;
                SetSize();
                this.Invalidate();
            }
        }

        // 加工以前的原始的 源分类号字符串
        string _sourceTextOrigin = "";
        public string SourceTextOrigin
        {
            get
            {
                return this._sourceTextOrigin;
            }
            set
            {
                this._sourceTextOrigin = value;
                SetSize();
                this.Invalidate();
            }
        }

        // 源分类号字符串
        string _sourceText = "";
        public string SourceText
        {
            get
            {
                return this._sourceText;
            }
            set
            {
                this._sourceText = value;
                SetSize();
                this.Invalidate();
            }
        }

        // 目标分类号字符串
        string _targetText = "";
        public string TargetText
        {
            get
            {
                return this._targetText;
            }
            set
            {
                this._targetText = value;
                SetSize();
                this.Invalidate();
            }
        }


        public RelationControl()
        {
            InitializeComponent();
        }

        // 上面部分是源分类号
        // 源分类号顶部有每个层级的命中数
        // 下面部分是选定的目标分类号
        private void RelationControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            Rectangle rectBack = new Rectangle(
0,
0,
this.ClientSize.Width,
this.ClientSize.Height);

            if (this._selected == true)
            {
                using (Brush brush = new SolidBrush(Color.LightBlue))
                {
                    e.Graphics.FillRectangle(brush, rectBack);
                }
            }

            if (this.Focused)
            {

                rectBack.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(e.Graphics,
                    rectBack);
            }
#if NO
                SizeF size = e.Graphics.MeasureString("M",
                    font,
                    100,
                    format);
#endif
            // 绘制标题行
            if (string.IsNullOrEmpty(this._titleText) == false)
            {
                using (Font font = GetTitleFont())
                {
                    StringFormat format = new StringFormat();
                    format.FormatFlags |= StringFormatFlags.FitBlackBox;
                    format.Alignment = StringAlignment.Near;

                    SizeF text_size = e.Graphics.MeasureString(this._titleText, font);

                    float x = this.Padding.Left;
                    float y = this.Padding.Top;
                    RectangleF textRect = new RectangleF(
    x,
    y,
    text_size.Width,
    text_size.Height);

                    using (Brush brush = new SolidBrush(this.TitleBackColor))
                    {
                        e.Graphics.FillRectangle(brush, textRect);
                    }
                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        e.Graphics.DrawString(
                            this._titleText,
                            font,
                            brush,
                            textRect,
                            format);
                    }
                }
            }

            SizeF size = GetSourceFontSize(e.Graphics);

            // 绘制命中数行
            if (this._hitCounts != null && this._hitCounts.Count > 0)
            {
                using (Font font = GetHitCountFont())
                using (Brush brush = new SolidBrush(Color.Gray))
                {
                    StringFormat format = new StringFormat();   //  (StringFormat)StringFormat.GenericTypographic.Clone();
                    format.FormatFlags |= StringFormatFlags.FitBlackBox;
                    format.Alignment = StringAlignment.Center;

                    float x = this.Padding.Left;
                    float y = this.Padding.Top + size.Height / 2;
                    foreach (string strText in this._hitCounts)
                    {
                        RectangleF textRect = new RectangleF(
    x,
    y,
    size.Width,
    size.Height);
                        e.Graphics.DrawString(
                            strText,
                            font,
                            brush,
                            textRect,
                            format);
                        x += size.Width;
                    }
                }
            }

            // 绘制源行
            if (string.IsNullOrEmpty(this._sourceText) == false)
            {
                using (Font font = GetSourceFont())
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    StringFormat format = new StringFormat();   //  (StringFormat)StringFormat.GenericTypographic.Clone();
                    format.FormatFlags |= StringFormatFlags.FitBlackBox;
                    format.Alignment = StringAlignment.Center;

                    string strSourceText = this.SourceText;
                    if (this._sourceTextOrigin.Length > strSourceText.Length)
                        strSourceText = this._sourceTextOrigin;

                    int i = 0;
                    float x = this.Padding.Left;
                    float y = this.Padding.Top + size.Height / 2 + size.Height / 2;
                    foreach (char ch in strSourceText)
                    {
                        RectangleF textRect = new RectangleF(
    x,
    y,
    size.Width,
    size.Height);
                        // TODO: 超过 sourcetext 的后面几个字符颜色要有所区别
                        e.Graphics.DrawString(
                            ch.ToString(),
                            font,
                            brush,
                            textRect,
                            format);
                        i++;
                        x += size.Width;
                    }
                }
            }

            // 绘制目标行
            if (string.IsNullOrEmpty(this._targetText) == false)
            {
                using (Font font = GetTargetFont())
                using (Brush brush = new SolidBrush(Color.DarkRed))  // Color.DarkBlue
                {
                    StringFormat format = new StringFormat();   //  (StringFormat)StringFormat.GenericTypographic.Clone();
                    format.FormatFlags |= StringFormatFlags.FitBlackBox | StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
                    format.Alignment = StringAlignment.Far;

                    float x = this.Padding.Left;
                    float y = this.Padding.Top + size.Height / 2 + size.Height / 2 + size.Height;
                    RectangleF textRect = new RectangleF(
    x,
    y,
    this.Width - this.Padding.Horizontal,
    size.Height);
                    e.Graphics.DrawString(
                        this._targetText,
                        font,
                        brush,
                        textRect,
                        format);
                }
            }
        }

        Font GetTitleFont()
        {
            return new System.Drawing.Font(this.Font.FontFamily,
                (float)this.SourceFontSize / 2,
                FontStyle.Bold,
                GraphicsUnit.Pixel);
        }

        Font GetSourceFont()
        {
            return new System.Drawing.Font(this.Font.FontFamily,
                (float)this.SourceFontSize, 
                FontStyle.Bold,
                GraphicsUnit.Pixel);
        }

        Font GetTargetFont()
        {
            return new System.Drawing.Font(this.Font.FontFamily,
                (float)this.SourceFontSize,
                FontStyle.Regular,
                GraphicsUnit.Pixel);
        }

        Font GetHitCountFont()
        {
            return new System.Drawing.Font(this.Font.FontFamily,
                (float)this.SourceFontSize/2,
                FontStyle.Regular,
                GraphicsUnit.Pixel);
        }

        // 得到源分类号的一个字符的尺寸
        SizeF GetSourceFontSize(Graphics g_param)
        {
            Graphics g = g_param;
            if(g == null)
                g = Graphics.FromHwnd(this.Handle);
            try {
                using(Font font = GetSourceFont())
                {
                    SizeF size = g.MeasureString("MMMMMMMMMM",
                    font);
                    size = new SizeF(size.Width / 10, size.Height);
                    return size;
                }
            }
            finally
            {
                if (g_param == null)
                    g.Dispose();
            }
        }

        // 设置控件尺寸
        void SetSize()
        {
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                SizeF size = GetSourceFontSize(g);

                // 目标文字像素宽度
                float fTargetTextWidth = 0;
                using (Font font = GetTargetFont())
                {
                    StringFormat format = new StringFormat();
                    format.FormatFlags |= StringFormatFlags.FitBlackBox | StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
                    format.Alignment = StringAlignment.Far;

                    fTargetTextWidth = g.MeasureString(this.TargetText,
                    font, 1000, format).Width;
                }

                // 源文字像素宽度
                string strSourceText = this.SourceText;
                if (this._sourceTextOrigin.Length > strSourceText.Length)
                    strSourceText = this._sourceTextOrigin;

                float fSourceTextWidth = size.Width * Math.Max(1, strSourceText.Length);

                int nWidth = (int)(Math.Max(fSourceTextWidth, fTargetTextWidth))
                    + this.Padding.Horizontal + 2;
                int nHeight = (int)(size.Height / 2) // title line
                    + (int)(size.Height / 2) // hitcount line
                    + (int)size.Height  // source line
                    // + (int)(size.Height / 2)    // blank
                    + (int)size.Height// target line
                    + this.Padding.Vertical;
                this.Size = new Size(nWidth, nHeight);
            }
        }

        private void RelationControl_FontChanged(object sender, EventArgs e)
        {
            SetSize();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.Invalidate();
        }

        // 绘制带有下划线的分类号文字
        public static void PaintSourceText(Graphics g,
            Font font,
            Color color,
            float x,
            float y,
            string strText,
            int nLevel)
        {
            using (Brush brush = new SolidBrush(color))
            {
                StringFormat format = new StringFormat();   //  (StringFormat)StringFormat.GenericTypographic.Clone();
                format.FormatFlags |= StringFormatFlags.FitBlackBox;
                format.Alignment = StringAlignment.Near;

                {
                    SizeF size = g.MeasureString(strText.Substring(0, nLevel), font);

                    RectangleF textRect = new RectangleF(
    x,
    y,
    size.Width,
    size.Height);
                    float fPenWidth = 3.0F;
                    using (Pen pen = new Pen(Color.Green, fPenWidth))
                    {
                        g.DrawLine(pen,
                            new PointF(textRect.X, textRect.Y + textRect.Height - fPenWidth),
                            new PointF(textRect.X + textRect.Width, textRect.Y + textRect.Height - fPenWidth));
                    }
                }

                {
                    SizeF size = g.MeasureString(strText, font);

                    RectangleF textRect = new RectangleF(
    x,
    y,
    size.Width,
    size.Height);
                    g.DrawString(
                        strText,
                        font,
                        brush,
                        textRect,
                        format);
                }
            }
        }

        // 绘制分类号文字的下划线部分
        // parameters:
        //      x,y 指向文字矩形的左下角位置
        public static void PaintSourceTextUnderline(Graphics g,
            Font font,
            Color color,
            float x,
            float y,
            string strText,
            int nLevel)
        {
            if (nLevel < 0 || nLevel > strText.Length)
                throw new ArgumentException("nLevel 值 "+nLevel.ToString()+" 不应越过 strText '"+strText+"' 内容字符数");

            SizeF size = g.MeasureString(strText.Substring(0, nLevel), font);

            float fPenWidth = 3.0F;
            RectangleF textRect = new RectangleF(
x,
y - fPenWidth,
size.Width,
fPenWidth);
            using (Pen pen = new Pen(color, fPenWidth))
            {
                g.DrawLine(pen,
                    new PointF(textRect.X, textRect.Y + textRect.Height / 2),
                    new PointF(textRect.X + textRect.Width, textRect.Y + textRect.Height / 2));
            }
        }

    }
}
