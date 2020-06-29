using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.SqlClient;

public class DkywInterface : MarshalByRefObject
{
    public const int WM_USER = 0x0400;
    public const int WM_THREADEND = WM_USER + 200;
    public const int WM_GETPORT = WM_USER + 201;
    public const int WM_DISABLESENDKEY = WM_USER + 202;  // ����ۿ�״̬
    public const int WM_ENABLESENDKEY = WM_USER + 203;    // �˳��ۿ�״̬
    public const int WM_GETNEXTSEQUENCENUMBER = WM_USER + 204;    // �����ˮ��


    [DllImport("user32")]
    public static extern bool
        PostMessage(IntPtr hWnd, int Msg,
        int wParam, int lParam);

    // SendMessage
    [DllImport("user32")]
    public static extern IntPtr
        SendMessage(IntPtr hWnd, uint Msg,
        UIntPtr wParam, IntPtr lParam);

    [DllImport("user32")]
    public static extern IntPtr
        SendMessage(IntPtr hWnd, int Msg,
        IntPtr wParam, IntPtr lParam);

    [DllImport("user32")]
    public static extern IntPtr
        SendMessage(IntPtr hWnd, uint Msg,
        int wParam, int lParam);

    // 
    /*
1��rf_link_com_pro():�򿪴���
��  ��: rf_link_com_pro(incom : integer) : integer
��  ��: incom     : ���Ӷ������Ĵ���,�ֱ��ʾCOM1��COM4(1..4)
����ֵ: ����ֵ����
     =0 �ɹ�
 * */
    [DllImport("rf_card_pro.dll")]
    public static extern int rf_link_com_pro(int com_port);

    /*
2��rf_unlink_com_pro():�رմ���
 ��  ��: rf_unlink_com_pro(incom : integer) : integer
 ��  ��: incom     : ���Ӷ������Ĵ���,�ֱ��ʾCOM1��COM4(1..4)
 ����ֵ: ����ֵ����
         =0 �ɹ�
     * */
    [DllImport("rf_card_pro.dll")]
    public static extern int rf_unlink_com_pro(int com_port);

    /*
3��rf_Beep_pro():����
 ��  ��: rf_Beep_pro(nums : integer) : integer
 ��  ��:  nums  : ��������
 ����ֵ: ����ֵ����
         =0 �ɹ�
     * */
    [DllImport("rf_card_pro.dll")]
    public static extern int rf_Beep_pro(int times);

    /*
4��rf_card_p():�ж϶��������Ƿ��п�
 ��  ��: rf_card_p() : integer
 ��  ��: �޲���
 ����ֵ: ����ֵ����
         =0 �ɹ�  ��ʾ�п�
     * */
    [DllImport("rf_card_pro.dll")]
    public static extern int rf_card_p();

    /*
5��ReadCardNo_pro():��ȡ�����
 ��  ��: ReadCardNo_pro(incom : integer;
     * user_code :pchar;
     * card_key : pchar;
     * Card_id : pchar;
     * Card_no : pchar) : integer
 ��  ��: incom     : ���Ӷ������Ĵ���,�ֱ��ʾCOM1��COM4(1..4)
     user_code : �û�����
     card_key  : ����Կ
         card_id   : ���ؿ���Ψһ���к�
         card_no   : ���ؿ����
 ����ֵ: ����ֵ����
         =0 �ɹ�  
     * */
    [DllImport("rf_card_pro.dll")]
    public static extern int ReadCardNo_pro(int com_port,
        string user_code,
        string card_key,
        StringBuilder card_id,
        StringBuilder card_no);

