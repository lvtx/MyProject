using System;
using LibraryModel;
using DataAccessLayer;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DALTestProject
{
    [TestClass]
    public class TestReaderTypeRepository
    {
        IRepository<RoleType> ReaderTypeEntity = null;
        [TestInitialize]
        public void TestInitialize()
        {
            ReaderTypeEntity = new ReaderTypeRepository();
        }
        [TestMethod]
        public void TestGetAllClient()
        {
            List<RoleType> ReaderTypes = ReaderTypeEntity.GetAllClient();
            foreach (var readerType in ReaderTypes)
            {
                var readers = readerType.SReader;
                foreach (var reader in readers)
                {
                    Console.WriteLine(reader.ReaderName);
                }
            }
        }
    }
}
