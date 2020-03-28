using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class ReaderTypeRepository : BaseRepository<LibraryEntities>,
        IRepository<ReaderType>
    {
        public ReaderTypeRepository(LibraryEntities dbContext) :
        base(dbContext)
        {

        }

        public ReaderTypeRepository()
            : base(new LibraryEntities())
        {

        }
        public void AddClient(ReaderType client)
        {
            throw new NotImplementedException();
        }

        public void DeleteClient(int ClientID)
        {
            throw new NotImplementedException();
        }

        public List<ReaderType> GetAllClient()
        {
            return _dbContext.ReaderTypes.ToList();
        }

        public Task<List<ReaderType>> GetAllClientsAsync()
        {
            return _dbContext.ReaderTypes.ToListAsync();
        }

        public void ModifyClient(ReaderType client)
        {
            throw new NotImplementedException();
        }
    }
}
