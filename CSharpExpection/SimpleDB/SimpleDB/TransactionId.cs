using System;

namespace SimpleDB
{
    /**
	 * TransactionId is a class that contains the identifier of a transaction.
	 */
    public class TransactionId
    {


        private static readonly long serialVersionUID = 1L;

        static Int64 counter = new Int64();
        readonly long myid;

        public TransactionId()
        {
            myid = counter++;
        }

        public long GetId()
        {
            return myid;
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;
            TransactionId other = (TransactionId)obj;
            if (myid != other.myid)
                return false;
            return true;
        }

        public int HashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + (int)(myid ^ (myid >> 32));
            return result;
        }
    }
}
