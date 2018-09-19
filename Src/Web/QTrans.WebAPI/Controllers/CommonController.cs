using QTrans.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QTrans.WebAPI.Controllers
{
    public class CommonController : ApiController
    {
        [Route("GetMaterialType")]
        [HttpGet]
        public IHttpActionResult GetMaterialType()
        {
            CommonRepository repository = new CommonRepository();
            var result = repository.GetMaterialType();
            return Ok(new { Status = "OK", data = result });
        }

        [Route("GetPackageType")]
        [HttpGet]
        public IHttpActionResult GetPackageType()
        {
            CommonRepository repository = new CommonRepository();
            var result = repository.GetPackageType();
            return Ok(new { Status = "OK", data = result });
        }

        [Route("GetVehicleType")]
        [HttpGet]
        public IHttpActionResult GetVehicleType()
        {
            CommonRepository repository = new CommonRepository();
            var result = repository.GetVehicleType();
            return Ok(new { Status = "OK", data = result });
        }
    }
}
