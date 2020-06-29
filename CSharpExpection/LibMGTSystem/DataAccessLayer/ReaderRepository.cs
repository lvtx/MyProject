using LibraryModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ReaderRepository : BaseRepository<LibraryEntities>, IRepository<Reader>
    {
        public ReaderRepository(LibraryEntities dbContext) : base(dbContext){ }
        public ReaderRepository():base(new LibraryEntities()) { }
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
            return _dbContext.Reader.ToList();
        }

        public Task<List<Reader>> GetAllClientsAsync()
        {
            return _dbContext.Reader.ToListAsync();
        }

        public void ModifyClient(Reader client)
        {
            throw new NotImplementedException();
        }
    }
}
