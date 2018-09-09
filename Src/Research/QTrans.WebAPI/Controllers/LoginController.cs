using QTrans.Models;
using QTrans.Repositories;
using System.Web;
using System.Web.Http;
//using Google.Authenticator;

namespace QTrans.WebAPI.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {

        [Route("WebRegistration")]
        [HttpPost]
        public IHttpActionResult WebRegistration(UserProfile model)
        {            
            string message = string.Empty;
            UserRepository userRepository = new UserRepository();
            var result = userRepository.WebRegistration(model, out message);
            return Ok(new { Status = message, data = result });
        }

        [Route("DeviceRegistration")]
        [HttpPost]
        public IHttpActionResult DeviceRegistration(string mobileno)
        {
            string message = string.Empty;
            UserRepository userRepository = new UserRepository();
            var result = userRepository.DeviceRegistration(mobileno, out message);

            return Ok(new { Status = message, data = result });
        }

        [Route("OTPVerification")]
        [HttpPost]
        public IHttpActionResult DeviceVerification(long userId,int OTP)
        {
            string Platform = HttpContext.Current.Request.Headers["platform"].ToString();
            string message = string.Empty;
            bool result = false;
            string token = string.Empty;
            if (Platform.ToLower() == "web")
            {
                UserRepository userRepository = new UserRepository();
                result = userRepository.VerificationMobileEmail(userId, OTP,false,out token, out message);

            }
            else if (Platform.ToLower() == "mobile")
            {                
                UserRepository userRepository = new UserRepository();
                result = userRepository.VerificationMobileEmail(userId, OTP,true, out token, out message);
            }      
            
            return Ok(new { Status = message , data=token});
        }

        [Route("TransportTypeRegistration")]
        [HttpPost]
        public IHttpActionResult TransportTypeRegistration(long userId, long companyId, int TransportType)
        {
            string message = string.Empty;
            UserRepository userRepository = new UserRepository();
            var result = userRepository.UserTypeRegistration(userId,  companyId, TransportType, out message);

            return Ok(new { Status = message, data = result });
        }
    }
}
