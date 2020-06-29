using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /// <summary>
    /// 最简单的OrderRepository
    /// 在内部使用EF提取数据，向外界返回所需要的数据
    /// 所有返回给外界的数据，己经与EF和Database脱钩
    /// </summary>
    public interface ISimplestOrderClientRepository
    {
        List<OrderClient> GetAllClients();

        List<OrderClient> FindClientsByName(String FindWhat);

        int AddClient(OrderClient client);

        int DeleteClient(int ClientID);


        int ModifyClient(OrderClient client);
    }
}
