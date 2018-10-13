using AutoMapper;
using QTrans.Repositories;
using QTrans.WebPortal.Common;
using QTrans.WebPortal.Models;
using QTrans.WebPortal.Models.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            var user = repository.GetUserDetailById(id > 0 ?id:this.UserId, out message);
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
        public ActionResult Create(UserCompany userCompany)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    UserRepository repository = new UserRepository();
                    //Perform the conversion and fetch the destination view model
                    var userProf = Mapper.Map<QTrans.Models.UserProfile>(userCompany.userProfile);
                    var user = repository.WebRegistration(userProf, out message);
                    if (user != null)
                    {
                        CompanyRepository cmprepository = new CompanyRepository(user.Response.userid);
                        userCompany.company.userid = user.Response.userid;
                        //Perform the conversion and fetch the destination view model
                        var comp = Mapper.Map<QTrans.Models.Company>(userCompany.company);
                        var company=cmprepository.CompanyRegistration(comp, out message);
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
                    else
                    {
                        ViewBag.Message = string.IsNullOrEmpty(message) ? "Operation fail due to some reason." : message;
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

        public ActionResult UpdatePassword()
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult UpdatePassword(ChangePassword userProfile)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    if (userProfile.Newpassword == userProfile.ConfirmPassword)
                    {
                        var message = string.Empty;
                        UserRepository repository = new UserRepository();
                        var user = this.sessionStorage.GetValue("UserSession") as UserSession;
                        var result = repository.ChangePassword(user.MobileNo, user.EmailAddress, userProfile.OldPassword, userProfile.Newpassword, out message);
                        if (result.Response)
                        {
                            ViewBag.Message  = message;
                        }
                        else
                        {
                            ViewBag.Message  = string.IsNullOrEmpty(message) ? "Password Update is fail.":message;
                        }
                    }
                    else
                    {
                        ViewBag.Message  = "Password not match";
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

        public ActionResult Logout()
        {
            this.sessionStorage.RemoveValue("UserSession");
            return RedirectToAction("../Home/Index");
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            string fname = string.Empty;
            string filePath = string.Empty;
            int userID = 0;
            string message = string.Empty;
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    if (Request.Form["userid"] != null)
                    {
                        userID = Convert.ToInt32(Request.Form["userid"]);
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
                            filePath = ConfigurationManager.AppSettings["FileUploadPath"] + "/" + userID + "/" + file.FileName;
                            // Checking for Internet Explorer  
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }

                            // Get the complete folder path and store the file inside it.
                            string imageFolderPath = Server.MapPath("~/Uploads/") + userID.ToString() + "/";
                            if (!Directory.Exists(imageFolderPath))
                                Directory.CreateDirectory(imageFolderPath);

                            //delete all existing files for current user
                            DirectoryInfo di = new DirectoryInfo(imageFolderPath);

                            foreach (FileInfo fileToDelete in di.GetFiles())
                            {
                                fileToDelete.Delete();
                            }

                            fname = Path.Combine(imageFolderPath, fname);
                            file.SaveAs(fname);
                        }

                        UserRepository res = new UserRepository();
                        int rowsAffected = res.UpdateUserPhoto(userID, filePath, out message).Response;
                    }
                    // Returns message that successfully uploaded  
                    return Json("" + filePath);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                filePath = ConfigurationManager.AppSettings["DefaultPhotoPath"].ToString();
                return Json("" + filePath);
            }
        }
    }
}
