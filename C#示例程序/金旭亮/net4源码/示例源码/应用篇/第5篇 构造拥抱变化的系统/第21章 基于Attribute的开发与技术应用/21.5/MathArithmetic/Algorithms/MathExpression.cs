//''�������ʽ��
//''�����ʽ������һ���ִ����飬ͨ���±��������ض����ַ���
using System;
namespace MathArithmetic
{
    public class MathExpression
    {
        //����������ʽ
        private string _expr;
        public string Expression
        {
            get
            {
                return _expr;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("�յ��������ʽ");
                }
                _expr = value;
            }
        }

        //�ִ��еĵ�ǰ�ַ�����
        private int _index = 0;
        public int CurIndex
        {
            get
            {
                return _index;
            }
            set
            {
                if (value < 0)
                    throw new Exception("�ַ��������������0");

                if (value >= _expr.Length)
                    throw new Exception("�ַ�����������С���ִ�����");

                _index = value;
            }
        }


        /// <summary>
        /// ���ݵ�ǰ�������ص�ǰ�ַ������޸�����λ��
        /// </summary>
        /// <returns></returns>
        public char GetCurChar()
        {
            if (CurIndex >= _expr.Length)
                throw new ArgumentOutOfRangeException("���������˱��ʽ���ַ�������");

            return _expr[CurIndex];
        }


        /// <summary>
        /// �����ַ����������ض����ַ�
        /// </summary>
        /// <param name="index">�ַ�����</param>
        /// <returns></returns>
        public char GetChar(int index)
        {
            if (index < 0 || index > _expr.Length - 1)

                throw new CalculatorException("�ַ�����Խ��");

            if (_expr.Length == 0)
                throw new NullReferenceException("�ִ�Ϊ��");

            return _expr[index];
        }

        public MathExpression(string expr)
        {
            _expr = expr;
            _index = 0;
        }

        public MathExpression()
        {
            _expr = "";
            _index = 0;
        }

    }
}