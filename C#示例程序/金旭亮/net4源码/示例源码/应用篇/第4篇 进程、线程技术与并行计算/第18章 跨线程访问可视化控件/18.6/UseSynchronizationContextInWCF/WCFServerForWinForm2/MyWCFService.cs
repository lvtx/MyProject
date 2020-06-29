using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;

namespace WCFServerForWinForm2
{

    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Single,
        InstanceContextMode=InstanceContextMode.PerCall,
        UseSynchronizationContext=true)]
    public class MyWCFService:IMyWCFService
    {
      

        //外界传入的用于更新可视化界面的函数
        public static Action<int> UpdateUIFunc= null;

        //调用计数
        public static int CallCount;

        public string DoWork()
        {
          //访问计数加一
          int count=Interlocked.Increment(ref CallCount);
          //更新可视化界面
          if (UpdateUIFunc != null)
              UpdateUIFunc(count);
          //向WCF客户端返回结果
          return string.Format("您是第{0}个调用DoWork()方法的用户", count);
        }
    }
}
