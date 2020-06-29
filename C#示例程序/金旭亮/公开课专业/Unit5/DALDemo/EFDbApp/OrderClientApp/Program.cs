using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowClients();
            Console.ReadKey();
        }

        static async Task ShowClients()
        {
            IOrderClientRepository repo = new OrderClientRepository();
            var clients =await repo.GetAllClientsAsync();
            foreach (var client in clients)
            {
                Console.WriteLine("{0}:{1}",client.ClientName,client.Address);
            }
        }
    }
}
