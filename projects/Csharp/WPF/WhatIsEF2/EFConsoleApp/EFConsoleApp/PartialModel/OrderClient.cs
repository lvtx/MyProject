using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFConsoleApp
{
    partial class OrderClient : IEquatable<OrderClient>
    {
        public bool Equals(OrderClient other)
        {
            if (other != null)
            {
                return ClientName == other.ClientName
                && Address == other.Address
                && PostCode == other.PostCode
                && Telephone == other.Telephone
                && Email == other.Email;
            }
            else
            {
                return false;
            }
        }
    }
}
