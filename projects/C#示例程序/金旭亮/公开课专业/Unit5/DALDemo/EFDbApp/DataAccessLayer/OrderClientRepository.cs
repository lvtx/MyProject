using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class OrderClientRepository
        : BaseRepository<MyDBEntities>,
        IOrderClientRepository
    {
        public OrderClientRepository(MyDBEntities dbContext) :
            base(dbContext)
        {

        }

        public OrderClientRepository()
            : base(new MyDBEntities())
        {

        }

        public Task<List<OrderClient>> GetAllClientsAsync()
        {
            return _dbContext.OrderClients.ToListAsync();
        }

        public List<OrderClient> GetAllClient()
        {
            return _dbContext.OrderClients.ToList();

        }

        public void AddClient(OrderClient client)
        {
            if (client != null)
            {
                _dbContext.OrderClients.Add(client);
            }
        }

        public async void DeleteClient(int ClientID)
        {
            var client = await _dbContext.OrderClients.FindAsync(ClientID);
            if (client != null)
            {
                _dbContext.OrderClients.Remove(client);
            }
        }

        public async void ModifyClient(OrderClient client)
        {
            var tempClient = await _dbContext.OrderClients.FindAsync(client.ClientID);
            if (tempClient != null)
            {
                tempClient.Address = client.Address;
                tempClient.ClientName = client.ClientName;
                tempClient.Email = client.Email;
                tempClient.PostCode = client.PostCode;
                tempClient.Telephone = client.Telephone;
            }
        }
    }
}
