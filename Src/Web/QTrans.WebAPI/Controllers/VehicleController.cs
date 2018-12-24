using QTrans.Logging;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Repositories.Repositories;
using QTrans.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QTrans.WebAPI.Controllers
{
    [RoutePrefix("api/vehicle")]
    public class VehicleController : ApiController
    {
        private AppLogger log = new AppLogger("QTransAPILogger");

        [Route("GetVehicleById")]
        [HttpGet]
        public IHttpActionResult GetVehicleById([FromBody] VehicelParam vehicelParam)
        {
            string message = string.Empty;
            using (var vehicleRepository = new VehicleRepository(vehicelParam.userId))
            {
                ResponseSingleModel<Vehicle> result = null;
                if (vehicelParam.vehicleId > 0)
                {
                    result = vehicleRepository.GetVehicleById(vehicelParam.vehicleId);
                }
                else
                {
                    log.Info("Vehicle id is grater than zero");                    
                    return Ok(new { Constants.WebApiStatusFail, data = "Vehicle id is grater than zero" });
                }

                return Ok(new { result.Status, data = result.Message});
            }
        }

        [Route("VehicleRegistration")]
        [HttpPost]
        public IHttpActionResult VehicleRegistration([FromBody] Vehicle vehicle)
        {
            string message = string.Empty;
            using (var vehicleRepository = new VehicleRepository(vehicle.userid))
            {
                var result = vehicleRepository.VehicleRegistration(vehicle, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("VehicleInsurancede")]
        [HttpPost]
        public IHttpActionResult VehicleInsurancede(long userId, [FromBody] InsuranceDetails insurance)
        {
            string message = string.Empty;
            using (var vehicleRepository = new VehicleRepository(userId))
            {
                var result = vehicleRepository.Insurancedetails(insurance, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetInsuranceById")]
        [HttpGet]
        public IHttpActionResult GetInsuranceById([FromBody] VehicelParam vehicelParam)
        {
            string message = string.Empty;
            using (var vehicleRepository = new VehicleRepository(vehicelParam.userId))
            {
                var result = vehicleRepository.GetInsuranceById(vehicelParam.vehicleId, vehicelParam.insuranceId);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }
        
        [Route("GetVehicleListByUserId")]
        [HttpGet]
        public IHttpActionResult GetVehicleListByUserId(long userId)
        {
            string message = string.Empty;
            using (var vehicleRepository = new VehicleRepository(userId))
            {
                var result = vehicleRepository.GetVehicleListByUserId(userId);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }
    }
}
