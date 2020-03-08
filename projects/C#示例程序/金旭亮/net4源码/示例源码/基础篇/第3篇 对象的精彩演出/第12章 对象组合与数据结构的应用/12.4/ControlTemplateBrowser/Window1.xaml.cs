using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using System.Xml;
using System.Windows.Markup;


namespace ControlTemplateBrowser
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class Window1 : System.Windows.Window
    {

        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {                    
            Type controlType = typeof(Control);
            List<Type> derivedTypes = new List<Type>();

            // 获取Control类所在的程序集
            Assembly assembly = Assembly.GetAssembly(typeof(Control));
            foreach (Type type in assembly.GetTypes())
            {
                // 提取出所有WPF控件的公有成员
                if (type.IsSubclassOf(controlType) && !type.IsAbstract && type.IsPublic)
                {
                    derivedTypes.Add(type);
                }
            }
            
            // 按类型名排序
            derivedTypes.Sort(new TypeComparer());

            // 在ListBox中显示
            lstTypes.ItemsSource = derivedTypes;
        }

        private void lstTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            try
            {
                // 获取当前被选中的类型
                Type type = (Type)lstTypes.SelectedItem;                                            

                // 创建这一类型的实例
                ConstructorInfo info = type.GetConstructor(System.Type.EmptyTypes);
                Control control = (Control)info.Invoke(null);

                //将创建的控件实例加入到Grid中，但不显示它
                //只有将一个控件加入到逻辑树之后，才能访问到的它的模板
                control.Visibility = Visibility.Collapsed;
                grid.Children.Add(control);

                // 获取控件的模板
                ControlTemplate template = control.Template;

                // 将控件模板转换为Xaml代码
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                StringBuilder sb = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(sb, settings);
                XamlWriter.Save(template, writer);

                // 显示模板的XAML代码
                txtTemplate.Text = sb.ToString();

                // 移除创建的对象
                grid.Children.Remove(control);
            }
            catch (Exception err)
            {
                txtTemplate.Text = err.Message ;
            }
        }
    }

    public class TypeComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}