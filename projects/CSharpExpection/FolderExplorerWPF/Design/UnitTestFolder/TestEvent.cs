using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestFolder
{
    public delegate void MyEventDelegate(int value);
    [TestClass]
    public class TestEvent
    {
        [TestMethod]
        public void TestOriginalEvent()
        {
            EventSource myEventSource = new EventSource();
            EventResponsor myEventResponsor1 = new EventResponsor();
            EventResponsor myEventResponsor2 = new EventResponsor();
            myEventSource.handles += myEventResponsor1.Print;
            myEventSource.handles += myEventResponsor2.Print;
            myEventSource.handles(10);
        }

        [TestMethod]
        public void UseEventKeyWord()
        {
            EventSource2 eventSource = new EventSource2();
            EventResponsor myEventResponsor1 = new EventResponsor();
            EventResponsor myEventResponsor2 = new EventResponsor();
            eventSource.handlers += myEventResponsor1.Print;
            eventSource.handlers += myEventResponsor2.Print;
            eventSource.FireEvent();
        }
    }
    #region "第一个方法用到的类"
    public class EventSource
    {
        public MyEventDelegate handles;
    }
    public class EventResponsor
    {
        public void Print(int i)
        {
            Console.WriteLine("value is {0}",i);
        }
    }
    #endregion

    #region "第二个方法用到的类"
    public class EventSource2
    {
        //定义一个事件
        public event MyEventDelegate handlers;
        public void FireEvent()
        {
            if(handlers != null)
            {
                handlers(20);
            }
        }
    }
    #endregion

}
