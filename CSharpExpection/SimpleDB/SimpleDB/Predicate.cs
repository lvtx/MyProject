using System;
using SimpleDB;
using System.Collections.Generic;
using Tuple = SimpleDB.Tuple;

namespace SimpleDB
{
	/// <summary>
	/// 谓词将元组与指定的字段值进行比较
	/// </summary>
	[Serializable]
	public class Predicate
	{

		private const long serialVersionUID = 1L;
		private int index;
		private Op op;
		private Field operand;
		/// <summary>
		/// Field.Compare之后返回Code常量
		/// </summary>
		[Serializable]
		public sealed class Op
		{
			public static readonly Op EQUALS = new Op("EQUALS", InnerEnum.EQUALS);
			public static readonly Op GREATER_THAN = new Op("GREATER_THAN", InnerEnum.GREATER_THAN);
			public static readonly Op LESS_THAN = new Op("LESS_THAN", InnerEnum.LESS_THAN);
			public static readonly Op LESS_THAN_OR_EQ = new Op("LESS_THAN_OR_EQ", InnerEnum.LESS_THAN_OR_EQ);
			public static readonly Op GREATER_THAN_OR_EQ = new Op("GREATER_THAN_OR_EQ", InnerEnum.GREATER_THAN_OR_EQ);
			public static readonly Op LIKE = new Op("LIKE", InnerEnum.LIKE);
			public static readonly Op NOT_EQUALS = new Op("NOT_EQUALS", InnerEnum.NOT_EQUALS);

			private static readonly IList<Op> valueList = new List<Op>();

			static Op()
			{
				valueList.Add(EQUALS);
				valueList.Add(GREATER_THAN);
				valueList.Add(LESS_THAN);
				valueList.Add(LESS_THAN_OR_EQ);
				valueList.Add(GREATER_THAN_OR_EQ);
				valueList.Add(LIKE);
				valueList.Add(NOT_EQUALS);
			}

			public enum InnerEnum
			{
				EQUALS,
				GREATER_THAN,
				LESS_THAN,
				LESS_THAN_OR_EQ,
				GREATER_THAN_OR_EQ,
				LIKE,
				NOT_EQUALS
			}

			public readonly InnerEnum innerEnumValue;
			private readonly string nameValue;
			private readonly int ordinalValue;
			private static int nextOrdinal = 0;

			private Op(string name, InnerEnum innerEnum)
			{
				nameValue = name;
				ordinalValue = nextOrdinal++;
				innerEnumValue = innerEnum;
			}

			/// <summary>
			/// 通过整数值访问操作的接口，以便于命令行操作
			/// </summary>
			/// <param name="i"> 有效的整数运算索引 </param>
			public static Op GetOp(int i)
			{
				return Values()[i];
			}

			public override string ToString()
			{
				if (this == EQUALS)
				{
					return "=";
				}
				if (this == GREATER_THAN)
				{
					return ">";
				}
				if (this == LESS_THAN)
				{
					return "<";
				}
				if (this == LESS_THAN_OR_EQ)
				{
					return "<=";
				}
				if (this == GREATER_THAN_OR_EQ)
				{
					return ">=";
				}
				if (this == LIKE)
				{
					return "LIKE";
				}
				if (this == NOT_EQUALS)
				{
					return "<>";
				}
				throw new System.InvalidOperationException("impossible to reach here");
			}


			public static IList<Op> Values()
			{
				return valueList;
			}

			public int Ordinal()
			{
				return ordinalValue;
			}

			public static Op ValueOf(string name)
			{
				foreach (Op enumInstance in Op.valueList)
				{
					if (enumInstance.nameValue == name)
					{
						return enumInstance;
					}
				}
				throw new System.ArgumentException(name);
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="index">要进行比较的传入元组的字段编号</param>
		/// <param name="op">用于比较的操作</param>
		/// <param name="operand">要与传入的元组进行比较的字段值</param>
		public Predicate(int index, Op op, Field operand)
		{
			this.index = index;
			this.op = op;
			this.operand = operand;
		}

		/// <returns> the field number </returns>
		public virtual int Field
		{
			get
			{
				return index;
			}
		}

		/// <returns> the operator </returns>
		public virtual Op GetOp()
		{
			// some code goes here
			return null;
		}

		/// <returns> the operand </returns>
		public virtual Field Operand
		{
			get
			{
				return operand;
			}
		}

		/// <summary>
		/// 使用构造函数中特定的运算符，将构造函数中指定的字段编号
		/// t与构造函数中指定的操作数字段进行比较
		/// 可以通过Field的比较方法进行比较。
		/// </summary>
		/// <param name="t">要比较的元组</param>
		/// <returns> 如果比较为TRUE，则为TRUE，否则为FALSE </returns>
		public virtual bool Filter(Tuple t)
		{
			return t.GetField(index).Compare(op,operand);
		}

		/// <summary>
		/// Returns something useful, like "f = field_id op = op_string operand =
		/// operand_string
		/// </summary>
		public override string ToString()
		{
			// some code goes here
			return "";
		}
	}
}
