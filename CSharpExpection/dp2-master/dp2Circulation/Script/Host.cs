using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

using DigitalPlatform;
using DigitalPlatform.Marc;

namespace dp2Circulation
{
    /// <summary>
    /// �ɵ��ֲᴰ���ο��������ࡣ�����Ѿ��� DetailHost �������
    /// ����Host��Ĵ����ǳ��ڼ�����ǰ�Ľű����ǣ�����һ��ʱ���ɾ�������
    /// </summary>
    public class Host
    {
        /// <summary>
        /// �ֲᴰ
        /// </summary>
        public EntityForm DetailForm = null;

        /// <summary>
        /// �ű������� Assembly
        /// </summary>
        public Assembly Assembly = null;

        /// <summary>
        /// ���캯��
        /// </summary>
        public Host()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// ����һ�� Ctrl+A ����
        /// </summary>
        /// <param name="strFuncName">������</param>
        public void Invoke(string strFuncName)
        {
            Type classType = this.GetType();

            // ���ó�Ա����
            classType.InvokeMember(strFuncName,
                BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.InvokeMethod
                ,
                null,
                this,
                null);
        }

        /// <summary>
        /// ��ں���
        /// </summary>
        /// <param name="sender">�¼�������</param>
        /// <param name="e">�¼�����</param>
        public virtual void Main(object sender, HostEventArgs e)
        {

        }
    }

    /// <summary>
    /// Host ͳ���¼�����
    /// </summary>
    public class HostEventArgs : EventArgs
    {
        /*
        // �Ӻδ�����? MarcEditor EntityEditForm
        public object StartFrom = null;
         * */

        /// <summary>
        /// [in]�������ݵ��¼�����
        /// </summary>
        public GenerateDataEventArgs e = null;
    }
}
