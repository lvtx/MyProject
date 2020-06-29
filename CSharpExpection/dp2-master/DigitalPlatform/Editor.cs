using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DigitalPlatform
{
    class Editor
    {
    }

    // �������ݼӹ�ģ��
    public delegate void GenerateDataEventHandler(object sender,
        GenerateDataEventArgs e);

    public class GenerateDataEventArgs : EventArgs
    {
        // ��ǰ�������ڵ��ӿؼ�
        public object FocusedControl = null;

        // 2009/2/27
        public string ScriptEntry = ""; // ��ں����������Ϊ�գ�������Main(object sender, GenerateDataEventArgs e)

        public bool ShowErrorBox = true;    // [in]�Ƿ�Ҫ��ʾ����MessageBox
        public string ErrorInfo = "";   // [out]������Ϣ

        // 2015/5/29
        // ���ӵĲ������󡣾���������ÿ�����ܹ涨
        public object Parameter = null;
    }


    // Ctrl+?��������
    public delegate void ControlLetterKeyPressEventHandler(object sender,
        ControlLetterKeyPressEventArgs e);

    public class ControlLetterKeyPressEventArgs : EventArgs
    {
        public Keys KeyData = Keys.Control;
        public bool Handled = false;
    }

    //

    // �����Ŀ��Ϣ
    public delegate void GetBiblioEventHandler(object sender,
        GetBiblioEventArgs e);

    public class GetBiblioEventArgs : EventArgs
    {
        // ���ݼ�¼
        public string Data { get; set; }
        // ��ʽ��unimarc/usmarc/xml
        public string Syntax { get; set; }
    }

}
