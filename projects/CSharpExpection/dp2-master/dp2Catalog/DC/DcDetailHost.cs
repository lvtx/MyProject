using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using System.Windows.Forms;
using System.Diagnostics;

using DigitalPlatform.CommonControl;

namespace dp2Catalog
{
    /// <summary>
    /// Summary description for Host.
    /// </summary>
    public class DcDetailHost
    {
        public DcForm DetailForm = null;
        public Assembly Assembly = null;

        public DcDetailHost()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void Invoke(string strFuncName)
        {
            Type classType = this.GetType();

            // newһ��Host��������
            classType.InvokeMember(strFuncName,
                BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.InvokeMethod
                ,
                null,
                this,
                null);

        }

        public virtual void Main(object sender, HostEventArgs e)
        {

        }

    }

}