    /*
7��ReadCardMsg(): ������Ψһ���кš����źͿ��ڽ��
     ��  ��: code=ReadCardMsg(incom : integer;
     * user_code :pchar;
     * card_key : pchar;
     * Card_id : pchar;
     * Card_no : pchar;
     * limit_money : pchar) : integer
     ��  ��: incom     : ���Ӷ������Ĵ���,�ֱ��ʾCOM1��COM4(1..4)
	     user_code : �û�����
	     card_key  : ����Կ
             card_id   : ���ؿ���Ψһ���к�
             card_no   : ���ؿ���
            limit_money: ���ؿ��������޶�(�Է�Ϊ��λ)	  
	
     ����ֵ: ����ֵ����
                >0: �ɹ�,���ؿ��ڽ��,card_id(����Ψһ���к�),card_no(���ؿ���),limit_money(���ؿ��������޶�);
		-1:���Ӵ��ڴ���;
                -2:û�з��ֿ�Ƭ;
                -3:�޷���ȡ����Ψһ���к�; 
                -4:װ����Կ����;
		-5:��������;

     ˵  ��: �˺�������������Ψһ���кš����źͿ��ڽ��
     ��  ��: code=ReadCardMsg(1,'12345678','1122334455667788','','');
   
     * */
    [DllImport("rf_card_pro.dll")]
    public static extern int ReadCardMsg(int com_port,
        string user_code,
        string card_key,
        StringBuilder card_id,
        StringBuilder card_no,
        StringBuilder limit_money);

    /*
8��rf_WriteCard():�ӿ��м�Ǯ����
     ��  ��: code=rf_WriteCard(incom : integer;
     user_code :pchar;
     card_key : pchar;
     user_pwd : pchar;
     inmoney : integer;
     card_money : pchar) : integer
     ��  ��: incom     : ���Ӷ������Ĵ���,�ֱ��ʾCOM1��COM4(1..4)
             user_code : �û�����
	     card_key  : ����Կ
             user_pwd  : �û�����
             inmoney   : Ҫ�ӿ��п�ȥ�Ľ��(�Է�Ϊ��λ)	
             card_money: ���ؿ������
     ����ֵ: ����ֵ����
                >0 : �ɹ�,����д������ۼ��ÿ�����;
		-1:���Ӵ��ڴ���;
                -2:û�з��ֿ�Ƭ;
                -3:�޷���ȡ����Ψһ���к�; 
                -4:װ����Կ����;
		-5:��������;
		-6:���ѹ�����Ч��;
		-7:�������
		-8:����Ľ��̫��;
		-9:д��ʧ��;
     ˵  ��: �˺����������û��Ŀ��п�Ǯ��
���� ��  ��: code=rf_WriteCard(1,'12345678','1122334455667788','123456',200);
     * */
    [DllImport("rf_card_pro.dll")]
    public static extern int rf_WriteCard(int com_port,
        string user_code,
        string card_key,
        string user_password,
        int sub_money,
        StringBuilder card_money);

    static string GetErrorString(int nErrorCode)
    {
        switch (nErrorCode)
        {
            case -1:
                return "���Ӵ��ڴ���";
            case -2:
                return "û�з��ֿ�Ƭ";
            case -3:
                return "�޷���ȡ����Ψһ���к�";
            case -4:
                return "װ����Կ����";
            case -5:
                return "��������";
            case -6:
                return "���ѹ�����Ч��";
            case -7:
                return "�������";
            case -8:
                return "����Ľ��̫��";
            case -9:
                return "д��ʧ��";
        }

        return "δ֪�Ĵ��� " + nErrorCode.ToString();
    }

    public DkywInterface()
    {
        // Console.WriteLine("Constructor called");
    }

    // ��ֹSendKey״̬
    public void DisableSendKey()
    {
        Form form = Application.OpenForms[0];
        IntPtr r = SendMessage(form.Handle,
            WM_DISABLESENDKEY,
            0,
            0);
    }

