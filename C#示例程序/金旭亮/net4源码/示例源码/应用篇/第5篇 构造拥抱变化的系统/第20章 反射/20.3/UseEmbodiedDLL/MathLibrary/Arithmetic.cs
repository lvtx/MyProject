using System;
using System.Collections.Generic;
using System.Text;

namespace MathLibrary
{
    public class Arithmetic
    {
        public static int Add(int x, int y)
        {
            return x + y;
        }
        public void ShowForm()
        {
            frmInDLL frm = new frmInDLL();
            frm.Show();
        }
    }
}
