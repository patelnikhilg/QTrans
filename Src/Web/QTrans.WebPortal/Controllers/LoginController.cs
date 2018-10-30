using QTrans.Repositories;
using QTrans.WebPortal.Common;
using QTrans.WebPortal.Models.Login;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace QTrans.WebPortal.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {

        [AllowAnonymous]
        [HttpGet]
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult UserLogin()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UserLogin(UserLogin login)
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
                        return RedirectToAction("Details/" + user.Response.userid);
                    }
                }
            }
            catch (Exception exp)
            {
                ////TODO: log the error.
            }

            return View();
        }

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
                        var session = new UserSession();
                        session.SetValue(user.Response);
                        this.sessionStorage.SetValue("UserSession", session);
                        FormsAuthentication.SetAuthCookie(session.LoginUserName, false);
                        return RedirectToAction("../user/Details/" + user.Response.userid.ToString());
                    }
                    else
                    {
                        ViewBag.Message = "UserName or password is wrong";
                    }
                }
            }
            catch(Exception exp)
            {
                ////TODO: log the error.
            }

            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        // GET: Logout
        public ActionResult Logout()
        {
            try
            {

                return RedirectToAction("../Home/Index");
            }
            catch (Exception exp)
            {
                return View();
            }
        }
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