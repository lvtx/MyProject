using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaveFormStatus
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            LoadStatus();
        }

        private void btnChooseColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                BackColor = colorDialog1.Color;
            }
        }

        private FormStatus status = null;
        private void SaveStatus()
        {
            status=new FormStatus();
            status.BackgroundColor=this.BackColor;
            status.Left=this.Left;
            status.Top=this.Top;
            status.Width=this.Width;
            status.Height = this.Height;

            using(FileStream fs=new FileStream("FormStatus.cfg",FileMode.Create))
            {

                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, status);
            }
        }

        private void LoadStatus()
        {
            try
            {
                if (File.Exists("FormStatus.cfg"))
                {

                    using (FileStream fs = new FileStream("FormStatus.cfg", FileMode.Open))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        status = formatter.Deserialize(fs) as FormStatus;
                        if (status != null)
                        {
                            this.BackColor = status.BackgroundColor;
                            this.Left = status.Left;
                            this.Top = status.Top;
                            this.Width = status.Width;
                            this.Height = status.Height;

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveStatus();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
