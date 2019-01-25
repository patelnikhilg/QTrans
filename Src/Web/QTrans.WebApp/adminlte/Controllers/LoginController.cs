using adminlte.Common;
using adminlte.Models.Login;
using QTrans.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace adminlte.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        // GET: Login view
        public ActionResult Login()
        {
            UserLogin userLogin = new UserLogin();
            if (Request.Cookies["UserName"] != null )
            {
                userLogin.UserName= Request.Cookies["UserName"].Value;
                userLogin.remember = true;
            }
            else
            {
                userLogin.remember = false;
            }
         
            return View(userLogin);
        }

        //Post login detail (Login click)
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserLogin login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    UserRepository repository = new UserRepository();
                    var user = repository.Login(login.UserName, login.Password, out message);
                    if (user.Response != null)
                    {
                        UserLogin userLogin = new UserLogin();
                        if (login.remember )
                            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                        else
                            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                       
                        Response.Cookies["UserName"].Value = login.UserName;
                        var session = new UserSession();
                        session.SetValue(user.Response);
                        this.sessionStorage.SetValue("UserSession", session);
                        FormsAuthentication.SetAuthCookie(session.LoginUserName, false);
                        return RedirectToAction("../QTDashboard/Index");
                    }
                    else
                    {
                        ViewBag.Message = "Mobile Number or password is wrong";
                    }
                }
            }
            catch (Exception exp)
            {
                ////TODO: log the error.
            }

            return View();
        }

        //Get : Forgot Password view
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(Forgotpassword forgotpassword)
        {
            string output = "User account not found";
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    UserRepository repository = new UserRepository();
                    var result = repository.ForgotUserLoginDetail(forgotpassword.MobileNo, forgotpassword.EmailAddress, out message);
                    if (result.Response)
                    {
                        output = "Send password details on your email address";
                        ViewBag.Message = "Send password details on your email address";
                    }
                    else
                    {
                        ViewBag.Message = string.IsNullOrEmpty(message) ? "Operation fail due to some reason." : message;
                    }
                }
            }
            catch (Exception exp)
            {

            }

            return View();
        }


    }


}