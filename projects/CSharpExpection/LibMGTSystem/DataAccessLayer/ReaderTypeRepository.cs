using LibraryModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ReaderTypeRepository : BaseRepository<LibraryEntities>, IRepository<RoleType>
    {
        public ReaderTypeRepository() : base(new LibraryEntities()) { }
        public ReaderTypeRepository(LibraryEntities dbcontext) : base(dbcontext) { }
        public void AddClient(RoleType client)
        {
            throw new NotImplementedException();
        }

        public void DeleteClient(int ClientID)
        {
            throw new NotImplementedException();
        }

        public List<RoleType> GetAllClient()
        {
            return _dbContext.GetReaderType().ToList();
            //List<RoleType> retList = null;
            //foreach(var readerType in readerTypes)
            //{
            //    var ret = _dbContext.RoleType.Find(readerType.RoleTypeId);
            //    retList.Add(ret);
            //}
            //return retList;
        }

        public Task<List<RoleType>> GetAllClientsAsync()
        {
            return _dbContext.GetReaderType().ToListAsync();
        }

        public void ModifyClient(RoleType client)
        {
            throw new NotImplementedException();
        }
    }
}
