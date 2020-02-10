using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesInfoCRUD
{
    class StoreDB
    {
        public StoreDB()
        {

        }
        public virtual void GetResult()
        {
            Console.WriteLine("成功");
        }
    }
    /// <summary>
    /// 将查找到的盘符存储到数据库
    /// </summary>
    class StoreDBDrive : StoreDB
    {
        private DriveInfo[] drives = null;
        /// <summary>
        /// 传入一个DriveInfo[]类型的参数
        /// </summary>
        /// <param name="drivesArgu"></param>
        public StoreDBDrive(DriveInfo[] drivesArgu)
        {
            drives = drivesArgu;
        }
        public void StoreDrive()
        {
            int i = 1;
            List<tDirectory> tDirectories = new List<tDirectory>();
            foreach (var d in drives)
            {
                tDirectory tDirClient = new tDirectory();
                tDirClient.Directory = d.Name.Trim();
                tDirClient.DirectoryId = i;
                i++;
                tDirectories.Add(tDirClient);

            }
            using (var filesDbContext = new FilesDBContext())
            {
                try
                {
                    foreach (var item in tDirectories)
                    {
                        filesDbContext.tDirectories.Add(item);
                        filesDbContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        /// <summary>
        /// 读取存入的数据
        /// </summary>
        public override void GetResult()
        {
            using (var filesDbContext = new FilesDBContext())
            {
                var drives = from d in filesDbContext.tDirectories
                             select d;
                foreach (var d in drives)
                {
                    Console.WriteLine("{0} id is {1}",d.Directory.Trim(),d.DirectoryId);
                }
            }
        }
    }
    /// <summary>
    /// 将查找到的文件和子文件夹存储到数据库
    /// </summary>
    //class StoreDBFloder : StoreDB
    //{
    //    private Folder folder = new Folder();
    //    public StoreDBFloder(Folder folderArgu)
    //    {
    //        folder = folderArgu;
    //    }
    //    public void StoreFloder()
    //    {
    //        tFile fileClient = new tFile();
    //        int i = 0;
    //        foreach (var subFolder in folder.SubFolders)
    //        {
    //            fileClient.FileId = i;
    //            fileClient.sFile = subFolder.Name;
    //            i++;
    //        }
    //        using (var filesDbContext = new FilesDBContext())
    //        {                
    //            fileClient.;
    //        }
    //    }
    //}
}
