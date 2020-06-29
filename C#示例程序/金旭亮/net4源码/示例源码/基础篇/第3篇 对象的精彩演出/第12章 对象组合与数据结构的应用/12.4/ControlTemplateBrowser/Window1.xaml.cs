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

            // ��ȡControl�����ڵĳ���
            Assembly assembly = Assembly.GetAssembly(typeof(Control));
            foreach (Type type in assembly.GetTypes())
            {
                // ��ȡ������WPF�ؼ��Ĺ��г�Ա
                if (type.IsSubclassOf(controlType) && !type.IsAbstract && type.IsPublic)
                {
                    derivedTypes.Add(type);
                }
            }
            
            // ������������
            derivedTypes.Sort(new TypeComparer());

            // ��ListBox����ʾ
            lstTypes.ItemsSource = derivedTypes;
        }

        private void lstTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            try
            {
                // ��ȡ��ǰ��ѡ�е�����
                Type type = (Type)lstTypes.SelectedItem;                                            

                // ������һ���͵�ʵ��
                ConstructorInfo info = type.GetConstructor(System.Type.EmptyTypes);
                Control control = (Control)info.Invoke(null);

                //�������Ŀؼ�ʵ�����뵽Grid�У�������ʾ��
                //ֻ�н�һ���ؼ����뵽�߼���֮�󣬲��ܷ��ʵ�������ģ��
                control.Visibility = Visibility.Collapsed;
                grid.Children.Add(control);

                // ��ȡ�ؼ���ģ��
                ControlTemplate template = control.Template;

                // ���ؼ�ģ��ת��ΪXaml����
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                StringBuilder sb = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(sb, settings);
                XamlWriter.Save(template, writer);

                // ��ʾģ���XAML����
                txtTemplate.Text = sb.ToString();

                // �Ƴ������Ķ���
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