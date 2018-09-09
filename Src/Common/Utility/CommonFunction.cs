using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Utility
{
    public class CommonFunction
    {

        public static int GenerateOTP()
        {
            return new Random().Next(10000, 999999);
        }
    }
}
