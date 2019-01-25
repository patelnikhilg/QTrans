using adminlte.Common;
using QTrans.Models;
using QTrans.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;

using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace adminlte.Controllers
{
    public class QTTrucksController : BaseController
    {
        // GET: QTTrucks
        public ActionResult Index()
        {
            VehicleRepository vehicleRepository = new VehicleRepository(this.UserId);
            var vl = vehicleRepository.GetVehicleListByUserId(this.UserId);
            return View(vl.Response);
        }

        public ActionResult testing()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long Id)
        {
            VehicleRepository vehicleRepository = new VehicleRepository(this.UserId);
            var vehicle = vehicleRepository.GetVehicleById(Id).Response;
            return View(vehicle);
        }

        [HttpPost]
        public ActionResult Edit(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.modifydate = DateTime.Now;
                vehicle.userid = this.UserId;
                VehicleRepository vehicleRepository = new VehicleRepository(this.UserId);
                var msg = "";
                var v = vehicleRepository.VehicleRegistration(vehicle, out msg).Response;
                UploadRCPhoto(vehicle, v.vehicleid);

                return View(v);
            }
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(new { success = false });

        }

        [HttpGet]
        public ActionResult AddNew()
        {
            Vehicle vehicle = new Vehicle();
            vehicle.createddate = DateTime.Now;
            vehicle.companyid = 0;
            vehicle.descrition = "";
            vehicle.manufacturername = "";
            vehicle.manufactureryear = DateTime.Now;
            vehicle.modifydate = DateTime.Now;
            vehicle.rcbookcopypath = "";
            vehicle.registrationdate = DateTime.Now;
            vehicle.userid = this.UserId;
            vehicle.vehicleid = 0;

            return View(vehicle);
        }

        [HttpPost]
        public ActionResult AddNew(Vehicle vehicle)
        {
            vehicle.createddate = DateTime.Now;
            vehicle.companyid = 0;
            vehicle.descrition = "";
            vehicle.manufacturername = "";
            vehicle.manufactureryear = DateTime.Now;
            vehicle.modifydate = DateTime.Now;
            vehicle.rcbookcopypath = vehicle.rcbookcopypath == "" ? "NA" : vehicle.rcbookcopypath;
            vehicle.registrationdate = DateTime.Now;
            vehicle.userid = this.UserId;
            vehicle.vehicleid = 0;

            string msg = "Error process request, please try after some time.";
            if (ModelState.IsValid)
            {


               
                 VehicleRepository vehicleRepository = new VehicleRepository(this.UserId);
                var v = vehicleRepository.VehicleRegistration(vehicle, out msg).Response;


                if (v.vehicleid > 0)
                {
                    UploadRCPhoto(vehicle,v.vehicleid);

                    return Json(new { success = true, responseText = msg }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, responseText = msg }, JsonRequestBehavior.AllowGet);

        }


        public bool UploadRCPhoto(Vehicle vehicle,long vid)
        {

           string imageUri = string.Empty;
            try
            {

             
                //Fetch Form Data
                long UserID = this.UserId;
                long truckid = vehicle.vehicleid;
                bool IsDefault = false;

                string originalFileName = string.Empty;
                string newFileName = string.Empty;

                #region Fetch File(s)
                //string DocumentPath = HttpContext.Current.Server.MapPath("/") + ConfigurationManager.AppSettings["DocumentPath"].ToString() + UserID;
                string DocumentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["FileUploadPath"].ToString(), "RC", truckid.ToString());
                if (!Directory.Exists(DocumentPath))
                    Directory.CreateDirectory(DocumentPath);

                if (UserID > 0)
                {
                    if (vehicle.ImageFile != null)
                    {
                        var docfiles = new List<string>();

                        originalFileName = vehicle.ImageFile.FileName;
                        imageUri = ConfigurationManager.AppSettings["DefaultImageUri"] + "RC/" + truckid + "/" + vehicle.ImageFile.FileName;

                        string filePath = Path.Combine(DocumentPath, originalFileName);
                        vehicle.ImageFile.SaveAs(filePath);
                        string Message = "";
                        VehicleRepository vehicleRepository = new VehicleRepository(UserID);
                        vehicleRepository.updateRcPhoto(truckid, UserID, imageUri, IsDefault, out Message);

                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }




        public ActionResult Delete(long Id)
        {
            var msg = "Error processing request, try after some time.";
            if (ModelState.IsValid)
            {
                VehicleRepository vehicleRepository = new VehicleRepository(this.UserId);
                var v = vehicleRepository.DeleteVahicalById(Id, out msg).Response;
                if (v)
                {
                    return Json(new { success = true, responseText = msg }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, responseText = msg }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Detail(long Id)
        {
            VehicleRepository vehicleRepository = new VehicleRepository(this.UserId);
            var vehicle = vehicleRepository.GetVehicleById(Id).Response;
            return View(vehicle);
        }



    }
}