using System;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class ReaderRepository : BaseRepository<LibraryEntities>,
        IRepository<Reader>
    {
        public ReaderRepository(LibraryEntities dbContext) :
        base(dbContext)
        {

        }

        public ReaderRepository()
            : base(new LibraryEntities())
        {

        }
        public void AddClient(Reader client)
        {
            throw new NotImplementedException();
        }

        public void DeleteClient(int ClientID)
        {
            throw new NotImplementedException();
        }

        public List<Reader> GetAllClient()
        {
            return _dbContext.Readers.ToList();
        }

        public Task<List<Reader>> GetAllClientsAsync()
        {
            return _dbContext.Readers.ToListAsync();
        }

        public void ModifyClient(Reader client)
        {
            throw new NotImplementedException();
        }
    }
}
