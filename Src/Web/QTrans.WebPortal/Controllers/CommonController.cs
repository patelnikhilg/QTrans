﻿using QTrans.Models;
using QTrans.Repositories.Repositories;
using QTrans.WebPortal.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace QTrans.WebPortal.Controllers
{
    public class CommonController : BaseController
    {
        // GET: Common
        public ActionResult Index()
        {
            CommonRepository repository = new CommonRepository();
            var result = repository.GetAreaPeferenceByUserId(this.UserId);
            return View(result);
        }

        public ActionResult CreateAreaPeference()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateAreaPeference(int cityId)
        {
            try
            {
                if (cityId > 0)
                {
                    CommonRepository repository = new CommonRepository();
                    var result = repository.InsertAreaPeference(this.UserId, cityId);
                }
            }
            catch (Exception exp)
            {
                ////TODO: log the error.
            }

            return Json("Successfully Add", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteAreaPeference(int id)
        {
            try
            {
                if (id > 0)
                {
                    CommonRepository repository = new CommonRepository();
                    var result = repository.DeleteAreaPeference(this.UserId, id);
                }
            }
            catch (Exception exp)
            {
                ////TODO: log the error.
            }

            return RedirectToAction("../common/Index");
        }       

        [HttpGet]
        public ActionResult GetCity(int stateId)
        {
            List<SelectListItem> citynames = new List<SelectListItem>();
            if (stateId > 0)
            {
                List<QTrans.Models.ViewModel.Common.StateCity> city = new CommonRepository().GetCityByStateId(stateId).Where(x => x.StateId == stateId).ToList();
                city.ForEach(x =>
                {
                    citynames.Add(new SelectListItem { Text = x.CityName, Value = x.CityId.ToString() });
                });
            }
            return Json(citynames, JsonRequestBehavior.AllowGet);
        }
    }
}