using LibraryModel;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp
{
    public interface IInfo<T>
    {
        ObservableCollection<T> GetInfos(int page);
        ObservableCollection<T> GetInfos();
    }
    public class BookFactory : IInfo<BookInfo>
    {
        private BookType bookType;
        public BookFactory(BookType bookType)
        {
            this.bookType = bookType;
        }

        public ObservableCollection<BookInfo> GetInfos(int page)
        {
            var bookInfos = bookType.BookInfo.Take(20 * page).Skip(20 * (page - 1));
            var rets = new ObservableCollection<BookInfo>(bookInfos);
            return rets;
        }
        public ObservableCollection<BookInfo> GetInfos()
        {
            var bookInfos = bookType.BookInfo;
            var rets = new ObservableCollection<BookInfo>(bookInfos);
            return rets;
        }
    }

    public class ReaderFactory : IInfo<Reader>
    {
        private RoleType readerType;
        public ReaderFactory(RoleType readerType)
        {
            this.readerType = readerType;
        }
        public ObservableCollection<Reader> GetInfos(int page)
        {
            var readers = readerType.SReader.Take(20 * page).Skip(20 * (page - 1));
            var rets = new ObservableCollection<Reader>(readers);
            return rets;
        }
        public ObservableCollection<Reader> GetInfos()
        {
            var readers = readerType.SReader;
            var rets = new ObservableCollection<Reader>(readers);
            return rets;
        }
    }
}
