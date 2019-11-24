using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseClassAndSubClass
{
    class Program
    {
        static void Main(string[] args)
        {
            //MyBaseClass myBaseClass = new MyBaseClass("123");
            //MyBaseClass mySubClass = new MySubClass("123",1);
            MySubClass mySubClass = new MySubClass("123", 1);
        }
    }

    class MyBaseClass
    {
        public MyBaseClass(string baseClassNeedsThis)
        {
            MessageBox.Show("This is the base class: " + baseClassNeedsThis);
        }
    }

    class MySubClass : MyBaseClass
    {
        public MySubClass(string baseClassNeedsThis, int anotherValue):base(baseClassNeedsThis)
        {
            MessageBox.Show("This is the subclass: " + baseClassNeedsThis 
                + " and " + anotherValue);
        }
    }
}
