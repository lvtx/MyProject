using System;
using DataAccessLayer;
using LibraryModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DALTestProject
{
    [TestClass]
    public class TestReaderRepository
    {
        IRepository<Reader> ReaderTypeEntity = null;
        [TestInitialize]
        public void TestInitialize()
        {
            ReaderTypeEntity = new ReaderRepository();
        }
        [TestMethod]
        public void GetAllClientsTest()
        {
            List<Reader> readers = ReaderTypeEntity.GetAllClient();
            foreach (var reader in readers)
            {
                int i = 0;
                if (i < 20)
                {
                    Console.WriteLine(reader.ReaderName);
                    i++;
                }
            }
        }
    }
}
