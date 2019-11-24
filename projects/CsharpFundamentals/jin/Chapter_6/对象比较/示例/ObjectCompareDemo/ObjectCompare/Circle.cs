using System;

namespace ObjectCompare
{
    /// <summary>
    /// 圆心
    /// </summary>
    public struct CircleCenter
    {
        public double x;
        public double y;
    }

    class Circle : IComparable, IComparable<Circle>, IEquatable<Circle>
    {
        public double Radius = 0;   //半径
        public CircleCenter Center { get; set; } //圆心

        //实现IComparable接口定义的方法
        public int CompareTo(object obj)
        {
            if (!(obj is Circle))
                throw new ArgumentException("只能比对Cirlce对象");
            return CompareTo(obj as Circle);
        }
        //实现IComparable<T>接口定义的方法
        public int CompareTo(Circle other)
        {
            double ret = Math.Abs(other.Radius - this.Radius);
            if (ret < 1e-3)
                return 0;
            if (other.Radius < this.Radius)
                return 1;
            return -1;
        }

        //覆盖Object类的GetHashCode方法
        public override int GetHashCode()
        {
            //整数部分与小数点后3位相同的对象生成相同的哈希值
            return (int)(Radius * 1000); 
        }

        //重写Object类的Equals方法
        public override bool Equals(object obj)
        {
            if (this.CompareTo(obj) == 0)
                return true;
            return false;
        }

        //实现IEquatable<Circle>接口定义的方法
        public bool Equals(Circle other)
        {
            return this.CompareTo(other) == 0;
        }
        //----------------------------------------
        //重载相关的运算符
        //----------------------------------------
        public static bool operator ==(Circle obj1, Circle obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Circle obj1, Circle obj2)
        {
            return !(obj1.Equals(obj2));

        }

        public static bool operator >(Circle obj1, Circle obj2)
        {
            if (obj1.CompareTo(obj2) > 0)
                return true;
            return false;
        }
        public static bool operator <(Circle obj1, Circle obj2)
        {
            if (obj1.CompareTo(obj2) < 0)
                return true;
            return false;
        }

        public static bool operator <=(Circle obj1, Circle obj2)
        {
            if ((obj1.CompareTo(obj2) < 0) || (obj1.CompareTo(obj2) == 0))
                return true;
            return false;
        }

        public static bool operator >=(Circle obj1, Circle obj2)
        {
            if ((obj1.CompareTo(obj2) > 0) || (obj1.CompareTo(obj2) == 0))
                return true;
            return false;
        }





    }

}
