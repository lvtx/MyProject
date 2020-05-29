using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


namespace DigitalPlatform.dp2.Statis
{

    /// <summary>
    /// �������ֵ��ļ��ļ���
    /// �����������������ļ�����
    /// </summary>
    public class NamedStringFileCollection : List<NamedStringFile>
    {
        public NamedStringFileCollection()
        {
        }

        public void AddLine(
            string strName,
            string strLine)
        {
            NamedStringFile file = GetStringFile(strName);

            // Debug.Assert(false, "");
            LineItem lineitem = new LineItem();
            lineitem.FileLine = new FileLine(strLine);
            file.StringFile.Add(lineitem);
        }

        // ���һ���ʵ��ı�����û���ҵ������Զ�����
        public NamedStringFile GetStringFile(string strName)
        {
            for (int i = 0; i < this.Count; i++)
            {
                NamedStringFile file = this[i];

                if (file.Name == strName)
                    return file;
            }

            // û���ҵ�������һ���µı�
            NamedStringFile newFile = new NamedStringFile();
            newFile.Name = strName;
            newFile.StringFile = new StringFile();
            newFile.StringFile.Open(false);

            this.Add(newFile);
            return newFile;
        }

    }

    // �������ֵı��
    public class NamedStringFile
    {
        public string Name = "";   // ����
        public StringFile StringFile = null;
    }

}