    // �ָ�SendKey״̬
    public void EnableSendKey()
    {
        Form form = Application.OpenForms[0];
        IntPtr r = SendMessage(form.Handle,
            WM_ENABLESENDKEY,
            0,
            0);
    }
    /*
    // ���פ���������õĲ���
    // return:
    //      -1  error
    //      0   not found label control
    //      1   succeed
    static int GetParameters(out int nPort,
        out string strUserCode,
        out string strCardKey,
        out string strError)
    {
        nPort = 0;
        strUserCode = "";
        strCardKey = "";
        strError = "";

        Form form = Application.OpenForms[0];
        for(int i=0;i<form.Controls.Count;i++)
        {
            Control control = form.Controls[i];

            if (control is Label)
            {
                string strText = ((Label)control).Text;
                if (strText.Length > 0 && strText[0] == '@')
                {
                    strText = strText.Substring(1);

                    string [] parts = strText.Split(new char [] {','});
                    if (parts.Length != 3)
                    {
                        strError = "������ʽ����Ӧ��Ϊ3����";
                        return -1;
                    }

                    try {
                    nPort = Convert.ToInt32(parts[0]);
                    }
                    catch {
                        strError = "�˿ں����ָ�ʽ���� '" + parts[0] + "'";
                        return -1;
                    }

                    strUserCode = parts[1];
                    strCardKey = parts[2];
                    return 1;
                }
            }

        }

        strError = "not found label control";
        return 0;
    }
    */

        // ���פ���������õĲ���
    // ��װ�汾
    // return:
    //      -1  error
    //      0   not found label control
    //      1   succeed
    static int GetParameters(out int nPort,
        out string strUserCode,
        out string strCardKey,
        out string strError)
    {
        string strSqlConnectionString = "";
        string strSqlDbName = "";
        string strAccount = "";
        string strSubject = "";

        return GetParameters(out nPort,
            out strUserCode,
            out strCardKey,
            out strSqlConnectionString,
            out strSqlDbName,
            out strAccount,
            out strSubject,
            out strError);
    }

    // ���פ���������õĲ���
    // return:
    //      -1  error
    //      0   not found label control
    //      1   succeed
    static int GetParameters(out int nPort,
        out string strUserCode,
        out string strCardKey,
        out string strConnectionString,
        out string strSqlDbName,
        out string strAccount,
        out string strSubject,
        out string strError)
    {
        nPort = 0;
        strUserCode = "";
        strCardKey = "";
        strConnectionString = "";
        strSqlDbName = "";
        strAccount = "";
        strSubject = "";
        strError = "";

        Form form = Application.OpenForms[0];
        for (int i = 0; i < form.Controls.Count; i++)
        {
            Control control = form.Controls[i];

            if (control is Label)
            {
                string strText = ((Label)control).Text;
                if (strText.Length > 0 && strText[0] == '@')
                {
                    strText = strText.Substring(1);

                    string[] parts = strText.Split(new char[] { '|' });
                    if (parts.Length != 7)
                    {
                        strError = "������ʽ����Ӧ��Ϊ7����";
                        return -1;
                    }

                    try
                    {
                        nPort = Convert.ToInt32(parts[0]);
                    }
                    catch
                    {
                        strError = "�˿ں����ָ�ʽ���� '" + parts[0] + "'";
                        return -1;
                    }

                    strUserCode = parts[1];
                    strCardKey = parts[2];
                    strConnectionString = parts[3];
                    strSqlDbName = parts[4];
                    strAccount = parts[5];
                    strSubject = parts[6];
                    return 1;
                }
            }

        }

        strError = "not found label control";
        return 0;
    }

