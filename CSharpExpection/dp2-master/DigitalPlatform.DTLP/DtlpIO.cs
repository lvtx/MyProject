using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace DigitalPlatform.DTLP
{
	/// <summary>
	/// ����dtlp��¼��������Ľӿ���
	/// </summary>
	public class DtlpIO
	{
        DtlpChannelArray Channels = null;
		DtlpChannel channel = null;	// DTLPͨ��
		// public IWin32Window owner = null;

		public string	m_strStartNumber = "";
		public string	m_strEndNumber = "";
		string	m_strNextNumber = "";			// �����������һ����¼�ļ�¼��
		public string	m_strCurNumber = "";			// �Ѿ�������ĵ�ǰ��¼�ļ�¼��

		public string	m_strDBPath = "";

		public string	m_strRecord = "";
		public byte[]	m_baTimeStamp = new byte[9];

		public string	m_strPath = "";	// ��ǰ��¼·��

		public int ErrorNo = 0;

		public DtlpIO()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// ��ʼ��������
		// ���ⲿ����lChannel����ʹ�����һ��Ҫ����Ϊ-1������ᵼ�¶��Destroy()
		public int Initial(DtlpChannelArray channels,
			string strDBPath,
			string strStartNumber,
			string strEndNumber)
		{
            Debug.Assert(channels != null, "channels��������Ϊnull");

            this.Channels = channels;
			this.channel = this.Channels.CreateChannel(0);

			// Debug.Assert(channel != null, "channel��������Ϊnull");


			/*
			pConnect->m_strDefUserName = strDefUserName;
			pConnect->m_strDefPassword = strDefPassword;
			*/

			Debug.Assert(strStartNumber != "",
				"strStartNumber��������Ϊ��");
			m_strStartNumber = strStartNumber;

			Debug.Assert(strEndNumber != "",
				"strEndNumber��������Ϊ��");
			m_strEndNumber = strEndNumber;

			Debug.Assert(strDBPath != "",
				"strDBPath��������Ϊ��");
			m_strDBPath = strDBPath;			// ���ݿ�·��

			return 0;
		}

		// �õ���һ����¼
		// return:
		//		-1	����
		//		0	����
		//		1	����ĩβ(����m_strEndNumber)
		//		2	û���ҵ���¼
		public int NextRecord(ref int nRecCount,
            out string strError)
		{
            strError = "";

			string strPath;
			string strNumber;
			string strNextNumber;
			byte[] baPackage = null;
		
			int nSearchStyle = DtlpChannel.XX_STYLE;
			int nRet;
			int nErrorNo;
			int nDirStyle = 0;	// ������
			byte[] baMARC = null;

            if (this.channel == null)
                this.channel = this.Channels.CreateChannel(0);

			Debug.Assert(m_strStartNumber != "",
				"m_strStartNumberֵ����Ϊ��");
			Debug.Assert(m_strEndNumber != "",
				"m_strEndNumberֵ����Ϊ��");

			// �״ν��뱾����
			if (nRecCount == -1) 
			{
				strNumber = m_strStartNumber;

				nDirStyle = 0;
				nRecCount = 0;
			}
			else 
			{
				strNumber = m_strNextNumber;

				if (Convert.ToInt64(m_strStartNumber) <= Convert.ToInt64(m_strEndNumber)) 
				{
					nDirStyle = DtlpChannel.NEXT_RECORD;
					if (Convert.ToInt64(strNumber) >= Convert.ToInt64(m_strEndNumber))
						return 1;	// ����
				}
				else 
				{
					nDirStyle = DtlpChannel.PREV_RECORD;
					if (Convert.ToInt64(strNumber) <= Convert.ToInt64(m_strEndNumber))
						return 1;	// ����
				}
			}


			strPath = m_strDBPath;
			strPath += "/ctlno/";
			strPath += strNumber;

	//		REDO:
				nRet = channel.Search(strPath,
					nSearchStyle | nDirStyle,
					out baPackage);
			if (nRet == -1) 
			{
				nErrorNo  = channel.GetLastErrno();
				if (nErrorNo == DtlpChannel.GL_NOTLOGIN) 
				{
					// ���µ�¼�����飬�Ѿ���Search()�ӹ���
				}
				else 
				{
					if (nErrorNo == DtlpChannel.GL_NOTEXIST) 
					{
						// ��������
						Int64 n64Number = Convert.ToInt64(strNumber);
						string strVersion;

						// GetDTLPVersion(m_strDBPath, strVersion);	// ���Խ�������ȥ����Ч������
						strVersion = "0.9";
				
						if (n64Number+1<9999999
							&& strVersion == "0.9") 
						{
							//strNumber.Format(_T("%I0764d"), n64Number+1);
							// ȷ��7λ����
							strNumber = String.Format("{0:D7}", n64Number+1);   // "{0,7:D}" BUG!!! ���ʵ�����ǿո� 2009/2/26
						}
						else 
						{
							// strNumber.Format(_T("%I64d"), n64Number+1);
                            strNumber = String.Format("{0:D7}", n64Number + 1);  // "{0,7:D}" BUG!!! ���ʵ�����ǿո� 2009/2/26
						}


						m_strCurNumber = strNumber;
						goto NOTFOUND;
					}

					this.ErrorNo = nErrorNo;

					if (nErrorNo == DtlpChannel.GL_INVALIDCHANNEL)
						this.channel = null;

					// �õ��ַ�������
                    /*
					channel.ErrorBox(owner, "DtlpIO",
						"������������\nSearch() error");
                     * */
                    strError = "������������\nSearch() error: \r\n" + channel.GetErrorDescription();
					goto ERROR1;
				}

				m_strPath = strPath;
	
				goto ERROR1;
			}


			/// 
			Package package = new Package();

			package.LoadPackage(baPackage, channel.GetPathEncoding(strPath));
			nRet = package.Parse(PackageFormat.Binary);
			if (nRet == -1) 
			{
				Debug.Assert(false, "Package::Parse() error");
                strError = "Package::Parse() error";
				goto ERROR1;
			}

			nRet = package.GetFirstBin(out baMARC);
			if (nRet == -1 || nRet == 0) 
			{
				Debug.Assert(false, "Package::GetFirstBin() error");
                strError = "Package::GetFirstBin() error";
				goto ERROR1;
			}

			if (baMARC.Length >= 9) 
			{
				Array.Copy(baMARC, 0, m_baTimeStamp, 0, 9);

				byte [] baBody = new byte[baMARC.Length - 9];
				Array.Copy(baMARC, 9, baBody, 0, baMARC.Length - 9);
				baMARC = baBody;

				//baMARC.RemoveAt(0, 9);	// ʱ���
			}
			else 
			{
				// ��¼�����⣬����һ���ռ�¼?
			}

			// ---????????? ��дһ����byte[]ĩβ׷�Ӷ����ĺ���?

			// baMARC = ByteArray.Add(baMARC, (byte)0);
			// baMARC.Add(0);
			// baMARC.Add(0);

			m_strRecord = channel.GetPathEncoding(strPath).GetString(baMARC);

			strPath = package.GetFirstPath();

			nRet = GetCtlNoFromPath(strPath,
				out strNextNumber);
			if (nRet == -1) 
			{
				Debug.Assert(false, "GetCtlNoFromPath() return error ...");
				strError = "GetCtlNoFromPath() error ...";
				goto ERROR1;
			}

			m_strNextNumber = strNextNumber;

			if (nRecCount == 0)	// �״�
				m_strCurNumber = strNumber;
			else
				m_strCurNumber = strNextNumber;

			m_strPath = strPath;	//

			nRecCount ++;
			return 0;
			ERROR1:
				return -1;
			NOTFOUND:
				return 2;
		}

        // ������ǰ���룬�Ա������ȡ����ļ�¼
        // 2009/2/26
        public void IncreaseNextNumber()
        {
            // ��������
            string strNumber = this.m_strNextNumber;

            Int64 n64Number = Convert.ToInt64(strNumber);
            string strVersion;

            // GetDTLPVersion(m_strDBPath, strVersion);	// ���Խ�������ȥ����Ч������
            strVersion = "0.9";

            if (n64Number + 1 < 9999999
                && strVersion == "0.9")
            {
                //strNumber.Format(_T("%I0764d"), n64Number+1);
                // ȷ��7λ����
                strNumber = String.Format("{0:D7}", n64Number + 1);
            }
            else
            {
                // strNumber.Format(_T("%I64d"), n64Number+1);
                strNumber = String.Format("{0:D7}", n64Number + 1);
            }

            this.m_strNextNumber = strNumber;
        }

		// �õ������濪ʼ�ĵ�һ��'/'���ҵĲ���
		// return:
		//		-1	error
		//		���� strRightPart�ĳ���
		static int GetCtlNoFromPath(string strPath,
			out string strRightPart)
		{
			Debug.Assert(strPath != null, "strPath��������Ϊnull");

			string strTemp = strPath;
			int nRet;
	
			strRightPart = "";
			nRet = strTemp.LastIndexOf('/');
			if (nRet == -1)
				return -1;

			strTemp = strTemp.Substring(nRet+1);

			strRightPart = strTemp;

			return strRightPart.Length;
		}


		// У׼ת����Χ����β��
		// return:
		//		-1	����
		//		0	û�иı���β��
		//		1	У׼��ı�����β��
		//		2	��Ŀ����û�м�¼
		public int VerifyRange(out string strError)
		{
            strError = "";

			string strPath;
			byte[] baPackage;

			string strMinNumber;
			string strMaxNumber;
			int nRet;
			int style = DtlpChannel.JH_STYLE;
			int nErrorNo;

			bool bChanged = false;
			string strVersion;

            if (this.channel == null)
                this.channel = this.Channels.CreateChannel(0);

			Debug.Assert(m_strStartNumber != "",
				"m_strStartNumberֵ����Ϊ��");
			Debug.Assert(m_strEndNumber != "",
				"m_strEndNumberֵ����Ϊ��");

			// У׼����
			//REDO1:
				strPath = m_strDBPath;

			// GetDTLPVersion(m_strDBPath, strVersion);	// ���Խ�������ȥ����Ч������
			strVersion = "0.9";

			if (strVersion == "0.9")
				strPath += "/ctlno/9999999";	// 7λ
			else
				strPath += "/ctlno/9999999999";	// 10λ

			nRet = channel.Search(strPath,
				style | DtlpChannel.PREV_RECORD,
				out baPackage);
			if (nRet <= 0) 
			{
				nErrorNo  = channel.GetLastErrno();
				this.ErrorNo = nErrorNo;

				if (nErrorNo == DtlpChannel.GL_NOTEXIST)
					return 2;

				if (nErrorNo == DtlpChannel.GL_INVALIDCHANNEL)
					this.channel = null;

                /*
				channel.ErrorBox(owner,
                    "dp1Batch",
                    "У׼����ʱ��������\nSearch() style=PREV_RECORD error");
                 * */
                strError = "У׼����ʱ��������\nSearch() style=PREV_RECORD error: \r\n" + channel.GetErrorDescription();
				goto ERROR1;
			}

			/// 
			Package package = new Package();

			package.LoadPackage(baPackage, channel.GetPathEncoding(strPath));
			nRet = package.Parse(PackageFormat.Binary);
			if (nRet == -1) 
			{
                strError = "Package::Parse(PackageFormat.Binary) error";
				Debug.Assert(false, strError);
				goto ERROR1;
			}

			strPath = package.GetFirstPath();

			nRet = GetCtlNoFromPath(strPath,
				out strMaxNumber);
			if (nRet == -1) 
			{
                strError = "GetCtlNoFromPath() error ...";
				Debug.Assert(false, strError);
				goto ERROR1;
			}

            // 2007/8/18
            if (strVersion == "0.9")
                strMaxNumber = strMaxNumber.PadLeft(7, '0');	// 7λ
            else
                strMaxNumber = strMaxNumber.PadLeft(10, '0');	// 10λ


			if (Convert.ToInt64(m_strStartNumber) <= Convert.ToInt64(m_strEndNumber)) 
			{
				if (Convert.ToInt64(m_strEndNumber) > Convert.ToInt64(strMaxNumber)) 
				{
					m_strEndNumber = strMaxNumber;
					bChanged = true;
				}
			}
			else 
			{
				if (Convert.ToInt64(m_strStartNumber) > Convert.ToInt64(strMaxNumber)) 
				{
					m_strStartNumber = strMaxNumber;
					bChanged = true;
				}
			}


			// У׼��С��
			//REDO2:
				strPath = m_strDBPath;
			strPath += "/ctlno/0000000000";	// -

			nRet = channel.Search(strPath,
				style | DtlpChannel.NEXT_RECORD,
				out baPackage);
			if (nRet <= 0) 
			{
				nErrorNo = channel.GetLastErrno();

				this.ErrorNo = nErrorNo;

				if (nErrorNo == DtlpChannel.GL_NOTEXIST)
					return 2;
				if (nErrorNo == DtlpChannel.GL_INVALIDCHANNEL)
					this.channel = null;

                /*
				channel.ErrorBox(owner,
					"Batch",
					"У׼��С��ʱ��������\nSearch() style=NEXT_RECORD error");
                 * */
                strError = "У׼��С��ʱ��������\nSearch() style=NEXT_RECORD error: \r\n" + channel.GetErrorDescription();
				goto ERROR1;

			}

			/// 
			package.LoadPackage(baPackage, channel.GetPathEncoding(strPath));
			nRet = package.Parse(PackageFormat.Binary);
			if (nRet == -1) 
			{
                strError = "Package::Parse(PackageFormat.Binary) error";
				Debug.Assert(false, strError);
				goto ERROR1;
			}

			strPath = package.GetFirstPath();

			nRet = GetCtlNoFromPath(strPath,
				out strMinNumber);
			if (nRet == -1) 
			{
                strError = "GetCtlNoFromPath() error ...";
				Debug.Assert(false, strError);
				goto ERROR1;
			}

            // 2007/8/18
            if (strVersion == "0.9")
                strMinNumber = strMinNumber.PadLeft(7, '0');	// 7λ
            else
                strMinNumber = strMinNumber.PadLeft(10, '0');	// 10λ


			if (Convert.ToInt64(m_strStartNumber) <= Convert.ToInt64(m_strEndNumber)) 
			{
				if (Convert.ToInt64(m_strStartNumber) < Convert.ToInt64(strMinNumber)) 
				{
					m_strStartNumber = strMinNumber;
					bChanged = true;
				}
			}
			else 
			{
				if (Convert.ToInt64(m_strEndNumber) < Convert.ToInt64(strMinNumber)) 
				{
					m_strEndNumber = strMinNumber;
					bChanged = true;
				}
			}

			if (bChanged == true)
				return 1;
			else
				return 0;

			ERROR1:
				return -1;
		}

	}
}
