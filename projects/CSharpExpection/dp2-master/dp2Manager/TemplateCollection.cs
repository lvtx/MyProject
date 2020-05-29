using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using DigitalPlatform.rms.Client;

namespace dp2Manager
{

    [Serializable()]
    public class Template
    {
        public DatabaseObject Object = null;
        public List<string[]> LogicNames = null;
        public string Type = "";
        public string SqlDbName = "";
        public string KeysDef = "";
        public string BrowseDef = "";

    }

    [Serializable()]
    public class TemplateCollection
    {
        List<Template> m_list = null;

        public TemplateCollection()
        {
            m_list = new List<Template>();
        }

        public void Add(Template o)
        {
            m_list.Add(o);
        }

        public int Count
        {
            get
            {
                return this.m_list.Count;
            }
        }

        public Template this[int index]
        {
            get
            {
                return m_list[index];
            }
        
        }

        // ���ļ���װ�ش���һ��TemplateCollection����
        // parameters:
        //		bIgnorFileNotFound	�Ƿ��׳�FileNotFoundException�쳣��
        //							���==true������ֱ�ӷ���һ���µĿ�TemplateCollection����
        // Exception:
        //			FileNotFoundException	�ļ�û�ҵ�
        //			SerializationException	�汾Ǩ��ʱ���׳���
        public static TemplateCollection Load(
            string strFileName,
            bool bIgnorFileNotFound)
        {
            Stream stream = null;
            TemplateCollection templates = null;

            try
            {
                stream = File.Open(strFileName, FileMode.Open);
            }
            catch (FileNotFoundException ex)
            {
                if (bIgnorFileNotFound == false)
                    throw ex;

                templates = new TemplateCollection();
                // templates.m_strFileName = strFileName;

                // �õ�����һ���µĿն������
                return templates;
            }


            BinaryFormatter formatter = new BinaryFormatter();

            templates = (TemplateCollection)formatter.Deserialize(stream);
            stream.Close();
            // templates.m_strFileName = strFileName;


            return templates;
        }

        // ���浽�ļ�
        // parameters:
        //		strFileName	�ļ��������==null,��ʾʹ��װ��ʱ������Ǹ��ļ���
        public void Save(string strFileName)
        {
            if (String.IsNullOrEmpty(strFileName) == true)
            {
                throw (new Exception("TemplateCollection.Save()û��ָ�������ļ���"));
            }

            Stream stream = File.Open(strFileName,
                FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, this);
            stream.Close();
        }

    }
}
