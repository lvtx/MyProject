using LibraryModel;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace LibraryApp
{
    public class Searcher
    {
        protected IEnumerable source;
        protected ICollectionView dgrdView;
        protected string keyWord;
        public Searcher(IEnumerable source, string keyWord)
        {
            this.source = source;
            dgrdView = CollectionViewSource.GetDefaultView(source);
            this.keyWord = keyWord;
        }
        public Searcher(){ }
        protected void ResetTextBox()
        {
            if (dgrdView == null)
                return;
            if ((keyWord.Trim().Length == 0))
            {
                dgrdView.Filter = null;
                return;
            }
        }
        public virtual void Search() { }
    }
    /// <summary>
    /// ISBN搜索类
    /// </summary>
    public class SearchISBN : Searcher
    {
        public SearchISBN(IEnumerable source, string keyWord) 
            :base(source, keyWord) { }
        public SearchISBN(){ }
        public override void Search()
        {
            ResetTextBox();
            dgrdView.Filter = (item) =>
            {
                BookInfo bookInfo = item as BookInfo;
                if (bookInfo != null && bookInfo.ISBN.ToString()
                .IndexOf(keyWord.Trim()) != -1)
                {
                    return true;
                }
                else
                    return false;
            };
        }
    }

    /// <summary>
    /// BookName搜索类
    /// </summary>
    public class SearchBookName : Searcher
    {
        public SearchBookName(IEnumerable source, string keyWord)
            : base(source, keyWord)
        {

        }
        public SearchBookName(){ }
        public override void Search()
        {
            ResetTextBox();
            dgrdView.Filter = (item) =>
            {
                BookInfo bookInfo = item as BookInfo;
                if (bookInfo != null && bookInfo.BookName.IndexOf(keyWord.Trim()) != -1)
                {
                    return true;
                }
                else
                    return false;
            };
        }
    }
    public class SearchAuthor : Searcher
    {
        public SearchAuthor(IEnumerable source, string keyWord)
            : base(source, keyWord)
        {

        }
        public SearchAuthor() { }
        public override void Search()
        {
            ResetTextBox();
            dgrdView.Filter = (item) =>
            {
                BookInfo bookInfo = item as BookInfo;
                if (bookInfo != null && bookInfo.Author.IndexOf(keyWord.Trim()) != -1)
                {
                    return true;
                }
                else
                    return false;
            };
        }
    }

    public class SearchPageNumber : Searcher
    {
        public SearchPageNumber(IEnumerable source, string keyWord)
            : base(source, keyWord) { }
        public SearchPageNumber() { }
        public override void Search()
        {
            ResetTextBox();
            dgrdView.Filter = (item) =>
            {
                BookInfo bookInfo = item as BookInfo;
                if (bookInfo != null && bookInfo.PageNumber.IndexOf(keyWord.Trim()) != -1)
                {
                    return true;
                }
                else
                    return false;
            };
        }
    }

    public class SearchPrice : Searcher
    {
        public SearchPrice(IEnumerable source, string keyWord)
            : base(source, keyWord){ }
        public SearchPrice() { }
        public override void Search()
        {
            ResetTextBox();
            dgrdView.Filter = (item) =>
            {
                BookInfo bookInfo = item as BookInfo;
                if (bookInfo != null && bookInfo.Price.IndexOf(keyWord.Trim()) != -1)
                {
                    return true;
                }
                else
                    return false;
            };
        }
    }
    public class SearchReaderId : Searcher
    {
        public SearchReaderId(IEnumerable source, string keyWord)
            : base(source, keyWord) { }
        public SearchReaderId() { }
        public override void Search()
        {
            ResetTextBox();
            dgrdView.Filter = (item) =>
            {
                Reader reader = item as Reader;
                if (reader != null && reader.ReaderId.IndexOf(keyWord.Trim()) != -1)
                {
                    return true;
                }
                else
                    return false;
            };
        }
    }
    /// <summary>
    /// 搜索工厂类
    /// </summary>
    public class SearchFactory
    {
        /// <summary>
        /// 工厂类中的搜索方法
        /// </summary>
        /// <param name="source">Binding的源</param>
        /// <param name="item">需要查询的项目</param>
        /// <param name="keyWord">搜索的关键字</param>
        public void Search(IEnumerable source,string item,string keyWord)
        {
            Searcher searcher = new Searcher();
            switch (item)
            {
                case "ISBN":
                    searcher = new SearchISBN(source, keyWord);
                    searcher.Search();
                    break;
                case "图书名称":
                    searcher = new SearchBookName(source, keyWord);
                    searcher.Search();
                    break;
                case "作者":
                    searcher = new SearchAuthor(source, keyWord);
                    searcher.Search();
                    break;
                case "页数":
                    searcher = new SearchPageNumber(source, keyWord);
                    searcher.Search();
                    break;
                case "价格":
                    searcher = new SearchPrice(source, keyWord);
                    searcher.Search();
                    break;
                case "读者编号":
                    break;
                default:
                    break;
            }
        }
    }
}


