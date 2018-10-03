using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Contract
{
    public interface IProcessor : IDisposable
    {
        bool Init(IConfiguration config);

        bool StartOperation();

        bool UpdateConfig(IConfiguration config);

        bool StopOperation();
    }
}
