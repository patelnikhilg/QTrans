using AutoMapper;
using QTrans.Repositories;
using QTrans.WebPortal.Common;
using QTrans.WebPortal.Models;
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
            var message = string.Empty;
            UserRepository repository = new UserRepository();
            //Perform the conversion and fetch the destination view model
            var user = repository.GetUserDetailById(id, out message);
            var userProf = Mapper.Map<QTrans.WebPortal.Models.UserProfile>(user);
            return View(userProf);
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
        public ActionResult Create(UserProfile userProfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    UserRepository repository = new UserRepository();
                    //Perform the conversion and fetch the destination view model
                    var userProf = Mapper.Map<QTrans.Models.UserProfile>(userProfile);
                    var user = repository.WebRegistration(userProf, out message);
                    if (user != null)
                    {
                        return RedirectToAction("../login/login");
                    }
                }
            }
            catch(Exception exp)
            {
                ////TODO: log the error.
            }

            return View();
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            var message = string.Empty;
            UserRepository repository = new UserRepository();
            //Perform the conversion and fetch the destination view model
            var user = repository.GetUserDetailById(id, out message);
            var userProf = Mapper.Map<QTrans.WebPortal.Models.UserProfile>(user);
            return View(userProf);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UserProfile userProfile)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    userProfile.userid = id;
                    var message = string.Empty;
                    UserRepository repository = new UserRepository();
                    //Perform the conversion and fetch the destination view model
                    var userProf = Mapper.Map<QTrans.Models.UserProfile>(userProfile);
                    var user = repository.UpdateUserProfile(userProf, out message);
                    if (user != null)
                    {
                        return RedirectToAction("Details/" + id.ToString());
                    }
                }
               /// return RedirectToAction("Details/" + id.ToString());
            }
            catch
            {
                ////TODO: log the error.
            }

            return View();
        }        
    }
}
