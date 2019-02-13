using adminlte.Common;
using QTrans.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace adminlte.Controllers
{
    public class QTSearchLoadController : BaseController
    {
        // GET: QTSearchLoad
        public ActionResult Index()
        {
            BiddingRepository biddingRepository = new BiddingRepository(this.UserId);
            var posts= biddingRepository.GetPostByUserPef(this.UserId);
            return View(posts.Response.ToList());
        }

        public ActionResult list()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetPostDetail(long dtlpostId)
        {
            BiddingRepository biddingRepository = new BiddingRepository(this.UserId);
            var response = biddingRepository.GetPostingDetailByDtlPostingId(dtlpostId, this.UserId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


    }
}