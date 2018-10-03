using QTrans.Common;
using QTrans.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Alert.Core
{
    public class AlertManager : ApplicationManager,IManager
    {
        protected override void StartProcessor(IConfiguration data)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        bool IManager.Init()
        {
            throw new NotImplementedException();
        }

        bool IManager.Start()
        {
            throw new NotImplementedException();
        }

        bool IManager.Stop()
        {
            throw new NotImplementedException();
        }
    }
}
