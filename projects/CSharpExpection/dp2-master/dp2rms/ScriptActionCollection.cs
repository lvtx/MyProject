using System;
using System.Collections;

namespace dp2rms
{
	// �ű���������
	public class ScriptAction
	{
		public string Name = "";
		public string Comment = "";
		public string ScriptEntry = "";	// �ű���ں�����

		public bool Active = false;


	}

	/// <summary>
	/// �ű��������Ƽ���
	/// </summary>
	public class ScriptActionCollection : ArrayList
	{
		public ScriptActionCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// ����һ��������
		public ScriptAction NewItem(string strName,
			string strComment,
			string strScriptEntry,
			bool bActive)
		{
			ScriptAction item = new ScriptAction();
			item.Name = strName;
			item.Comment = strComment;
			item.ScriptEntry = strScriptEntry;
			item.Active = bActive;

			this.Add(item);
			return item;
		}
	}
}
