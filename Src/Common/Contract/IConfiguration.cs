using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Contract
{
    public interface IConfiguration
    {
        int ScheduleTimeIterval { get; }

        int BatchSize { get; }

        bool UpdateConfig();
    }
}
