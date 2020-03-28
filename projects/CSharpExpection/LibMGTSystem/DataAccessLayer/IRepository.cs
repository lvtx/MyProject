using Model;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /// <summary>
    /// 一个需要手工Commit，支持同步/异步操作和Dispose的Repository，
    /// </summary>
    public interface IRepository<T>
    {        
        Task<List<T>> GetAllClientsAsync();

        List<T> GetAllClient();

        void AddClient(T client);
        void DeleteClient(int ClientID);
        void ModifyClient(T client);
    }
}
