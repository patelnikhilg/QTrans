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
        public ActionResult CurrentPost()
        {
            BiddingRepository postingRepository = new BiddingRepository(this.UserId);
            var post = postingRepository.GetPostingList(this.UserId, false);
            return View(post);
        }

        // GET: Bidding
        public ActionResult ClosePost()
        {
            var message = string.Empty;
            BiddingRepository postingRepository = new BiddingRepository(this.UserId);
            var post = postingRepository.GetPostingList(this.UserId, true);
            return View(post);
        }

        // GET: Bidding
        public ActionResult ActivePost()
        {
            var message = string.Empty;
            BiddingRepository postingRepository = new BiddingRepository(this.UserId);
            var post = postingRepository.GetPostingList(this.UserId, false);
            return View(post);
        }

        // GET: Bidding
        public ActionResult BiddingList()
        {
            var message = string.Empty;
            BiddingRepository postingRepository = new BiddingRepository(this.UserId);
            var post = postingRepository.GetBiddingDetailListByUserId(this.UserId);
            return View(post);
        }

        // GET: Bidding
        public ActionResult PostDetails(long Id)
        {
            var message = string.Empty;
            BiddingRepository postingRepository = new BiddingRepository(this.UserId);
            var post = postingRepository.GetPostingDetailByDtlPostingId(Id);
            return View(post);
        }

        // GET: posting/Create
        public ActionResult Create(long DtlPostingId)
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
            BiddingRepository repository = new BiddingRepository(this.UserId);
            //Perform the conversion and fetch the destination view model
            var profile = repository.GetBiddingDetailById(id);
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
                    string message = string.Empty;
                    profile.biddingid = id;
                    BiddingRepository repository = new BiddingRepository(this.UserId);
                    //Perform the conversion and fetch the destination view model
                    var profileresult = repository.BiddingSubmition(profile, out message);
                    if (profileresult != null)
                    {
                        ViewData["Message"] = "Successfully updated";
                    }
                    else
                    {
                        ViewData["Message"] = "Updation fail."; ;
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