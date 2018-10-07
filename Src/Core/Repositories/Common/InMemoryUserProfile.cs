using QTrans.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Repositories.Common
{
    public class InMemoryUserProfile
    {
        public ConcurrentDictionary<int, UserProfile> userProfileStorage;
    }
}