    // parameters:
    //      strRest    ���ؽ���ԪΪ��λ
    //      nErrorCode ԭʼ������
    //          -1:���Ӵ��ڴ���;
    //          -2:û�з��ֿ�Ƭ;
    //          -3:�޷���ȡ����Ψһ���к�; 
    //          -4:װ����Կ����;
    //          -5:��������;
    // return:
    //      -1  ����
    //      0   û�п�
    //      1   �ɹ������Ϣ
    public int GetCardInfo(out string strCardNumber,
        out string strRest,
        out string strLimitMoney,
        out int nErrorCode,
        out string strError)
    {
        strError = "";
        strCardNumber = "";
        strRest = "";
        strLimitMoney = "";
        nErrorCode = 0;

        int nPort = 0;
        string strUserCode = "";
        string strCardKey = "";

        // ��ò���
        // return:
        //      -1  error
        //      0   not found label control
        //      1   succeed
        int nRet = GetParameters(out nPort,
            out strUserCode,
            out strCardKey,
            out strError);
        if (nRet != 1)
            return -1;

        StringBuilder card_id = new StringBuilder(255);
        StringBuilder card_no = new StringBuilder(255);
        StringBuilder limit_money = new StringBuilder(255);


        nRet = ReadCardMsg(nPort,
            strUserCode,
            strCardKey,
            card_id,
            card_no,
            limit_money);

        if (nRet > 0)
        {
            strCardNumber = card_no.ToString();
            strLimitMoney = limit_money.ToString();

            // ת��ΪԪ
            Decimal v = Convert.ToDecimal(strLimitMoney);
            v = v / 100;

            strLimitMoney = v.ToString();

            // ת��ΪԪ
            v = Convert.ToDecimal(nRet);
            v = v / 100;
            strRest = v.ToString();

            return 1;
        }

        if (nRet == -2)
        {
            strError = "�Ķ�����û��IC��";
            return 0;
        }

        strError = GetErrorString(nRet) + "��������:" + nRet.ToString();

        nErrorCode = nRet;
        return -1;
    }

    // �����ˮ�š�ÿ���һ�Σ��Զ�����һ��
    int GetSequenceNumber()
    {
        Form form = Application.OpenForms[0];
        IntPtr r = SendMessage(form.Handle,
            WM_GETNEXTSEQUENCENUMBER,
            0,
            0);

        return r.ToInt32();
    }

