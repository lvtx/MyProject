using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;

using DigitalPlatform.rms.Client;
using DigitalPlatform.Xml;
using DigitalPlatform.Text;

namespace dp2Manager
{

    public class QuickRights
    {
        XmlDocument dom = null;

        public event SetRightsEventHandle SetRights = null;

        public event GetNodeStyleEventHandle GetNodeStyle = null;

        /*
        public string ServerRights = "";
        public string DatabaseRights = "";
        public string DirectoryRights = "";
        public string FileRights = "";
         */

        public static bool MatchType(int nType,
              string strType)
        {
            if (nType == ResTree.RESTYPE_SERVER
                && strType == "server")
                return true;
            if (nType == ResTree.RESTYPE_DB
                && strType == "database")
                return true;
            if (nType == ResTree.RESTYPE_FOLDER
                && strType == "directory")
                return true;
            if (nType == ResTree.RESTYPE_FILE
                && strType == "file")
                return true;

            return false;
        }

        // ��xml�ļ�����ʹ�õķ���ַ�������ΪResTree.RESSTYLE_???���͵�����
        public static int GetStyleInt(string strStyle)
        {
            if (String.IsNullOrEmpty(strStyle) == true)
                return 0;

            if (String.Compare(strStyle, "userdatabase", true) == 0)
                return ResTree.RESSTYLE_USERDATABASE;

            throw (new Exception("δ֪�ķ���ַ��� '" + strStyle + "'"));
        }

        public static int Build(XmlDocument CfgDom,
            string strProjectName,
            out QuickRights quickrights,
            out string strError)
        {
            quickrights = null;
            strError = "";

            string strXPath = "//style[@name='" + strProjectName + "']";
            XmlNode parent = CfgDom.DocumentElement.SelectSingleNode(strXPath);
            if (parent == null)
            {
                strError = "����Ϊ '" + strProjectName + "' �ķ���û���ҵ�";
                return -1;
            }

            quickrights = new QuickRights();

            quickrights.dom = new XmlDocument();

            quickrights.dom.LoadXml(parent.OuterXml);

            return 0;
        }

        // �ⲿ����
        // parameters:
        //      ois Ҫ����Ķ����������
        public int ModiRights(List<TreeNode> selectedTreeNodes,
            TreeNode root)
        {
            List<XmlNode> cfgNodes = new List<XmlNode>();

            if (root.ImageIndex == ResTree.RESTYPE_SERVER)
            {
                XmlNodeList nodes = this.dom.DocumentElement.SelectNodes("object");
                for (int i = 0; i < nodes.Count; i++)
                {
                    cfgNodes.Add(nodes[i]);
                }
            }
            else if (root.ImageIndex == ResTree.RESTYPE_DB)
            {
                XmlNodeList nodes = this.dom.DocumentElement.SelectNodes("object/object[@type='database' or @type='*']");
                for (int i = 0; i < nodes.Count; i++)
                {
                    cfgNodes.Add(nodes[i]);
                }
            }
            else
            {
                Debug.Assert(false, "�����ܵ����");
            }

            ModiRights(0,
                selectedTreeNodes,
                root,
                cfgNodes);
            return 0;
        }

        /*
        // �����������ҵ����е�ȫ�����ýڵ�
        void HitCfgNodes(string strPath,
            XmlNode parentXmlNode,
            ref List<XmlNode> cfgnodes)
        {
            if (strPath == "")
                return;

            string strCur = StringUtil.GetFirstPartPath(ref strPath);

            for (int i = 0; i < parentXmlNode.ChildNodes.Count; i++)
            {
                XmlNode xmlnode = parentXmlNode.ChildNodes[i];
                if (xmlnode.NodeType != XmlNodeType.Element)
                    continue;
                string strName = DomUtil.GetAttr(xmlnode, "name");
                if (strName == "" || strName == "*")
                {
                    goto DONEST;
                }

                if (strName == strCur)
                    goto DONEST;

                continue;

            DONEST:
                if (strPath == "")  // ĩ��
                    cfgnodes.Add(xmlnode);
                else
                {
                    HitCfgNodes(strPath,
                        xmlnode,
                        ref cfgnodes);
                }
            }
        }
         */

        // parameters:
        //      bLocateCfgNode  ==true treenode���ǴӺ�cfgParentNode��Ӧ�ĸ���ʼ�ģ�������Ҫ�����¼���������·�����ҵ���Ӧ��cfgnode����
        void ModiRights(
            int nFound,
            List<TreeNode> selectedTreeNodes,
            List<TreeNode> treenodes,
            List<XmlNode> cfgnodes)
        {

            for (int i = 0; i < treenodes.Count; i++)
            {
                TreeNode curtreenode = treenodes[i];

                ModiRights(nFound,
                    selectedTreeNodes,
                    curtreenode,
                    cfgnodes);
            }

        }

