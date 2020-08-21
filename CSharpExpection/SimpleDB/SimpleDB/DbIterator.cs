using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDB
{
    public interface DbIterator
    {
        /**
         * Opens the iterator. This must be called before any of the other methods.
         * @throws DbException when there are problems opening/accessing the database.
         */
        void Open();

        /** Returns true if the iterator has more tuples.
         * @return true f the iterator has more tuples.
         * @throws IllegalStateException If the iterator has not been opened
       */
        bool HasNext();

        /**
         * Returns the next tuple from the operator (typically implementing by reading
         * from a child operator or an access method).
         *
         * @return the next tuple in the iteration.
         * @throws NoSuchElementException if there are no more tuples.
         * @throws IllegalStateException If the iterator has not been opened
         */
        Tuple Next();

        /**
         * Resets the iterator to the start.
         * @throws DbException when rewind is unsupported.
         * @throws IllegalStateException If the iterator has not been opened
         */
        void Rewind();

        /**
         * Returns the TupleDesc associated with this DbIterator. 
         * @return the TupleDesc associated with this DbIterator.
         */
        TupleDesc GetTupleDesc();

        /**
         * Closes the iterator. When the iterator is closed, calling next(),
         * hasNext(), or rewind() should fail by throwing IllegalStateException.
         */
        void Close();
    }
}
