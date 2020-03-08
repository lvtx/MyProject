using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace UseSynchronizationContextInWCF
{
    [ServiceContract]
    public interface IMyWCFService
    {
        [OperationContract]
        string DoWork();
    }
}
