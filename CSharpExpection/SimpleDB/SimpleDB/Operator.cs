using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDB
{
	/// <summary>
	/// Abstract class for implementing operators. It handles <code>close</code>,
	/// <code>next</code> and <code>hasNext</code>. Subclasses only need to implement
	/// <code>open</code> and <code>readNext</code>.
	/// </summary>
	public abstract class Operator : DbIterator
	{

		private const long serialVersionUID = 1L;

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public boolean hasNext() throws DbException, TransactionAbortedException
		public virtual bool HasNext()
		{
			if (!this.open_Conflict)
			{
				throw new System.InvalidOperationException("Operator not yet open");
			}

			if (next_Conflict == null)
			{
				next_Conflict = FetchNext();
			}
			return next_Conflict != null;
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public Tuple next() throws DbException, TransactionAbortedException, java.util.NoSuchElementException
		public virtual Tuple Next()
		{
			if (next_Conflict == null)
			{
				next_Conflict = FetchNext();
				if (next_Conflict == null)
				{
					throw new NullReferenceException();
				}
			}

			Tuple result = next_Conflict;
			next_Conflict = null;
			return result;
		}

		/// <summary>
		/// Returns the next Tuple in the iterator, or null if the iteration is
		/// finished. Operator uses this method to implement both <code>next</code>
		/// and <code>hasNext</code>.
		/// </summary>
		/// <returns> the next Tuple in the iterator, or null if the iteration is
		///         finished. </returns>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: protected abstract Tuple fetchNext() throws DbException, TransactionAbortedException;
		protected internal abstract Tuple FetchNext();

		/// <summary>
		/// Closes this iterator. If overridden by a subclass, they should call
		/// super.close() in order for Operator's internal state to be consistent.
		/// </summary>
		public virtual void Close()
		{
			// Ensures that a future call to next() will fail
			next_Conflict = null;
			this.open_Conflict = false;
		}

		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods of the current type:
		private Tuple next_Conflict = null;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods of the current type:
		private bool open_Conflict = false;
		private int estimatedCardinality = 0;

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public void open() throws DbException, TransactionAbortedException
		public virtual void Open()
		{
			this.open_Conflict = true;
		}

		/// <returns> return the children DbIterators of this operator. If there is
		///         only one child, return an array of only one element. For join
		///         operators, the order of the children is not important. But they
		///         should be consistent among multiple calls.
		///  </returns>
		public abstract DbIterator[] Children { get; set; }


		/// <returns> return the TupleDesc of the output tuples of this operator
		///  </returns>
		public abstract TupleDesc TupleDesc { get; }

		/// <returns> The estimated cardinality of this operator. Will only be used in
		///         lab6
		///  </returns>
		public virtual int EstimatedCardinality
		{
			get
			{
				return this.estimatedCardinality;
			}
			set
			{
				this.estimatedCardinality = value;
			}
		}
	}
}
