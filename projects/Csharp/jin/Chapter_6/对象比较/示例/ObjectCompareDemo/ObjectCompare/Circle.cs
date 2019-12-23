using System;

namespace ObjectCompare
{
    /// <summary>
    /// Բ��
    /// </summary>
    public struct CircleCenter
    {
        public double x;
        public double y;
    }

    class Circle : IComparable, IComparable<Circle>, IEquatable<Circle>
    {
        public double Radius = 0;   //�뾶
        public CircleCenter Center { get; set; } //Բ��

        //ʵ��IComparable�ӿڶ���ķ���
        public int CompareTo(object obj)
        {
            if (!(obj is Circle))
                throw new ArgumentException("ֻ�ܱȶ�Cirlce����");
            return CompareTo(obj as Circle);
        }
        //ʵ��IComparable<T>�ӿڶ���ķ���
        public int CompareTo(Circle other)
        {
            double ret = Math.Abs(other.Radius - this.Radius);
            if (ret < 1e-3)
                return 0;
            if (other.Radius < this.Radius)
                return 1;
            return -1;
        }

        //����Object���GetHashCode����
        public override int GetHashCode()
        {
            //����������С�����3λ��ͬ�Ķ���������ͬ�Ĺ�ϣֵ
            return (int)(Radius * 1000); 
        }

        //��дObject���Equals����
        public override bool Equals(object obj)
        {
            if (this.CompareTo(obj) == 0)
                return true;
            return false;
        }

        //ʵ��IEquatable<Circle>�ӿڶ���ķ���
        public bool Equals(Circle other)
        {
            return this.CompareTo(other) == 0;
        }
        //----------------------------------------
        //������ص������
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
