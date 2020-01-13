using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderExplorer
{
    class Folder
    {
        public Folder()
        {
            this.FullPath = "c:\\";
        }
        private DirectoryInfo folder { get; set; }
        private ObservableCollection<Folder> subFolders { get; set; }
        public ObservableCollection<FileInfo> files { get; set; }
        public string Name { get { return this.folder.Name; } }
        public string FullPath
        {
            get { return this.folder.FullName; }
            set
            {
                if (Directory.Exists(value))
                {
                    this.folder = new DirectoryInfo(value);
                }
                else
                {
                    throw new ArgumentException("must exist", "fullPath");
                }
            }
        }
        public ObservableCollection<FileInfo> Files
        {
            get
            {
                if (this.files == null)
                {
                    this.files = new ObservableCollection<FileInfo>();
                    FileInfo[] fi = this.folder.GetFiles();
                    for (int i = 0; i < fi.Length - 1; i++)
                    {
                        this.files.Add(fi[i]);
                    }
                }
                return this.files;
            }
        }
        public ObservableCollection<Folder> SubFolders
        {
            get
            {
                if (this.subFolders == null)
                {
                    try
                    {
                        this.subFolders = new ObservableCollection<Folder>();
                        DirectoryInfo[] di = this.folder.GetDirectories();
                        for (int i = 0; i < di.Length - 1; i++)
                        {
                            Folder newFolder = new Folder();
                            newFolder.FullPath = di[i].FullName;
                            this.subFolders.Add(newFolder);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.Message);
                    }
                }
                return this.subFolders;
            }
        }
    }
}

