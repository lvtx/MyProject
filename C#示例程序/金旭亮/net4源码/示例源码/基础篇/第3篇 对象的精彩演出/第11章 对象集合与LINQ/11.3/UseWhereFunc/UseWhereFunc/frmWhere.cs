using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace UseWhereFunc
{
    public partial class frmWhere : Form
    {
        public frmWhere()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtFindWhat.Text.Trim().Length == 0)
                return;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string folder = folderBrowserDialog1.SelectedPath;
                lblFolder.Text = folder;
                List<FileInfo> files = FileBrowserHelper.GetAllFilesInFolder(folder);
                IEnumerable<FileInfo> ret=files.Where<FileInfo>(
                    file=>Path.GetFileNameWithoutExtension(file.Name).IndexOf(txtFindWhat.Text.Trim())!=-1);

                FileBrowserHelper.ShowFileListInListBox(ret, lstFiles);
            }
        }
    }
}
