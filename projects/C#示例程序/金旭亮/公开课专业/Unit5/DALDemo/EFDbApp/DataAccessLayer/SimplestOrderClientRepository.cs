using Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class SimplestOrderClientRepository : ISimplestOrderClientRepository
    {
        public List<OrderClient> GetAllClients()
        {
            using (var context = new MyDBEntities())
            {
                return context.OrderClients.ToList();

            }
        }

        public int AddClient(OrderClient client)
        {
            if (client == null)
            {
                return 0;
            }
            using (var context = new MyDBEntities())
            {
                context.OrderClients.Add(client);
                return context.SaveChanges();

            }

        }

        public int DeleteClient(int ClientID)
        {
            using (var context = new MyDBEntities())
            {
                var client = context.OrderClients.Find(ClientID);
                if (client != null)
                {
                    context.OrderClients.Remove(client);
                    return context.SaveChanges();
                }
                else
                {
                    return 0;
                }

            }
        }

        public int ModifyClient(OrderClient client)
        {
            if (client == null)
            {
                return 0;
            }
            using (var context = new MyDBEntities())
            {
                var tempClient = context.OrderClients.FirstOrDefault(c => c.ClientID == client.ClientID);
                if (tempClient != null)
                {
                    tempClient.Address = client.Address;
                    tempClient.ClientName = client.ClientName;
                    tempClient.Email = client.Email;
                    tempClient.PostCode = client.PostCode;
                    tempClient.Telephone = client.Telephone;

                    return context.SaveChanges();
                }
                else
                {
                    return 0;
                }

            }
        }


        public List<OrderClient> FindClientsByName(string FindWhat)
        {
            using (var context = new MyDBEntities())
            {
                return context.OrderClients.Where(c =>
                    c.ClientName.StartsWith(FindWhat)).ToList();
            }
        }
    }
}
