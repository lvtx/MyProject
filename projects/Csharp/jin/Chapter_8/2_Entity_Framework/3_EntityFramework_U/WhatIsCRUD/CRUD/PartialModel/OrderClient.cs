using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD
{
    public partial class OrderClient:IEquatable<OrderClient>
    {
        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}", ClientID, ClientName, Address);
        }
        public bool Equals(OrderClient other)
        {
            if (other == null)
            {
                return false;
            }
            else
                return
                other.ClientName == ClientName &&
                other.Address == Address &&
                other.PostCode == PostCode &&
                other.Telephone == Telephone &&
                other.Email == Email;
        }
    }
}
