using adminlte.Common;
using QTrans.Models;
using QTrans.Repositories.Repositories;
using System;
using System.Collections.Generic;
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

            if (ModelState.IsValid)
            {
                string msg;
                VehicleRepository vehicleRepository = new VehicleRepository(this.UserId);
                var v = vehicleRepository.VehicleRegistration(vehicle, out msg).Response;
                if (v.vehicleid > 0)
                {
                    return Json(v, "json");
                }
            }
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(new { success = false });

        }

       public ActionResult Delete(long Id)
        {
            var msg = "Error processing request, try after some time.";
            if (ModelState.IsValid)
            {
                VehicleRepository vehicleRepository = new VehicleRepository(this.UserId);
                var v = vehicleRepository.DeleteVahicalById(Id, out msg).Response;
                if(v)
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