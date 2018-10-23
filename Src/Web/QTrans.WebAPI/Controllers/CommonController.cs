using QTrans.Logging;
using QTrans.Models;
using QTrans.Models.ViewModel.Common;
using QTrans.Repositories.Repositories;
using System.Web.Http;

namespace QTrans.WebAPI.Controllers
{
    [RoutePrefix("api/common")]
    public class CommonController : ApiController
    {
        private AppLogger log = new AppLogger("QTransAPILogger");
        private AppLogger Devicelog = new AppLogger("QTransDeviceAPILogger");

        [Route("GetMaterialType")]
        [HttpGet]
        public IHttpActionResult GetMaterialType()
        {
            using (var repository = new CommonRepository())
            {
                var result = repository.GetMaterialType();
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetPackageType")]
        [HttpGet]
        public IHttpActionResult GetPackageType()
        {
            using (var repository = new CommonRepository())
            {
                var result = repository.GetPackageType();
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetVehicleType")]
        [HttpGet]
        public IHttpActionResult GetVehicleType()
        {
            using (var repository = new CommonRepository())
            {
                var result = repository.GetVehicleType();
                return Ok(new { result.Status, data = result });
            }
        }


        [Route("SubmitLog")]
        [HttpPost]
        public IHttpActionResult SubmitLog([FromBody] LoggingMessage logging)
        {
            switch(logging.LogType)
            {
                case 1:
                    Devicelog.Info(null, logging.PrepareLogMessage());
                    break;
                case 2:
                    Devicelog.Warn(null, logging.PrepareLogMessage());
                    break;
                case 3:
                    Devicelog.Debug(null, logging.PrepareLogMessage());
                    break;
                case 4:
                    Devicelog.Error(logging.ExceptionObj, logging.PrepareLogMessage());
                    break;
                default:
                    Devicelog.Info(null, logging.PrepareLogMessage());
                    break;
            }
           
            return Ok(new { Status = Constants.WebApiStatusOk, data = "Success" });
        }

       
        #region=============== Area Preference ============
        [Route("AddAreaPeference")]
        [HttpPost]
        public IHttpActionResult CreateAreaPeference(AreaPreferenceParam area)
        {
            using (var repository = new CommonRepository())
            {
                var result = repository.InsertAreaPeference(area.UserId, area.CityId);
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("DeleteAreaPeference")]
        [HttpPost]
        public IHttpActionResult DeleteAreaPeference(AreaPreferenceParam area)
        {
            using (var repository = new CommonRepository())
            {
                var result = repository.DeleteAreaPeference(area.UserId, area.CityId);
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetAreaPeference")]
        [HttpGet]
        public IHttpActionResult GetAreaPeference(long userId)
        {
            using (var repository = new CommonRepository())
            {
                var result = repository.GetAreaPeferenceByUserId(userId);
                return Ok(new { result.Status, data = result });
            }
        }
        #endregion


        #region ============== Location Details=================
        [Route("GetState")]
        [HttpGet]
        public IHttpActionResult GetState()
        {
            using (var repository = new CommonRepository())
            {
                var result = repository.GetState();
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetCity")]
        [HttpGet]
        public IHttpActionResult GetCity()
        {
            using (var repository = new CommonRepository())
            {
                var result = repository.GetCity();
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetPincode")]
        [HttpGet]
        public IHttpActionResult GetPincode()
        {
            using (var repository = new CommonRepository())
            {
                var result = repository.GetPincode();
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetCityByStateId")]
        [HttpGet]
        public IHttpActionResult GetCityByStateId(int stateId)
        {
            using (var repository = new CommonRepository())
            {
                var result = repository.GetCityByStateId(stateId);
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetPincodeByCityId")]
        [HttpGet]
        public IHttpActionResult GetPincodeByCityId(int cityId)
        {
            using (var repository = new CommonRepository())
            {
                var result = repository.GetPincodeByCityId(cityId);
                return Ok(new { result.Status, data = result });
            }
        }
        #endregion
    }
}
