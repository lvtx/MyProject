using System;
using System.Collections.Generic;
using System.Text;

namespace Zoo2
{
    abstract class Animal
    {
        public abstract void eat();
    }
    //ʨ��
    class Lion : Animal
    {
        public override void eat()
        {
           Console.WriteLine("����ʨ�ӣ��Ҳ�����˭�ҳ��⣡");
        }
    }
    //����
    class Monkey : Animal
    {
        public override void eat()
        {
            Console.WriteLine("���Ǻ��ӣ���ϲ��͵���㽶��");
        }
    }
    //����
    class Pigeon : Animal
    {
        public override void eat()
        {
            Console.WriteLine("����һֻƯ���ĸ��ӣ�Ϊ��ά�����������Σ���ÿ��ֻ�Լ������ף�");
        }
    }

}
