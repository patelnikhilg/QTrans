using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace QTrans.Utility
{
    public static class UsersSession
    {
        public static ConcurrentDictionary<string, long> userTokenDic = new ConcurrentDictionary<string, long>();
    }
}
