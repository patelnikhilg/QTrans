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

        public long companyId { get; set; } = 0;
        public int TransportType { get; set; }
    }

    public class UserOTPVerification
    {
        public string mobileno { get; set; }
        public string emailaddres { get; set; }
        public int OTP { get; set; }
    }
}