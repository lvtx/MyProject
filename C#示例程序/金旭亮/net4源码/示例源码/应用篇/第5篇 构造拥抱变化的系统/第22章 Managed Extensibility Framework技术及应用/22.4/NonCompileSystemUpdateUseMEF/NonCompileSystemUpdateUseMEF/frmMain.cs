using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InterfaceLibrary;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace NonCompileSystemUpdateUseMEF
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            DirectoryCatalog dircata = new DirectoryCatalog(Application.StartupPath);
            _container = new CompositionContainer(dircata);
            _container.ComposeParts(this);
  
        }
        [ImportMany(typeof(IMyInterface))]
        private IEnumerable<Lazy<IMyInterface,IMyComponentMetaDataView>> Components;

       
        private IMyInterface Component=null;

        private CompositionContainer _container = null;

        private void ComposeComponent()
        {
            if(rdoNone.Checked)
                Component=null;
            else
            {
                if (rdoComponent1.Checked)
                     FindComponent("SystemComponent1");
                if (rdoComponent2.Checked)
                     FindComponent("SystemComponent2");
            }

        }

        private void FindComponent(string ComponentName)
        {
            foreach (var part in Components)
            {
                if (part.Metadata.ComponentName==ComponentName)
                {
                    Component = part.Value;
                    break;
                }
            }
        }

        private void btnOpenForm_Click(object sender, EventArgs e)
        {
            ComposeComponent();
            if( Component!=null)
                 Component.Run();
            
        }
    }
}
