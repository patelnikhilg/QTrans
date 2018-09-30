using QTrans.Models;
using QTrans.Repositories;
using QTrans.Utility;
using QTrans.WebPortal.Common;
using QTrans.WebPortal.Models.Posting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QTrans.WebPortal.Controllers
{
    public class PostingController : BaseController
    {
        // GET: Posting
        public ActionResult Index(long id)
        {
            var message = string.Empty;
            PostingRepository postingRepository = new PostingRepository(this.UserId);
            var post = postingRepository.GetPostingProfileById(id, out message);
            return View(post);
        }

        // GET: posting/Create
        public ActionResult Create()
        {
            TempDataFilling();
            return View();
        }

        private void TempDataFilling()
        {
            List<SelectListItem> postStatus = new List<SelectListItem>() {
                                        new SelectListItem {
                                            Text = PostStatus.None.ToString(), Value = "0"
                                        },
                                        new SelectListItem {
                                            Text = PostStatus.Open.ToString(), Value = "1"
                                        },
                                        new SelectListItem {
                                             Text = PostStatus.Close.ToString(), Value = "2"
                                        },

                                         };
            ViewBag.PostStatus = postStatus;

            List<SelectListItem> orderType = new List<SelectListItem>() {
                                        new SelectListItem {
                                            Text =OrderType.None.ToString(), Value = "0"
                                        },
                                        new SelectListItem {
                                            Text = OrderType.SingleParty.ToString(), Value = "1"
                                        },
                                        new SelectListItem {
                                             Text = OrderType.Distributive.ToString(), Value = "2"
                                        },

                                         };
            ViewBag.OrderType = orderType;
        }

        // POST: posting/Create
        [HttpPost]
        public ActionResult Create(PostingData data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    data.profile.userid = this.UserId;
                    PostingRepository repository = new PostingRepository(this.UserId);
                    //Perform the conversion and fetch the destination view model
                    var profileresult = repository.PostingPorfileCreation(data.profile, out message);
                    if (profileresult != null)
                    {
                        data.details.postingid = profileresult.postingid;
                        var details = repository.PostingDetailCreation(data.details, out message);
                        if (details != null)
                        {
                            ViewData["Message"] = message;
                            return RedirectToAction("CreateDetails");
                        }
                        else
                        {
                            ViewData["Message"] = message;
                        }
                    }
                    else
                    {
                        ViewData["Message"] = message;
                    }
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                    ViewData["Message"] = errors;

                }
            }
            catch (Exception exp)
            {
                ////TODO: log the error
                ViewData["Message"] = "Unexpected error occured";
            }

            TempDataFilling();

            return View(data);
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
        public ActionResult Edit(int id, PostingData data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    data.profile.userid = this.UserId;
                    PostingRepository repository = new PostingRepository(this.UserId);
                    //Perform the conversion and fetch the destination view model
                    var profileresult = repository.PostingPorfileCreation(data.profile, out message);
                    if (profileresult != null)
                    {
                        data.details.postingid = id;
                        var details = repository.PostingDetailCreation(data.details, out message);
                        if (details != null)
                        {
                            ViewData["Message"] = message;
                            return RedirectToAction("CreateDetails");
                        }
                        else
                        {
                            ViewData["Message"] = message;
                        }
                    }
                    else
                    {
                        ViewData["Message"] = message;
                    }
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                    ViewData["Message"] = errors;

                }
            }
            catch (Exception exp)
            {
                ////TODO: log the error
                ViewData["Message"] = "Unexpected error occured";
            }

            return View();
        }
        
        public ActionResult PastList()
        {
            var message = string.Empty;
            PostingRepository postingRepository = new PostingRepository(this.UserId);
            var post = postingRepository.GetPostingListByUserId(this.UserId, true, out message);
            return View(post);
        }

        public ActionResult CurrentList()
        {
            var message = string.Empty;
            PostingRepository postingRepository = new PostingRepository(this.UserId);
            var post = postingRepository.GetPostingListByUserId(this.UserId, false, out message);
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
            ViewBag.PostingId = TempData.Peek("PostingId");
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
                    details.postingid = ViewBag.PostingId;
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