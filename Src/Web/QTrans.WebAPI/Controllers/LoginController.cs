using QTrans.Logging;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Repositories;
using QTrans.Utility;
using QTrans.WebAPI.Models;
using System.Web;
using System.Web.Http;
//using Google.Authenticator;

namespace QTrans.WebAPI.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private AppLogger log = new AppLogger("QTransAPILogger");

        [Route("WebRegistration")]
        [HttpPost]
        public IHttpActionResult WebRegistration(UserProfile model)
        {
            string message = string.Empty;
            using (var userRepository = new UserRepository())
            {
                var result = userRepository.WebRegistration(model, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("DeviceRegistration")]
        [HttpPost]
        public IHttpActionResult DeviceRegistration(MobileModel model)
        {
            string message = string.Empty;
            using (var userRepository = new UserRepository())
            {
                var result = userRepository.DeviceRegistration(model.mobileno, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("OTPVerificationByUser")]
        [HttpPost]
        public IHttpActionResult DeviceVerificationByUser([FromBody] OTPVerification OTP)
        {
            string Platform = HttpContext.Current.Request.Headers["platform"].ToString();
            string message = string.Empty;
            var result = false;
            var response = new ResponseSingleModel<string>();
            string token = string.Empty;
            if (Platform.ToLower() == "web")
            {
                using (var userRepository = new UserRepository())
                {
                    result = userRepository.VerificationMobileEmail(OTP.userId, OTP.OTP, false, out token, out message).Response;
                }

            }
            else if (Platform.ToLower() == "mobile")
            {
                using (var userRepository = new UserRepository())
                {
                    result = userRepository.VerificationMobileEmail(OTP.userId, OTP.OTP, true, out token, out message).Response;
                }
            }
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }

            if (!string.IsNullOrEmpty(token))
            {
                UsersSession.userTokenDic.TryAdd(token, OTP.userId);
            }

            response.Response = token;
            response.Status = result ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            response.Message = message;
            return Ok(new { response.Status, data = response });
        }

        [Route("OTPVerification")]
        [HttpPost]
        public IHttpActionResult DeviceVerification([FromBody] UserOTPVerification OTP)
        {
            string Platform = HttpContext.Current.Request.Headers["platform"].ToString();
            string message = string.Empty;
            bool result = false;
            string token = string.Empty;
            var response = new ResponseSingleModel<string>();
            long userid = 0;
            if (Platform.ToLower() == "web")
            {
                using (var userRepository = new UserRepository())
                {
                    result = userRepository.UpdateMobileEmailVerification(OTP.mobileno, OTP.emailaddres, false, OTP.OTP, out userid, out token, out message).Response;
                }

            }
            else if (Platform.ToLower() == "mobile")
            {
                using (var userRepository = new UserRepository())
                {
                    result = userRepository.UpdateMobileEmailVerification(OTP.mobileno, OTP.emailaddres, true, OTP.OTP, out userid, out token, out message).Response;
                }
            }
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
                message = Constants.WebApiStatusFail;
            }
            else
            {
                message = Constants.WebApiStatusOk;
            }
            if (!string.IsNullOrEmpty(token) && userid > 0)
            {
                UsersSession.userTokenDic.TryAdd(token, userid);
            }

            response.Response = token;
            response.Status = result ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            response.Message = message;
            return Ok(new { response.Status, data = response });
        }

        [Route("ForgotUserLoginDetail")]
        [HttpGet]
        public IHttpActionResult ForgotUserLoginDetail(string mobileno, string emailaddress)
        {
            string message = string.Empty;
            using (var userRepository = new UserRepository())
            {
                var result = userRepository.ForgotUserLoginDetail(mobileno, emailaddress, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

    }
}
