using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectEmbodied
{
    /// <summary>
    /// 一对多的对象组合方式一
    /// 通常会定义一些公有方法实现向其内部集合中增删对象
    /// </summary>
    class OneToManyClass
    {
        /// <summary>
        /// 内部对象集合
        /// </summary>
        private List<InnerClass> objs = new List<InnerClass>();

        public void Add(InnerClass obj)
        {
            if (obj != null)
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
    /// <summary>
    /// 一对多的对象组合方式二
    /// 一般使用对象注入的方式关联上外部对象集合
    /// 对象集合放在外部，本身通常不需要提供向此对象集合中增删对象的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class OneToManyClass2<T>
    {
        /// <summary>
        /// 内部对象集合
        /// </summary>
        private IEnumerable<T> objs = null;

        public OneToManyClass2(IEnumerable<T> objCollections)
        {
            objs = objCollections;
        }
    }
}
