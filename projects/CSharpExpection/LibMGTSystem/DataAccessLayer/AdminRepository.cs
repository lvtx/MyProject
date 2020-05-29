using LibraryModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace DataAccessLayer
{
    public class AdminRepository
        : BaseRepository<LibraryEntities>,
        IRepository<Admin>
    {
        public AdminRepository(LibraryEntities dbContext) :
            base(dbContext)
        {

        }

        public AdminRepository()
            : base(new LibraryEntities())
        {

        }
        public void AddClient(Admin client)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteClient(int ClientID)
        {
            throw new System.NotImplementedException();
        }

        public List<Admin> GetAllClient()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Admin>> GetAllClientsAsync()
        {
            throw new System.NotImplementedException();
        }

        public void ModifyClient(Admin client)
        {
            throw new System.NotImplementedException();
        }
    }
}
