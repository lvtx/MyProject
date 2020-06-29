using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrawSelectRectangle
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        //是否处于鼠标拖动选择状态
        private bool InDragSelectMode = false;
        //用于显示拖动选择虚线框的矩形对象
        private Rectangle DragSelectRect = null;

        //本次MouseMove事件发生时鼠标的前一点位置（即前一次MouseMove事件激发时的鼠标当前位置）
        private Point PrevP;

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

            Point curP = e.GetPosition(sender as IInputElement);
            //计算坐标偏移量
            double deltaX = curP.X - PrevP.X;
            double deltaY = curP.Y - PrevP.Y;
            if (InDragSelectMode)
                txtInfo.Text = string.Format("当前坐标：({0},{1})--坐标偏移量：({2},{3})", curP.X, curP.Y, deltaX, deltaY);
            else
                txtInfo.Text = string.Format("当前坐标：({0},{1})", curP.X, curP.Y);


            if (DragSelectRect == null)
                return;
            //鼠标向左上角移动
            if ((deltaX < 0) && (deltaY < 0))
            {
                //重设矩型的绘制起点
                DragSelectRect.SetValue(Canvas.LeftProperty, curP.X);
                DragSelectRect.SetValue(Canvas.TopProperty, curP.Y);
            }
            //鼠标向右上角移动
            if ((deltaX > 0) && (deltaY < 0))
            {
                DragSelectRect.SetValue(Canvas.LeftProperty, PrevP.X);
                DragSelectRect.SetValue(Canvas.TopProperty, curP.Y);
            }

            //鼠标向左上角移动
            if ((deltaX < 0) && (deltaY > 0))
            {
                DragSelectRect.SetValue(Canvas.LeftProperty, curP.X);
                DragSelectRect.SetValue(Canvas.TopProperty, PrevP.Y);
            }
            //根据当前鼠标位置，设置矩形的高度和宽度
            DragSelectRect.Width = Math.Abs(deltaX);
            DragSelectRect.Height = Math.Abs(deltaY);

        }





        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //进入鼠标拖动选择状态
            InDragSelectMode = true;
            //设置当前坐标点为第一个“起始点”
            PrevP = e.GetPosition(sender as IInputElement);

            //创建“选择虚线框”对象
            DragSelectRect = new Rectangle();
            //加入到绘图面板中以便显示
            DrawPanel.Children.Add(DragSelectRect);
            //设置“选择虚线框”的左上角位置
            DragSelectRect.SetValue(Canvas.LeftProperty, PrevP.X);
            DragSelectRect.SetValue(Canvas.TopProperty, PrevP.Y);
            //设置“选择虚线框”的边框线为点划线
            DragSelectRect.Stroke = Brushes.Black;
            DragSelectRect.StrokeThickness = 2;
            DragSelectRect.StrokeDashArray = new DoubleCollection { 1, 2 };
            //给它一个最小的高度和宽度以便显示
            DragSelectRect.Width = 1;
            DragSelectRect.Height = 1;

        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //离开鼠标拖动选择状态
            InDragSelectMode = false;
            //从画布中移除“选择虚线框”
            DrawPanel.Children.Remove(DragSelectRect);

            DragSelectRect = null;

        }
    }
}
