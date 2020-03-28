using Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /// <summary>
    /// 一个需要手工Commit，支持同步/异步操作和Dispose的Repository，
    /// </summary>
    public interface IOrderClientRepository 
    {
        Task<List<OrderClient>> GetAllClientsAsync();

        List<OrderClient> GetAllClient();

        void AddClient(OrderClient client);
        void DeleteClient(int ClientID);
        void ModifyClient(OrderClient client);
    }
}
