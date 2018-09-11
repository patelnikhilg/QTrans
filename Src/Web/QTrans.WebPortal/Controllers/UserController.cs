using QTrans.WebPortal.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace QTrans.WebPortal.Controllers
{
    public class UserController : BaseController
    {
        //// GET: User
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [AllowAnonymous]
        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
               
                return RedirectToAction("Details/");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Details/" + id.ToString());
            }
            catch
            {
                return View();
            }
        }        
    }
}
