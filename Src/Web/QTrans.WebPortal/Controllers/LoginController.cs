using QTrans.Repositories;
using QTrans.WebPortal.Models.Login;
using System.Web.Mvc;

namespace QTrans.WebPortal.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

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
                    if (user != null)
                    {
                        return RedirectToAction("Details/" + user.userid);
                    }
                }
            }
            catch
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
                    if (user != null)
                    {
                        return RedirectToAction("../user/Details/" + user.userid.ToString());
                    }
                }
            }
            catch
            {
                ////TODO: log the error.
            }

            return View();
        }

        // GET: Logout
        public ActionResult Logout()
        {
            try
            {

                return RedirectToAction("../Home/Index");
            }
            catch
            {
                return View();
            }
        }

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
                    if (result)
                    {
                        output = "Send password details on your email address";
                    }
                }
            }
            catch
            {
               
            }

            return View();
        }
    }
}