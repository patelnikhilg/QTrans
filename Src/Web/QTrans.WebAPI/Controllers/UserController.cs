using QTrans.Logging;
using QTrans.Models;
using QTrans.Repositories;
using QTrans.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QTrans.WebAPI.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private AppLogger log = new AppLogger("QTransAPILogger");

        [Route("GetDetails")]
        [HttpGet]
        public IHttpActionResult GetUserDetailsById(long userId)
        {
            string message = string.Empty;
            UserRepository userRepository = new UserRepository();
            var result = userRepository.GetUserDetailById(userId, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            else
            {
                message = "OK";
            }
            return Ok(new { Status = message, data = result });
        }

        [Route("TransportTypeRegistration")]
        [HttpPost]
        public IHttpActionResult TransportTypeRegistration([FromBody] TransportTypeRegistration transportTypeRegistration)
        {
            string message = string.Empty;
            UserRepository userRepository = new UserRepository();
            var result = userRepository.UserTypeRegistration(transportTypeRegistration.userId, transportTypeRegistration.companyId, transportTypeRegistration.TransportType, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            else
            {
                message = "OK";
            }
            return Ok(new { Status = message, data = result });
        }

        [Route("GetTransportType")]
        [HttpGet]
        public IHttpActionResult GetTransportType()
        {
            UserRepository userRepository = new UserRepository();
            var result = userRepository.GetTransportType();            
            return Ok(new { Status = "OK", data = result });
        }


        [Route("GetTransportTypeByuserId")]
        [HttpGet]
        public IHttpActionResult GetTransportTypeByUserId(long userId)
        {
            string message = string.Empty;
            UserRepository userRepository = new UserRepository();
            var result = userRepository.GetTransportTypeByUserId(userId, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            else
            {
                message = "OK";
            }
            return Ok(new { Status = message, data = result });
        }

        [Route("ChangePassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword([FromBody] ChangePassword changePassword)
        {
            string message = string.Empty;
            UserRepository userRepository = new UserRepository();
            var result = userRepository.ChangePassword(changePassword.mobilenumber, changePassword.emailaddres, changePassword.password, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            else
            {
                message = "OK";
            }
            return Ok(new { Status = message, data = result });
        }

        [Route("UpdateUserProfile")]
        [HttpPost]
        public IHttpActionResult UpdateUserProfile([FromBody] UserProfile userProfile)
        {
            string message = string.Empty;
            UserRepository userRepository = new UserRepository();
            var result = userRepository.UpdateUserProfile(userProfile, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            else
            {
                message = "OK";
            }

            return Ok(new { Status = message, data = result });
        }
    }
}