    // �ۿ�
    // parameters:
    //      strCardNumber   Ҫ��Ŀ��š����Ϊ�գ����ʾ��Ҫ�󿨺ţ�ֱ�Ӵӵ�ǰ���Ͽۿ�
    //      strSubMoney Ҫ�۵Ŀ����磺"0.01"
    //      strUsedCardNumber   ʵ�ʿۿ�Ŀ���
    //      strRest    �ۿ������
    //      nErrorCode ԭʼ������
    //          -1:���Ӵ��ڴ���;
    //          -2:û�з��ֿ�Ƭ;
    //          -3:�޷���ȡ����Ψһ���к�; 
    //          -4:װ����Կ����;
    //          -5:��������;
    //          -6:���ѹ�����Ч��;
    //          -7:�������
    //          -8:����Ľ��̫��;
    //          -9:д��ʧ��;
    // return:
    //      -1  ����
    //      0   û�п�
    //      1   �ɹ��ۿ�ͻ����Ϣ
    //      2   ��Ȼ�ۿ�ɹ��������ϴ���ˮʧ��
    public int SubCardMoney(string strCardNumber,
        string strSubMoney,
        string strPassword,
        out string strUsedCardNumber,
        out string strRest,
        out int nErrorCode,
        out string strError)
    {
        strError = "";
        strCardNumber = "";
        strRest = "";
        strUsedCardNumber = "";
        nErrorCode = 0;

        int nPort = 0;
        string strUserCode = "";
        string strCardKey = "";
        string strSqlConnectionString = "";
        string strSqlDbName = "";
        string strAccount = "";
        string strSubject = "";

        // ��ò���
        // return:
        //      -1  error
        //      0   not found label control
        //      1   succeed
        int nRet = GetParameters(out nPort,
            out strUserCode,
            out strCardKey,
            out strSqlConnectionString,
            out strSqlDbName,
            out strAccount,
            out strSubject,
            out strError);
        if (nRet != 1)
            return -1;

        int nProcAcc = 0;

        try
        {
            nProcAcc = Convert.ToInt32(strAccount);
        }
        catch
        {
            strError = "�ʻ� '" + strAccount + "' Ӧ��Ϊ������ֵ";
            return -1;
        }

        StringBuilder card_id = new StringBuilder(255);
        StringBuilder card_no = new StringBuilder(255);
        StringBuilder limit_money = new StringBuilder(255);

        // string strThisCardNumber = "";

        nRet = ReadCardMsg(nPort,
            strUserCode,
            strCardKey,
            card_id,
            card_no,
            limit_money);
        if (nRet > 0)
        {
            strUsedCardNumber = card_no.ToString();

            // ת��ΪԪ
            Decimal v = Convert.ToDecimal(nRet);
            v = v / 100;
            strRest = v.ToString();
        }
        else
        {
            if (nRet == -2)
            {
                strError = "�Ķ�����û��IC��";
                return 0;
            }

            strError = GetErrorString(nRet) + "��ReadCard������:" + nRet.ToString();

            nErrorCode = nRet;
            return -1;
        }

        // �жϿ���
        if (String.IsNullOrEmpty(strCardNumber) == false)
        {
            if (strCardNumber != strUsedCardNumber)
            {
                strError = "�������ϷŵĿ��� '" + strUsedCardNumber + "' ����������Ŀ��� '" + strCardNumber + "'";
                return -1;
            }
        }

        // �ж�����Ƿ��㹻

        /* ���� ����ۿ����
        strError = "test error";
        nErrorCode = -6;
        return -1;
         * */

        /* ģ���������
        if (strPassword != "9")
        {
            strError = "test error";
            nErrorCode = -7;
            return -1;
        }*/


        // ����Ϊ�ֵ�λ
        decimal sub_v = Convert.ToDecimal(strSubMoney);
        int sub_money = Convert.ToInt32(sub_v * 100);

        StringBuilder card_money = new StringBuilder(255);

        // �ۿ�
        nRet = rf_WriteCard(nPort,
            strUserCode,
            strCardKey,
            strPassword,
            sub_money,
            card_money);

        if (nRet > 0)
        {
            int nUseCount = nRet;   // �ÿ�����

            // �ۿ�ǰ�����
            strRest = card_money.ToString();

            int nBalance = 0;

            try
            {
                nBalance = Convert.ToInt32(strRest);
            }
            catch
            {
                strError = "�ۿ�ǰ��� '" + strRest + "' ��ʽ����ȷ��Ӧ��Ϊ������";
                return -1;
            }

            // ת��ΪԪ
            Decimal v = Convert.ToDecimal(strRest);
            v = v / 100;

            // �µ����
            v -= sub_v;

            strRest = v.ToString();


            // �ϴ���ˮ
            if (String.IsNullOrEmpty(strSqlConnectionString) == false)
            {
                int nSequenceNumber = GetSequenceNumber();

                string strDateTime = "";

                strDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                // parameters:
                //      nAmount �ۿ��Է�Ϊ��λ
                //      nCount  �ۼ��ÿ�����(ע����API����ֵ+1��ֵ)
                //      nBalance    �ۿ�ǰ�����Է�Ϊ��λ
                //      nSequeceNumber  ��ˮ��
                //      strDateTime �������� 14λ�ַ�
                //      nProcAcc    �ʺ�
                //      strSubject  ��Ŀ����
                nRet = UploadInfo(
                    card_no.ToString().TrimStart(new char[] { '0' }),
                    card_id.ToString().TrimStart(new char[] { '0' }),
                    sub_money,
                    nUseCount + 1,
                    nBalance,
                    nSequenceNumber,
                    strSqlConnectionString,
                    strSqlDbName,
                    strDateTime,
                    nProcAcc,
                    strSubject,
                    out strError);
                if (nRet == -1)
                {
                    strError = "��Ȼ�ϴ���ˮʧ�ܣ����Ǵ�IC���ۿ��Ѿ��ɹ�������ԭ��: " + strError;
                    return 2;
                }
            }

            return 1;
        }

        if (nRet == -2)
        {
            strError = "�Ķ�����û��IC��";
            return 0;
        }

        strError = GetErrorString(nRet) + "��WriteCard������:" + nRet.ToString();

        nErrorCode = nRet;
        return -1;
    }

