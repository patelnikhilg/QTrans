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

        // GET: Logout
        public ActionResult Logout()
        {
            try
            {

                return RedirectToAction("Home/Index");
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
        public ActionResult ForgotPassword(FormCollection collection)
        {
            try
            {
                ////TODO: logic here
                return RedirectToAction("Details/");
            }
            catch
            {
                return View();
            }
        }

    }
}