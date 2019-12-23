using CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTest.MultiLevelEmbed
{
    public class MultiLevelEmbedTestHelper
    {
        private static Random ran = new Random();
        public static MyClassContainer CreateMyClassContainer()
        {
            return new MyClassContainer()
            {
                Information = "MyClassContainer" + ran.Next(1, 10000)
            };
        }

        public static MyClass CreateMyClass(MyClassContainer container=null)
        {
            MyClass obj = new MyClass();
            if (container != null)
            {
                obj.MyClassContainerID = container.MyClassContainerID;
                
                obj.Information = container.Information + "的MyClass子对象" + ran.Next(1, 1000);
                container.MyClasses.Add(obj);
            }
            else
            {
                obj.Information = "MyClass对象" + ran.Next(1, 1000);
            }
            return obj;
        }

        public static MyClassChild CreateMyClassChild(MyClass parent)
        {
            MyClassChild child = new MyClassChild();
            if (parent != null)
            {
                child.MyClassID = parent.MyClassID;
                parent.MyClassChilds.Add(child);
                child.Informatin = parent.Information + "的MyClassChild子对象" + ran.Next(1, 1000);
            }
            else
            {
                child.Informatin =  "MyClassChild对象" + ran.Next(1, 1000);
            }
            return child;
        }
    }
}