    // parameters:
    //      nAmount �ۿ��Է�Ϊ��λ
    //      nCount  �ۼ��ÿ�����(ע����API����ֵ+1��ֵ)
    //      nBalance    �ۿ�ǰ�����Է�Ϊ��λ
    //      nSequeceNumber  ��ˮ��
    //      strDateTime �������� 14λ�ַ�
    //      nProcAcc    �ʺ�
    //      strSubject  ��Ŀ����
    int UploadInfo(
        string strCardNo,
        string strCardID,
        int nAmount,
        int nCount,
        int nBalance,
        int nSequanceNumber,
        string strConnectionString,
        string strSqlDbName,
        // string strSystemNo,
        // string strComputerCode,
        string strDateTime,
        int nProcAcc,
        string strSubject,
        out string strError)
    {
        strError = "";

        try
        {
            SqlConnection connection = new SqlConnection(strConnectionString);
            connection.Open();

            SqlCommand command = new SqlCommand("", connection);

            string strCommand = "";
            strCommand = "use " + strSqlDbName;

            strCommand += " INSERT INTO Jour_list "
    + " (CARDNO,CARDID,TRCD,OLDTRCD,TRANAMT,SYSTEMNO,CompCode,PosCode,PassWord,JOURNO,JNTOTAL,BALANCE,JDATETIME,PROCACC,SUBJECT,JFlag) "
    + " VALUES(@CARDNO,@CARDID,@TRCD,@OLDTRCD,@TRANAMT,@SYSTEMNO,@CompCode,@PosCode,@PassWord,@JOURNO,@JNTOTAL,@BALANCE,@JDATETIME,@PROCACC,@SUBJECT,@JFlag)";

            // �����
            command.Parameters.Add("@CARDNO",
                SqlDbType.VarChar).Value = strCardNo;

            // �����к�
            command.Parameters.Add("@CARDID",
                SqlDbType.VarChar).Value = strCardID;

            // ���״���
            command.Parameters.Add("@TRCD",
                SqlDbType.Char).Value = "1210";

            // ԭ���״���
            command.Parameters.Add("@OLDTRCD",
                SqlDbType.Char).Value = "";

            // �ۿ����
            command.Parameters.Add("@TRANAMT",
                SqlDbType.Int).Value = nAmount;

            // ϵͳ����
            command.Parameters.Add("@SYSTEMNO",
                SqlDbType.Char).Value = "0003";

            // ���ؼ��������
            command.Parameters.Add("@CompCode",
                SqlDbType.Char).Value = "0000";

            // POS����
            command.Parameters.Add("@PosCode",
                SqlDbType.Char).Value = "1616";

            // ����
            command.Parameters.Add("@PassWord",
                SqlDbType.Char).Value = "";

            // ��ˮ��
            command.Parameters.Add("@JOURNO",
                SqlDbType.Int).Value = nSequanceNumber;

            // �ۼ��ÿ�����
            command.Parameters.Add("@JNTOTAL",
                SqlDbType.Int).Value = nCount;

            // ����ǰ���
            command.Parameters.Add("@BALANCE",
                SqlDbType.Int).Value = nBalance;

            // ��������
            command.Parameters.Add("@JDATETIME",
                SqlDbType.Char).Value = strDateTime;

            // �ʺ�
            command.Parameters.Add("@PROCACC",
                SqlDbType.Int).Value = nProcAcc;

            // ��Ŀ����
            command.Parameters.Add("@SUBJECT",
                SqlDbType.Char).Value = strSubject; // "0243010000"

            command.Parameters.Add("@JFlag",
                SqlDbType.Char).Value = "0";

            command.CommandText = strCommand;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                strError = "SQL����ִ�г���ԭ��" + ex.Message;
                return -1;
            }
            strCommand = "";
            command.Parameters.Clear();
        }
        catch (Exception ex)
        {
            strError = "Exception: " + ex.Message;
            return -1;
        }
        finally
        {
        }
        return 0;
    }

    /*
    public string Greeting(string name)
    {
        Form form = Application.OpenForms[0];
        IntPtr r = SendMessage(form.Handle,
            WM_GETPORT,
            0,
            0);

        return "Hello," + name + "  port:" + r.ToInt32().ToString();

        // return ChildTree(0, ForegroundWindow.Instance.Handle);
    }*/

}


