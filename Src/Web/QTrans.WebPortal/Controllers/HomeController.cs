using QTrans.Models;
using QTrans.Repositories.Repositories;
using System.Web.Mvc;

namespace QTrans.WebPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
          
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CommonRepository repository = new CommonRepository();
                    //Perform the conversion and fetch the destination view model
                    if (repository.InsertContactDetails(contact))
                    {
                        ViewBag.Message  = "Successfully Submitted";
                    }
                    else
                    {
                        ViewBag.Message  = "Fail to Submit";
                    }
                }
            }
            catch
            {
                ViewBag.Message  = "Fail to Submit";
            }

            return View();
        }
    }
}