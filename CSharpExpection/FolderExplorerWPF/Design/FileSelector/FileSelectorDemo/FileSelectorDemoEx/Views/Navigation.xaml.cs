using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileSelectorDemo.Views
{
    /// <summary>
    /// Navigation.xaml 的交互逻辑
    /// </summary>
    public partial class Navigation : UserControl
    {
        public Navigation()
        {
            InitializeComponent();
            Loaded += Navigation_Loaded;            
        }

        private void Navigation_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModels.NavigationViewModel(AttachedDataContext);
        }

        /// <summary>
        /// 当前FileList的DataContext对象
        /// </summary>
        public object AttachedDataContext
        {
            get { return (object)GetValue(AttachedDataContextProperty); }
            set { SetValue(AttachedDataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttachedDataContextProperty =
            DependencyProperty.Register("AttachedDataContext", typeof(object), typeof(Navigation), new PropertyMetadata(null));
        
    }
}
