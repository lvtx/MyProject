using System;
namespace MathArithmetic
{
    //此类使用有穷自动机算法从表达式字串中抽取一个数字
    //接收一个MathExpression对象，从其当前索引开始，取出一个有效的操作数，完成操作后索引值
    //为取出操作数后的下一个字符（已到字串尾，则不动）
    //外界使用时,给的索引必须是一个数字,否则,此类不会抽取出任何字符,也不修改字符索引

    public enum DFAState
    {
        q0, //初始状态
        qN,//负数状态 N=Negative
        qI,//整数的状态 I=Integer
        qF,//浮点数的状态 F=Float
        qQ  //退出状态 Q=Quit
    }
    /// <summary>
    /// 提取操作数
    /// </summary>
    public class OperandGetter
    {
        #region "初始化区"
        //算术表达式，由外界传入
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

        ///构造函数
        ///
        public OperandGetter(MathExpression expr)
        {
            _expr = expr;
        }
        #endregion

        //自动机运行的结果
        private string _result = "";  //用于存放抽取出的数字字串
        public string result
        {
            get
            {
                return _result;
            }

        }


        private DFAState curState;  //当前自动机的状态


        /// <summary>
        /// 通过有穷自动机的状态转换获取数字
        /// </summary>
        public void Run()
        {

            //保存原先的字符索引,只有在状态转换时才记录
            int OldCharIndex = _expr.CurIndex;

            //是否当前字符索引代表字串的最后一个字符
            bool IsEndOfString;

            //设置有穷自动机的初态
            curState = DFAState.q0;


            char curChar; //当前字符
            do
            {
                IsEndOfString = (_expr.CurIndex == _expr.Expression.Length - 1);
                curChar = _expr.GetCurChar();

                switch (curState)
                {
                    case DFAState.q0:

                        if (curChar == '-')//是否减号？
                        {
                            curState = DFAState.qN;//状态转移至qN，表明现在处于负数状态
                            if (!IsEndOfString)
                                _expr.CurIndex += 1; //读取下一个字符
                            else //初始态输入非数字字符，则退出
                                if (!Char.IsDigit(curChar))
                                    curState = DFAState.qQ;
                        }
                        if (Char.IsDigit(curChar))//为数字字符
                        {
                            curState = DFAState.qI; //状态转移至qI,表明现在处理输入整数状态
                            if (!IsEndOfString)
                                _expr.CurIndex += 1; //读取下一个字符
                        }
                        break;

                    case DFAState.qN:

                        if (Char.IsDigit(curChar))
                        {
                            curState = DFAState.qI;
                            if (!IsEndOfString)
                                _expr.CurIndex += 1; //读取下一个字符
                        }
                        else //非数字，退出
                            curState = DFAState.qQ;

                        break;

                    case DFAState.qF:
                        if (!Char.IsDigit(curChar))//非数字字符,退出
                            curState = DFAState.qQ;
                        else
                            if (!IsEndOfString)
                                _expr.CurIndex += 1; //读取下一个字符
                        break;

                    case DFAState.qI:
                        if (curChar == '.')
                        {
                            curState = DFAState.qF;  //输入小数点，状态转移至qF
                            if (!IsEndOfString)
                                _expr.CurIndex += 1; //读取下一个字符
                        }
                        else
                        {
                            if (!Char.IsDigit(curChar)) //既不是数字，又不是小数点
                                curState = DFAState.qQ;
                            else
                               if (!IsEndOfString)
                                    _expr.CurIndex += 1;//读取下一个字符
              
                        }
                        break;

                    case DFAState.qQ:
                        break;

                }

            } while (curState != DFAState.qQ && !IsEndOfString);//只要不处于退出状态，以及还没有读到字串结束，就一直循环



            if (_expr.CurIndex == _expr.Expression.Length - 1 && Char.IsDigit(curChar))

                //取出最后一个数字
                _result = _expr.Expression.Substring(OldCharIndex, _expr.CurIndex - OldCharIndex + 1);
            else
                //在字串中间取数字
                _result = _expr.Expression.Substring(OldCharIndex, _expr.CurIndex - OldCharIndex);

        }

    }
}