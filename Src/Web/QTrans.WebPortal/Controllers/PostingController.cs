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
    public class PostingController : BaseController
    {      
        // GET: Posting
        public ActionResult Index(long postingId)
        {
            var message = string.Empty;
            PostingRepository postingRepository = new PostingRepository(this.UserId);
            var post=postingRepository.GetPostingProfileById(postingId, out message);
            return View(post);
        }

        // GET: posting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: posting/Create
        [HttpPost]
        public ActionResult Create(PostingProfile profile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    PostingRepository repository = new PostingRepository(this.UserId);
                    //Perform the conversion and fetch the destination view model
                    var profileresult = repository.PostingPorfileCreation(profile, out message);
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
            PostingRepository repository = new PostingRepository(this.UserId);
            //Perform the conversion and fetch the destination view model
            var profile = repository.GetPostingDetailById(id, out message);
            return View(profile);
        }

        // POST: posting/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PostingProfile profile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    profile.postingid = id;
                    PostingRepository repository = new PostingRepository(this.UserId);
                    //Perform the conversion and fetch the destination view model
                    var profileresult = repository.PostingPorfileCreation(profile, out message);
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
        

        public ActionResult List()
        {
            var message = string.Empty;
            PostingRepository postingRepository = new PostingRepository(this.UserId);
            var post = postingRepository.GetPostingListByUserId(this.UserId, out message);
            return View(post);
        }

        #region ===============Posting details =======================
        // GET: Posting
        public ActionResult IndexDetails(long postingId)
        {
            var message = string.Empty;
            PostingRepository postingRepository = new PostingRepository(this.UserId);
            var post = postingRepository.GetPostingDetailById(postingId, out message);
            return View(post);
        }

        // GET: posting/CreateDetails
        public ActionResult CreateDetails()
        {
            return View();
        }

        // POST: posting/CreateDetails
        [HttpPost]
        public ActionResult CreateDetails(PostingDetails details)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    PostingRepository repository = new PostingRepository(this.UserId);
                    //Perform the conversion and fetch the destination view model
                    var profileresult = repository.PostingDetailCreation(details, out message);
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

        // GET: posting/EditDetails/5
        public ActionResult EditDetails(int id)
        {
            var message = string.Empty;
            PostingRepository repository = new PostingRepository(this.UserId);
            var details = repository.GetPostingDetailById(id, out message);
            return View(details);
        }

        // POST: posting/EditDetails/5
        [HttpPost]
        public ActionResult EditDetails(int id, PostingDetails details)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    details.dtlpostingid = id;
                    PostingRepository repository = new PostingRepository(this.UserId);
                    var profileresult = repository.PostingDetailCreation(details, out message);
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
        #endregion
    }
}