        // ����Ԥ��ģʽ�޸�һ���ڵ��Լ����µ�ȫ���ӽڵ��Ȩ��
        void ModiRights(int nFound,
            List<TreeNode> selectedTreeNodes,
            TreeNode curtreenode,
            List<XmlNode> cfgnodes)
        {
            for (int i = 0; i < cfgnodes.Count; i++)
            {
                // ��ǰxml�ڵ���Ϣ
                XmlNode node = cfgnodes[i];

                string strType = DomUtil.GetAttr(node, "type");
                string strName = DomUtil.GetAttr(node, "name");
                string strRights = DomUtil.GetAttrDiff(node, "rights");
                int nStyle = QuickRights.GetStyleInt(DomUtil.GetAttr(node, "style"));

                // ƥ�������
                if (strName != "" && strName != "*")
                {
                    // @
                    if (strName != curtreenode.Text)
                        continue;
                }

                // ƥ���������
                bool bRet = QuickRights.MatchType(curtreenode.ImageIndex,
                    strType);
                if (bRet == false)
                    continue;

                // ƥ�������
                if (nStyle != 0)
                {
                    if (this.GetNodeStyle == null)
                    { 
                        // ȱʡ��Ϊ
                        /*
                        if (nStyle != ResRightTree.GetNodeStyle(curtreenode))
                            continue;
                         */
                    }
                    else
                    {
                        GetNodeStyleEventArgs e = new GetNodeStyleEventArgs();
                        e.Node = curtreenode;
                        e.Style = 0;
                        this.GetNodeStyle(this, e);
                        if (nStyle != e.Style)
                            continue;
                    }
                }

                int nIndex = -1;
                if (nFound == 0)
                    nIndex = selectedTreeNodes.IndexOf(curtreenode);

                // �����¼�
                // ���strRights == null����ʾ��ǰ�������޸���rightsֵ�����ǵݹ���ȻҪ����
                if (strRights != null 
                    && (nIndex != -1 || nFound > 0))
                {
                    if (this.SetRights != null)
                    {
                        SetRightsEventArgs e = new SetRightsEventArgs();
                        e.Node = curtreenode;
                        if (strRights == "{clear}" || strRights == "{null}")
                            e.Rights = null;
                        else
                            e.Rights = strRights;
                        this.SetRights(this, e);
                    }

                }

            // DOCHILDREN:

                // ��֯���Ӷ�������
                List<TreeNode> nodes = new List<TreeNode>();
                for (int j = 0; j < curtreenode.Nodes.Count; j++)
                {
                    nodes.Add(curtreenode.Nodes[j]);
                }

                // �ݹ�
                if (nodes.Count != 0)
                {
                    List<XmlNode> chidrencfgnodes = new List<XmlNode>();
                    for (int j = 0; j < node.ChildNodes.Count; j++)
                    {
                        XmlNode cur = node.ChildNodes[j];

                        if (cur.NodeType != XmlNodeType.Element)
                            continue;

                        if (cur.Name != "object")
                            continue;

                        chidrencfgnodes.Add(cur);
                    }

                    if (nIndex != -1)
                        nFound++;
                    ModiRights(nFound, 
                        selectedTreeNodes,
                        nodes,
                        chidrencfgnodes);
                    if (nIndex != -1)
                        nFound--;
                }

            }

        }

        /*
        public static List<TreeNode> GetTreeNodeCollection(List<ObjectInfo> ois,
            GetTreeNodeByPathEventHandle callback)
        {
            List<TreeNode> treenodes = new List<TreeNode>();
            for (int i = 0; i < ois.Count; i++)
            {
                GetTreeNodeByPathEventArgs e = new GetTreeNodeByPathEventArgs();
                e.Path = ois[i].Path;
                e.Node = null;
                callback(null, e);
                treenodes.Add(e.Node);
            }

            return treenodes;
        }
         */
    }

    public class ObjectInfo
    {
        public int ImageIndex = -1;
        public string Path = "";
        public string Url = "";
    }

    public delegate void SetRightsEventHandle(object sender,
    SetRightsEventArgs e);

    public class SetRightsEventArgs : EventArgs
    {
        public TreeNode Node = null;
        public string Rights = "";
    }

    //

    // �õ�treenode�����style����
    public delegate void GetNodeStyleEventHandle(object sender,
     GetNodeStyleEventArgs e);

    public class GetNodeStyleEventArgs : EventArgs
    {
        public TreeNode Node = null;
        public int Style = 0;
    }
}
