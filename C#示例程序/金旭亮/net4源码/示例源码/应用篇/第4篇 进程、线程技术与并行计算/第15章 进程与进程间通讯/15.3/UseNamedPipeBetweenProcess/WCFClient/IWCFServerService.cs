using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFClient
{
    // 注意: 如果更改此处的接口名称 "IWCFServerService"，也必须更新 App.config 中对 "IWCFServerService" 的引用。
    [ServiceContract]
    public interface IWCFServerService
    {
        [OperationContract]
        void SaySomething(string ClientName,string Message);
    }
}
