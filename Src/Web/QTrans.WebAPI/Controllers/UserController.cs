using QTrans.Logging;
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
            return Ok(new { Status = message, data = result });
        }

        [Route("TransportTypeRegistration")]
        [HttpPost]
        public IHttpActionResult TransportTypeRegistration(TransportTypeRegistration transportTypeRegistration)
        {
            string message = string.Empty;
            UserRepository userRepository = new UserRepository();
            var result = userRepository.UserTypeRegistration(transportTypeRegistration.userId, transportTypeRegistration.companyId, transportTypeRegistration.TransportType, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
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
    }
}
