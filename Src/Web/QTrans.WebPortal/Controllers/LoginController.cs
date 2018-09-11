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
        public ActionResult Login(FormCollection collection)
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