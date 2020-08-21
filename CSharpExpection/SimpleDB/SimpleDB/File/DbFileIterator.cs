﻿/**
 * DbFileIterator is the iterator interface that all SimpleDB Dbfile should
 * implement.
 */
namespace SimpleDB
{
    public interface DbFileIterator
    {
        /**
        * Opens the iterator
        * @throws DbException when there are problems opening/accessing the database.
        */
        void Open();

        /** @return true if there are more tuples available, false if no more tuples or iterator isn't open. */
        bool HasNext();

        /**
         * Gets the next tuple from the operator (typically implementing by reading
         * from a child operator or an access method).
         *
         * @return The next tuple in the iterator.
         * @throws NoSuchElementException if there are no more tuples
         */
        Tuple Next();

        /**
         * Resets the iterator to the start.
         * @throws DbException When rewind is unsupported.
         */
        void Rewind();

        /**
         * Closes the iterator.
         */
        void Close();
    }
}
