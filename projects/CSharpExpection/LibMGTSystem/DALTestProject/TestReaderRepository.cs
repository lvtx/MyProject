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
        [TestMethod]
        public void ShowReaderTypes()
        {
            //using (var library = new LibraryEntities())
            //{
            //    var readerTypes = library.GetReaderType();

            //    foreach (var readerType in readerTypes)
            //    {
            //        Console.WriteLine(readerType.RoleTypeName);
            //    }
            //}

            var readerTypes = DBHelper.GetReaderType();
            foreach (var readerType in readerTypes)
            {
                Console.WriteLine(readerType.RoleTypeName);
            }
        }
    }
}
