using System;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class BookInfoRepository : BaseRepository<LibraryEntities>,
        IRepository<BookInfo>
    {
        public BookInfoRepository(LibraryEntities dbContext) :
            base(dbContext)
        {

        }

        public BookInfoRepository()
            : base(new LibraryEntities())
        {

        }

        public Task<List<BookInfo>> GetAllClientsAsync()
        {
            return _dbContext.BookInfoes.ToListAsync();
        }

        public List<BookInfo> GetAllClient()
        {
            return _dbContext.BookInfoes.ToList();
        }

        public void AddClient(BookInfo client)
        {
            throw new NotImplementedException();
        }

        public void DeleteClient(int ClientID)
        {
            throw new NotImplementedException();
        }

        public void ModifyClient(BookInfo client)
        {
            throw new NotImplementedException();
        }
    }
}
