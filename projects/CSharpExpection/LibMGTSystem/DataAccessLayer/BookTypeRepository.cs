using System;
using LibraryModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Data.Entity;
using System.Runtime.CompilerServices;

namespace DataAccessLayer
{
    public class BookTypeRepository : BaseRepository<LibraryEntities>, IRepository<BookType>

    {
        public BookTypeRepository(LibraryEntities dbContext) :
        base(dbContext)
        {

        }

        public BookTypeRepository()
            : base(new LibraryEntities())
        {

        }

        //public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<BookType> _bookTypes;

        public ObservableCollection<BookType> BookTypes
        {
            get 
            {
                _bookTypes = new ObservableCollection<BookType>();
                foreach (var bookType in GetAllClient())
                {
                    _bookTypes.Add(bookType);
                }
                return _bookTypes; 
            }
            //set 
            //{
            //    //这里可以改为TPL
            //    //将从数据库中获取的List类型转为ObservableCollection
            //    if (_bookTypes != null)
            //    {
            //        SetProperty(ref _bookTypes, value);
            //    }
            //}
        }
        //protected void SetProperty<T>(ref T field, T value,
        //[CallerMemberName] string propName = null)
        //{
        //    if (!EqualityComparer<T>.Default.Equals(field, value))
        //    {
        //        field = value;
        //        var pc = PropertyChanged;
        //        if (pc != null)
        //            pc(this, new PropertyChangedEventArgs(propName));
        //    }
        //}
        public void AddClient(BookType bookType)
        {
            if (bookType != null)
            {
                _dbContext.BookType.Add(bookType);
            }
        }

        public async void DeleteClient(int ClientID)
        {
            var client = await _dbContext.BookType.FindAsync(ClientID);
            if(client != null)
            {
                _dbContext.BookType.Remove(client);
            }
        }

        public void DeleteClient(BookType Client)
        {
            if(Client != null)
            {
                _dbContext.BookType.Remove(Client);
            }
        }

        public List<BookType> GetAllClient()
        {
            return _dbContext.BookType.ToList();
        }

        public Task<List<BookType>> GetAllClientsAsync()
        {
            return _dbContext.BookType.ToListAsync();
        }
        /// <summary>
        /// 对数据库中已有的书籍类型进行修改
        /// </summary>
        /// <param name="bookType"></param>
        public async void ModifyClient(BookType bookType)
        {
            var temp = await _dbContext.BookType.FindAsync(bookType.BookTypeId);
            if (temp != null)
            {
                temp.BookTypeName = bookType.BookTypeName;
                temp.Description = bookType.Description;
            }
        }
    }
}
