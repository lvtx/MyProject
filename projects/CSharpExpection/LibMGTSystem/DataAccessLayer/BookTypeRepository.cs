using System;
using LibraryModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

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
        public void AddClient(BookType client)
        {
            
        }

        public void DeleteClient(int ClientID)
        {
            throw new NotImplementedException();
        }

        public List<BookType> GetAllClient()
        {
            return _dbContext.BookType.ToList();
        }

        public Task<List<BookType>> GetAllClientsAsync()
        {
            return _dbContext.BookType.ToListAsync();
        }

        public void ModifyClient(BookType client)
        {
            throw new NotImplementedException();
        }
    }
}
