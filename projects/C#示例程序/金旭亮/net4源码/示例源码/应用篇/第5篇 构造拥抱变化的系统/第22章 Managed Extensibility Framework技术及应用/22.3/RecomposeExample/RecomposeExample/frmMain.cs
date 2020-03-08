using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using MyPartInterfaceLibrary;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace RecomposeExample
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            GetParts();
        }
        [ImportMany(AllowRecomposition = true)]
        IEnumerable<IMyPart> parts;

        DirectoryCatalog dirCata;
       
        CompositionContainer container = null;
        private void GetParts()
        {
            dirCata= new DirectoryCatalog(".");
            container = new CompositionContainer(dirCata);
            container.ComposeParts(this);
            container.ExportsChanged += new EventHandler<ExportsChangeEventArgs>(container_ExportsChanged);
            RefreshPartList();
        }

        void container_ExportsChanged(object sender, ExportsChangeEventArgs e)
        {
            RefreshPartList();
        }

        private void RefreshPartList()
        {
            lstParts.Items.Clear();
            foreach (IMyPart part in parts)
            {
                lstParts.Items.Add(part.IntroduceMySelt());
               
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dirCata.Refresh();
            
        }

      

    }
}
