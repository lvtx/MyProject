using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDB
{
	public class Filter : Operator
	{

		private const long serialVersionUID = 1L;

		/// <summary>
		/// Constructor accepts a predicate to apply and a child operator to read
		/// tuples to filter from.
		/// </summary>
		/// <param name="p">
		///            The predicate to filter tuples with </param>
		/// <param name="child">
		///            The child operator </param>
		public Filter(Predicate p, DbIterator child)
		{
			// some code goes here
		}

		public virtual Predicate Predicate
		{
			get
			{
				// some code goes here
				return null;
			}
		}

		public virtual TupleDesc TupleDesc
		{
			get
			{
				// some code goes here
				return null;
			}
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public void open() throws DbException, NoSuchElementException, TransactionAbortedException
		public virtual void open()
		{
			// some code goes here
		}

		public virtual void close()
		{
			// some code goes here
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public void rewind() throws DbException, TransactionAbortedException
		public virtual void rewind()
		{
			// some code goes here
		}

		/// <summary>
		/// AbstractDbIterator.readNext implementation. Iterates over tuples from the
		/// child operator, applying the predicate to them and returning those that
		/// pass the predicate (i.e. for which the Predicate.filter() returns true.)
		/// </summary>
		/// <returns> The next tuple that passes the filter, or null if there are no
		///         more tuples </returns>
		/// <seealso cref= Predicate#filter </seealso>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: protected Tuple fetchNext() throws NoSuchElementException, TransactionAbortedException, DbException
		protected internal virtual Tuple fetchNext()
		{
			// some code goes here
			return null;
		}

		public override DbIterator[] Children
		{
			get
			{
				// some code goes here
				return null;
			}
			set
			{
				// some code goes here
			}
		}


	}
}
