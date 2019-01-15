using QTrans.Logging;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Repositories.Repositories;
using QTrans.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QTrans.WebAPI.Controllers
{
    [RoutePrefix("api/vehicle")]
    public class VehicleController : ApiController
    {
        private AppLogger log = new AppLogger("QTransAPILogger");

        [Route("GetVehicleById")]
        [HttpGet]
        public IHttpActionResult GetVehicleById([FromUri] VehicelParam vehicelParam)
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

                return Ok(new { result.Status, data = result });
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
        public IHttpActionResult GetInsuranceById([FromUri] VehicelParam vehicelParam)
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



        [Route("GetVehicleListByMobile")]
        [HttpGet]
        public IHttpActionResult GetVehicleListByMobile(long userId,string mobileNumber)
        {
            string message = string.Empty;
            using (var vehicleRepository = new VehicleRepository(userId))
            {
                var result = vehicleRepository.GetVehicleByMobile(mobileNumber);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }




        #region ==============================Upload Posting Photo===============================
        [Route("UploadRCPhoto")]
        [HttpPost]
        public IHttpActionResult UploadRCPhoto()
        {
            var result = new ResponseSingleModel<bool>();
            string returnStatus = string.Empty;
            string Message = string.Empty;
            string imageUri = string.Empty;

            var httpRequest = HttpContext.Current.Request;

            try
            {
                //Fetch Form Data
                Int64 UserID = Convert.ToInt64(httpRequest.Form["userid"]);
                Int64 truckid = Convert.ToInt64(httpRequest.Form["truckid"]);
                bool IsDefault = false;
                if (httpRequest.Form["isdefault"] != null)
                    IsDefault = Convert.ToBoolean(httpRequest.Form["isdefault"]);

                string originalFileName = string.Empty;
                string newFileName = string.Empty;

                #region Fetch File(s)
                //string DocumentPath = HttpContext.Current.Server.MapPath("/") + ConfigurationManager.AppSettings["DocumentPath"].ToString() + UserID;
                string DocumentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["FileUploadPath"].ToString(), "RC", truckid.ToString());
                if (!Directory.Exists(DocumentPath))
                    Directory.CreateDirectory(DocumentPath);

                if (UserID > 0)
                {
                    if (httpRequest.Files != null && httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        foreach (string fileName in httpRequest.Files)
                        {
                            HttpPostedFile file = httpRequest.Files[fileName];
                            originalFileName = file.FileName;
                            imageUri = ConfigurationManager.AppSettings["DefaultImageUri"] + "RC/" + truckid + "/" + file.FileName;

                            string filePath = Path.Combine(DocumentPath, originalFileName);
                            file.SaveAs(filePath);

                            VehicleRepository vehicleRepository = new VehicleRepository(UserID);
                            vehicleRepository.updateRcPhoto(truckid, UserID, imageUri, IsDefault, out Message);
                        }
                    }
                    else
                    {
                        result.Status = Constants.WebApiStatusFail;
                        result.Response = true;
                        result.Message = "No File To Upload";
                        return Ok(new { result.Status, data = result });
                    }
                }
                else
                {
                    result.Status = Constants.WebApiStatusFail;
                    result.Response = true;
                    result.Message = "User id is required";
                    return Ok(new { result.Status, data = result });
                }

                #endregion
            }
            catch (Exception ex)
            {
                result.Status = Constants.WebApiStatusFail;
                result.Response = true;
                result.Message = "Exception occurred in uploading profile photo.";
                return Ok(new { result.Status, data = result });
            }

            result.Status = Constants.WebApiStatusOk;
            result.Response = true;
            result.Message = "";
            return Ok(new { result.Status, data = result });
        }
        #endregion


        [Route("DeleteVehicleById")]
        [HttpPost]
        public IHttpActionResult DeleteVehicle([FromBody] Vehicle vehicle)
        {
            string message = string.Empty;
            using (var vehicleRepository = new VehicleRepository(vehicle.userid))
            {
                var result = vehicleRepository.DeleteVahicalById(vehicle.vehicleid , out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }


    }
}
