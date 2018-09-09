using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTrans.WebAPI.Models
{
    public class MobileModel
    {
        public string mobileno { get; set; }
    }

    public class OTPVerification
    {
        public long userId { get; set; }
        public int OTP { get; set; }
    }

    public class TransportTypeRegistration
    {
        public long userId { get; set; }

        public long companyId { get; set; }
        public int TransportType { get; set; }
    }
}