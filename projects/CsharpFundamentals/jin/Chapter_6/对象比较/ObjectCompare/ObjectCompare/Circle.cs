using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectCompare
{
    class Circle:IComparable,IComparable<Circle>,IEquatable<Circle>
    {
        public double radius = 0;
        public int CompareTo(object obj)
        {
            if(!(obj is Circle))
            {
                throw new ArgumentException("只能比较圆的大小");
            }
            else
            {
                return CompareTo(obj as Circle);
            }
        }
        public int CompareTo(Circle obj)
        {
            double diff = Math.Abs(this.radius - obj.radius);
            if (diff < 1e-3)
            {
                return 0;
            }
            if (this.radius > obj.radius)
                return 1;
            else
                return -1;
        }
        public override bool Equals(object obj)
        {
            if (this.CompareTo(obj) == 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool Equals(Circle obj)
        {
            return this.CompareTo(obj) == 0;
        }
        public override int GetHashCode()
        {
            return (int)(this.radius * 1000);
        }
        public static bool operator ==(Circle obj1,Circle obj2)
        {
            return obj1.Equals(obj2);
        }
        public static bool operator !=(Circle obj1,Circle obj2)
        {
            return !(obj1.Equals(obj2));
        }
        public static bool operator >(Circle obj1,Circle obj2)
        {
            return obj1.CompareTo(obj2) == 1;
        }
        public static bool operator <(Circle obj1,Circle obj2)
        {
            return obj1.CompareTo(obj2) == -1;
        }
        public static bool operator >=(Circle obj1,Circle obj2)
        {
            if (obj1.CompareTo(obj2) >= 0)
            {
                return true;
            }
            else
                return false;
        }
        public static bool operator <=(Circle obj1,Circle obj2)
        {
            if (obj1.CompareTo(obj2) <= 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
