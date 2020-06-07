using DataAccessLayer;
using LibraryModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Specialized;
using System.Threading;
using System.Windows.Data;

namespace LibraryApp
{
    /// <summary>
    /// Interaction logic for ModifyBookType.xaml
    /// </summary>
    public partial class ModifyBookType : Window
    {
        //记录新添加元素的索引
        List<int> AddElesIndex = new List<int>();
        //被删除元素的索引
        //int DeleteEleIndex = 0;
        //记录被修改的行的索引
        //List<int> ModifedElesIndex = new List<int>();
        //记录初始的所有行的索引
        //List<int> AllIndex = new List<int>();
        //测试用，删除标记
        BookTypeRepository bookTypeEntity = new BookTypeRepository();
        ObservableCollection<BookType> bookTypes = null;

        public ModifyBookType()
        {
            InitializeComponent();
            InitializeTxtStatus();
            #region "测试用"
            bookTypes = bookTypeEntity.BookTypes;
            dgrdShowBookType.ItemsSource = bookTypes;
            bookTypes.CollectionChanged += BookTypes_CollectionChanged;
            #endregion
        }
        /// <summary>
        /// 将TextBlock与Status绑定
        /// </summary>
        private void InitializeTxtStatus()
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Status");
            txtStatus.SetBinding(TextBlock.TextProperty, binding);
        }
        //此事件发生在New_Executed命令之前
        //AddEleIndex没有被赋值为DataGrid被选中行的索引值
        //依然保持为上一个被选中行的索引值
        private void BookTypes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {            
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //当New_Executed中的bookTypes.Add被触发之后
                //执行try中的语句
                //使用Thread.Sleep(3000)测试了一下
                //New_Executed中剩下的语句会等待try中的语句
                //执行完毕后继续执行
                try
                {
                    //更新datagrid绑定的对象
                    //dgrdShowBookType.ItemsSource = bookTypes;
                    //将当前最后一个元素设置为当前元素
                    dgrdShowBookType.Items.MoveCurrentToLast();
                    //滚动到最后一个元素
                    dgrdShowBookType.ScrollIntoView(dgrdShowBookType.Items.CurrentItem);
                    //选中当前的元素
                    dgrdShowBookType.SelectedItem = dgrdShowBookType.Items.CurrentItem;
                    BookType newBookType = dgrdShowBookType.SelectedItem as BookType;
                    //新书籍类型加入本地缓存
                    bookTypeEntity.AddClient(newBookType);

                    Status = "已添加一个新的书籍类型";
                    Console.WriteLine(Status);
                    Console.WriteLine("一个Item添加到BookTypes");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);                   
                }             
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Status = "已删除";
            }
        }

        public void ShowBookType(ObservableCollection<BookType> _bookTypes)
        {
            dgrdShowBookType.ItemsSource = _bookTypes;
        }

        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter.ToString() == "BookType")
            {
                BookType bookType = new BookType()
                { BookTypeName = "新书籍类型", Description = "新书籍类型的描述" };
                bookTypes.Add(bookType);
                //记录新添加元素的HashCode
                int index = bookType.GetHashCode();
                //Console.WriteLine("新添加元素的索引为:{0}",index);
                Console.WriteLine("新添加元素的HashCode为:{0}",bookType.GetHashCode());
                AddElesIndex.Add(index);
            }
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            
        }
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter.ToString() == "BookType")
            {
                BookType item = (BookType)dgrdShowBookType.SelectedItem;
                try
                {
                    if (item != null)
                    {
                        //DeleteEleIndex = bookTypes.IndexOf(item);
                        int index = item.GetHashCode();
                        if (AddElesIndex.Contains(index))
                        {
                            AddElesIndex.Remove(index);
                            bookTypeEntity.DeleteClient(item);
                            //Console.WriteLine("从集合中移除");
                        }
                        else
                        {
                            bookTypeEntity.DeleteClient(item.BookTypeId);
                            //Console.WriteLine("从本地缓存中移除");
                        }
                        //if (ModifedElesIndex.Contains(index))
                        //{
                        //    ModifedElesIndex.Remove(index);
                        //}
                        bookTypes.Remove(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }              
            }
        }
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //if (AddElesIndex != null)
            //{
            //    foreach (var index in AddElesIndex)
            //    {
            //        BookType newbookType = bookTypes[index];
            //        bookTypeEntity.AddClient(newbookType);
            //    }
            //}
            //if (ModifedElesIndex != null)
            //{
            //    foreach (var index in ModifedElesIndex)
            //    {
            //        BookType bookType = bookTypes[index];
            //        bookTypeEntity.ModifyClient(bookType);
            //    }
            //}
            Task<int> t = bookTypeEntity.SaveChangesAsync();
        }

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string),
                typeof(ModifyBookType), new PropertyMetadata(" "));

        private void dgrdShowBookType_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var item = (dgrdShowBookType.SelectedItem as BookType);
            if (item != null)
            {
                string text = string.Format("选中类型为:{0}", item.BookTypeName);
                Status = text;
            }
        }

        //本来是为Cell中数据修改用的
        //但是修改完数据后并不需要做过多的处理
        //这里有部分关于Cell使用的方法
        //private string CellPreValue;
        //private int hashCode;
        //private int index;
        //private void dgrdShowBookType_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        //{
        //    string preValue = null;
        //    var cell = dgrdShowBookType.CurrentCell;
        //    //获取单元格修改前的原值
        //    preValue = (cell.Column.GetCellContent(cell.Item) as TextBox).Text;
        //    //将返回值存储在preValue中
        //    CellPreValue = preValue.Trim();
        //    //获取选中单元格在bookTypes中的索引位置
        //    hashCode = (e.Row.Item as BookType).GetHashCode();
        //    index = bookTypes.IndexOf((e.Row.Item as BookType));
        //}
        //private void dgrdShowBookType_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        //{
        //    //当按下Enter键判定为提交，按下Esc判定为取消
        //    //还需要自己实现当Cell中的值不变时按下Enter键的判定
        //    //Console.WriteLine("原值为{0}", CellPreValue);

        //    string CellCurrentValue = ((TextBox)e.EditingElement).Text.Trim();
        //    bool IsEqual = CellPreValue.Equals(CellCurrentValue);
        //    Console.WriteLine((e.Column.Header as TextBox).Text.Trim());
        //    if (e.EditAction == DataGridEditAction.Cancel || IsEqual == true)
        //    {
        //        //好像这里取消之后会不断地触发dgrdShowBookType_CellEditEnding事件
        //        //dgrdShowBookType.BeginEdit();
        //        //dgrdShowBookType.CancelEdit();
        //        //发送一个ESC按键(WinForm)
        //        //SendKeys.Send("{ESC} ");
        //        //试图将误输入空格的值恢复为原值(失败了)
        //        //bookTypes[eIndex].BookTypeName = CellPreValue;
        //        Console.WriteLine("取消修改{0}");
        //    }
        //    else
        //    {
        //        //如果这个修改的单元格是原来存在的
        //        //就把它加入修改元素中
        //        try
        //        {
        //            //获取原值的两种方法
        //            //1.var bookType = (e.Row.Item as BookType);
        //            //Console.WriteLine(bookType.BookTypeName);
        //            //Console.WriteLine(bookType.Description);
        //            //2.(dgrdShowBookType.SelectedItem as BookType)

        //            if (AddElesIndex.Contains(hashCode))
        //            {
        //                Console.WriteLine("新添加的行");
        //                //BookType bookType = (e.Row.Item as BookType);
        //                //bookTypeEntity.ModifyClient(bookType);
        //            }
        //            else
        //            {
        //                Console.WriteLine("已存在的行");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);                  
        //        }
        //        Console.WriteLine("修改后的值:{0}", CellCurrentValue);
        //    }

        //    //Cell修改后的值
        //    //原值         
        //    //Console.WriteLine("原值 {0}",(e.Row.Item as BookType).BookTypeName);
        //    //获取
        //    //Cell修改后的值,这是其中一种方法
        //    //var cells = ((DataGrid)sender).SelectedCells;
        //    //if (cells.Any())
        //    //{
        //    //    var cell = cells.First();
        //    //    Console.WriteLine(((TextBox)(cell.Column.GetCellContent(cell.Item))).Text);
        //    //}
        //}
    }
}
