using System;
using LibraryModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class RoleTypeRepository : BaseRepository<LibraryEntities>, IRepository<RoleType>
    {
        public RoleTypeRepository(LibraryEntities dbContext):base(dbContext){ }
        public RoleTypeRepository():base(new LibraryEntities()){ }
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
            return _dbContext.RoleType.ToList();
        }

        public Task<List<RoleType>> GetAllClientsAsync()
        {
            return _dbContext.RoleType.ToListAsync();
        }

        public void ModifyClient(RoleType client)
        {
            throw new NotImplementedException();
        }
    }
}
