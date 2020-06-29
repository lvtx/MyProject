using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityAssociation;
using System.Threading.Tasks;

namespace EntityAssociationTest.MultiLevelEmbed
{
    [TestClass]
    public class MultiLevelEmbedTest
    {
        [TestMethod]
        public async Task TestInsert()
        {
            using (var context = new MyDBEntities())
            {
                //创建一个容器对象
                MyClassContainer container = MultiLevelEmbedTestHelper.CreateMyClassContainer();
                
                MyClass obj = null;
               
                //创建5个MyClass对象，每个对象有5个MyClassChild对象
                for (int i = 0; i < 5; i++)
                {
                    obj = MultiLevelEmbedTestHelper.CreateMyClass(container);
                    for (int j = 0; j < 5; j++)
                    {
                        MultiLevelEmbedTestHelper.CreateMyClassChild(obj);
                    }

                }

                //仅需加入最顶层的容器
                context.MyClassContainers.Add(container);

                int result = await context.SaveChangesAsync();
                Assert.IsTrue(result>0);

            }
        }
    }
}
