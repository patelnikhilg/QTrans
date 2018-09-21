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

        #region ============== Location Details=================
        [Route("GetState")]
        [HttpGet]
        public IHttpActionResult GetState()
        {
            CommonRepository repository = new CommonRepository();
            var result = repository.GetState();
            return Ok(new { Status = "OK", data = result });
        }

        [Route("GetCity")]
        [HttpGet]
        public IHttpActionResult GetCity()
        {
            CommonRepository repository = new CommonRepository();
            var result = repository.GetCity();
            return Ok(new { Status = "OK", data = result });
        }

        [Route("GetPincode")]
        [HttpGet]
        public IHttpActionResult GetPincode()
        {
            CommonRepository repository = new CommonRepository();
            var result = repository.GetPincode();
            return Ok(new { Status = "OK", data = result });
        }

        [Route("GetCityByStateId")]
        [HttpGet]
        public IHttpActionResult GetCityByStateId(int stateId)
        {
            CommonRepository repository = new CommonRepository();
            var result = repository.GetCityByStateId(stateId);
            return Ok(new { Status = "OK", data = result });
        }

        [Route("GetPincodeByCityId")]
        [HttpGet]
        public IHttpActionResult GetPincodeByCityId(int cityId)
        {
            CommonRepository repository = new CommonRepository();
            var result = repository.GetPincodeByCityId(cityId);
            return Ok(new { Status = "OK", data = result });
        }
        #endregion
    }
}
