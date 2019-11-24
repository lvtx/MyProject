using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEmbed
{
    class OneToManyClass
    {
        private List<InnerClass> objs = new List<InnerClass>();
        public void Add(InnerClass obj)
        {
            if(obj != null)
            {
                objs.Add(obj);
            }
        }
        public void Remove(int index)
        {
            if (index >= 0 && index < objs.Count)
            {
                objs.RemoveAt(index);
            }
        }
    }

    class OneToManyClass2<T>
    {
        private IEnumerable<T> objs = null;
        public OneToManyClass2(IEnumerable<T> objCollections)
        {
            objs = objCollections;
        }
    }
}
