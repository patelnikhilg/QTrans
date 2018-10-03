using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Contract
{
    public interface IManager : IDisposable
    {
        bool Init();

        bool Start();

        bool Stop();
    }
}
