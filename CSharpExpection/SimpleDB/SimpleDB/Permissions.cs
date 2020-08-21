using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDB
{
    public class Permissions
    {
        int permLevel;

        private Permissions(int permLevel)
        {
            this.permLevel = permLevel;
        }

        public override String ToString()
        {
            if (permLevel == 0)
                return "READ_ONLY";
            if (permLevel == 1)
                return "READ_WRITE";
            return "UNKNOWN";
        }

        public static readonly Permissions READ_ONLY = new Permissions(0);
        public static readonly Permissions READ_WRITE = new Permissions(1);
    }
}
