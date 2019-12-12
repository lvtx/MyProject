using System;

namespace CRUD
{
    /// <summary>
    /// 使用分部类，修改数据实体类
    /// </summary>
    public partial class OrderClient : IEquatable<OrderClient>
    {
        public override string ToString()
        {
            return String.Format("{0}:{1}:{2}",ClientID, ClientName, Address);
        }


        public bool Equals(OrderClient other)
        {
            if (other == null)
            {
                return false;
            }
            return ClientName == other.ClientName
                && Address == other.Address
                && PostCode == other.PostCode
                && Telephone == other.Telephone
                && Email == other.Email;
        }
    }

}
