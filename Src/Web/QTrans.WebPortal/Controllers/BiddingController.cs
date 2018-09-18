using QTrans.Models;
using QTrans.Repositories;
using QTrans.WebPortal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QTrans.WebPortal.Controllers
{
    public class BiddingController : BaseController
    {       
        // GET: Bidding
        public ActionResult Index(long Id)
        {
            var message = string.Empty;
            BiddingRepository Repository = new BiddingRepository(this.UserId);
            var bidding = Repository.GetBiddingDetailById(Id, out message);
            return View(bidding);
        }

        // GET: posting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: posting/Create
        [HttpPost]
        public ActionResult Create(BiddingProfile profile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    BiddingRepository repository = new BiddingRepository(this.UserId);
                    //Perform the conversion and fetch the destination view model
                    var profileresult = repository.BiddingSubmition(profile, out message);
                    if (profileresult != null)
                    {
                        ViewData["Message"] = message;
                    }
                    else
                    {
                        ViewData["Message"] = message;
                    }
                }
            }
            catch (Exception exp)
            {
                ////TODO: log the error
                ViewData["Message"] = "Unexpected error occured";
            }

            return View();
        }

        // GET: posting/Edit/5
        public ActionResult Edit(int id)
        {
            var message = string.Empty;
            BiddingRepository repository = new BiddingRepository(this.UserId);
            //Perform the conversion and fetch the destination view model
            var profile = repository.GetBiddingDetailById(id, out message);
            return View(profile);
        }

        // POST: posting/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, BiddingProfile profile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    profile.biddingid = id;
                    BiddingRepository repository = new BiddingRepository(this.UserId);
                    //Perform the conversion and fetch the destination view model
                    var profileresult = repository.BiddingSubmition(profile, out message);
                    if (profileresult != null)
                    {
                        ViewData["Message"] = message;
                    }
                    else
                    {
                        ViewData["Message"] = message;
                    }
                }
            }
            catch (Exception exp)
            {
                ////TODO: log the error
                ViewData["Message"] = "Unexpected error occured";
            }

            return View();
        }
    }
}