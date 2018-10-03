using QTrans.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Alert.Core
{
    public class Processor : IProcessor
    {
        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        bool IProcessor.Init(IConfiguration config)
        {
            throw new NotImplementedException();
        }

        bool IProcessor.StartOperation()
        {
            throw new NotImplementedException();
        }

        bool IProcessor.StopOperation()
        {
            throw new NotImplementedException();
        }

        bool IProcessor.UpdateConfig(IConfiguration config)
        {
            throw new NotImplementedException();
        }
    }
}
