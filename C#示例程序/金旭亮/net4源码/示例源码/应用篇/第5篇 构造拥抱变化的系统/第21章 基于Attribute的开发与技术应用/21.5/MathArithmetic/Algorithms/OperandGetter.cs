using System;
namespace MathArithmetic
{
    //����ʹ�������Զ����㷨�ӱ��ʽ�ִ��г�ȡһ������
    //����һ��MathExpression���󣬴��䵱ǰ������ʼ��ȡ��һ����Ч�Ĳ���������ɲ���������ֵ
    //Ϊȡ�������������һ���ַ����ѵ��ִ�β���򲻶���
    //���ʹ��ʱ,��������������һ������,����,���಻���ȡ���κ��ַ�,Ҳ���޸��ַ�����

    public enum DFAState
    {
        q0, //��ʼ״̬
        qN,//����״̬ N=Negative
        qI,//������״̬ I=Integer
        qF,//��������״̬ F=Float
        qQ  //�˳�״̬ Q=Quit
    }
    /// <summary>
    /// ��ȡ������
    /// </summary>
    public class OperandGetter
    {
        #region "��ʼ����"
        //�������ʽ������紫��
        private MathExpression _expr = null;

        public MathExpression Expression
        {
            get
            {
                return _expr;
            }
            set
            {
                _expr = value;
            }
        }

        ///���캯��
        ///
        public OperandGetter(MathExpression expr)
        {
            _expr = expr;
        }
        #endregion

        //�Զ������еĽ��
        private string _result = "";  //���ڴ�ų�ȡ���������ִ�
        public string result
        {
            get
            {
                return _result;
            }

        }


        private DFAState curState;  //��ǰ�Զ�����״̬


        /// <summary>
        /// ͨ�������Զ�����״̬ת����ȡ����
        /// </summary>
        public void Run()
        {

            //����ԭ�ȵ��ַ�����,ֻ����״̬ת��ʱ�ż�¼
            int OldCharIndex = _expr.CurIndex;

            //�Ƿ�ǰ�ַ����������ִ������һ���ַ�
            bool IsEndOfString;

            //���������Զ����ĳ�̬
            curState = DFAState.q0;


            char curChar; //��ǰ�ַ�
            do
            {
                IsEndOfString = (_expr.CurIndex == _expr.Expression.Length - 1);
                curChar = _expr.GetCurChar();

                switch (curState)
                {
                    case DFAState.q0:

                        if (curChar == '-')//�Ƿ���ţ�
                        {
                            curState = DFAState.qN;//״̬ת����qN���������ڴ��ڸ���״̬
                            if (!IsEndOfString)
                                _expr.CurIndex += 1; //��ȡ��һ���ַ�
                            else //��ʼ̬����������ַ������˳�
                                if (!Char.IsDigit(curChar))
                                    curState = DFAState.qQ;
                        }
                        if (Char.IsDigit(curChar))//Ϊ�����ַ�
                        {
                            curState = DFAState.qI; //״̬ת����qI,�������ڴ�����������״̬
                            if (!IsEndOfString)
                                _expr.CurIndex += 1; //��ȡ��һ���ַ�
                        }
                        break;

                    case DFAState.qN:

                        if (Char.IsDigit(curChar))
                        {
                            curState = DFAState.qI;
                            if (!IsEndOfString)
                                _expr.CurIndex += 1; //��ȡ��һ���ַ�
                        }
                        else //�����֣��˳�
                            curState = DFAState.qQ;

                        break;

                    case DFAState.qF:
                        if (!Char.IsDigit(curChar))//�������ַ�,�˳�
                            curState = DFAState.qQ;
                        else
                            if (!IsEndOfString)
                                _expr.CurIndex += 1; //��ȡ��һ���ַ�
                        break;

                    case DFAState.qI:
                        if (curChar == '.')
                        {
                            curState = DFAState.qF;  //����С���㣬״̬ת����qF
                            if (!IsEndOfString)
                                _expr.CurIndex += 1; //��ȡ��һ���ַ�
                        }
                        else
                        {
                            if (!Char.IsDigit(curChar)) //�Ȳ������֣��ֲ���С����
                                curState = DFAState.qQ;
                            else
                               if (!IsEndOfString)
                                    _expr.CurIndex += 1;//��ȡ��һ���ַ�
              
                        }
                        break;

                    case DFAState.qQ:
                        break;

                }

            } while (curState != DFAState.qQ && !IsEndOfString);//ֻҪ�������˳�״̬���Լ���û�ж����ִ���������һֱѭ��



            if (_expr.CurIndex == _expr.Expression.Length - 1 && Char.IsDigit(curChar))

                //ȡ�����һ������
                _result = _expr.Expression.Substring(OldCharIndex, _expr.CurIndex - OldCharIndex + 1);
            else
                //���ִ��м�ȡ����
                _result = _expr.Expression.Substring(OldCharIndex, _expr.CurIndex - OldCharIndex);

        }

    }
}