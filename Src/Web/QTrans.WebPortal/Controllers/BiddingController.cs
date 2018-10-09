using DataTables.Mvc;
using QTrans.Models;
using QTrans.Models.ViewModel.Bidding;
using QTrans.Repositories;
using QTrans.WebPortal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
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
            ////var message = string.Empty;
            ////BiddingRepository postingRepository = new BiddingRepository(this.UserId);
            ////var post = postingRepository.GetPostingList(this.UserId, false);
            return View();
        }

        public ActionResult GetActivePost([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var message = string.Empty;
            BiddingRepository postingRepository = new BiddingRepository(this.UserId);
            var post = postingRepository.GetPostingList(this.UserId, false);
            IQueryable<PostingListBid> query = post.AsQueryable();
            var totalCount = query.Count();

            #region Filtering
            // Apply filters for searching
            if (requestModel.Search.Value != string.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.From.Contains(value) ||
                                         p.To.Contains(value) ||
                                         p.materialtype.Contains(value) ||
                                         p.packagetype.Contains(value) 
                                   );
            }

            var filteredCount = query.Count();

            #endregion Filtering

            #region Sorting
            // Sorting
            var sortedColumns = requestModel.Columns.GetSortedColumns();
            var orderByString = String.Empty;

            foreach (var column in sortedColumns)
            {
                orderByString += orderByString != String.Empty ? "," : "";
                orderByString += (column.Data) + (column.SortDirection == Column.OrderDirection.Ascendant ? " asc" : " desc");
            }

            query = query.OrderBy(orderByString == string.Empty ? "biddingclosedatetime asc" : orderByString);

            #endregion Sorting

            // Paging
            query = query.Skip(requestModel.Start).Take(requestModel.Length);


            var data = query.ToList();

            return Json(new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var message = string.Empty;
            BiddingRepository postingRepository = new BiddingRepository(this.UserId);
            var post = postingRepository.GetPostingList(this.UserId, false);
            IQueryable<PostingListBid> query = post.AsQueryable();
            var totalCount = query.Count();

            #region Filtering
            // Apply filters for searching
            if (requestModel.Search.Value != string.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.From.Contains(value) ||
                                         p.To.Contains(value) ||
                                         p.materialtype.Contains(value) ||
                                         p.packagetype.Contains(value)
                                   );
            }

            var filteredCount = query.Count();

            #endregion Filtering

            #region Sorting
            // Sorting
            var sortedColumns = requestModel.Columns.GetSortedColumns();
            var orderByString = String.Empty;

            foreach (var column in sortedColumns)
            {
                orderByString += orderByString != String.Empty ? "," : "";
                orderByString += (column.Data) + (column.SortDirection == Column.OrderDirection.Ascendant ? " asc" : " desc");
            }

            query = query.OrderBy(orderByString == string.Empty ? "biddingclosedatetime asc" : orderByString);

            #endregion Sorting

            // Paging
            query = query.Skip(requestModel.Start).Take(requestModel.Length);


            var data = query.ToList();

            return Json(new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount), JsonRequestBehavior.AllowGet);
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

        // POST: posting/Create (FormCollection profile)
        [HttpPost]
        public ActionResult Create(FormCollection controll, BiddingProfile profile)
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
                        ViewBag.Message  = message;
                    }
                    else
                    {
                        ViewBag.MessageFail  = message;
                    }
                }
            }
            catch (Exception exp)
            {
                ////TODO: log the error
                ViewBag.Message  = "Unexpected error occured";
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
                        ViewBag.Message  = "Successfully updated";
                    }
                    else
                    {
                        ViewBag.Message  = "Updation fail."; ;
                    }
                }
            }
            catch (Exception exp)
            {
                ////TODO: log the error
                ViewBag.Message  = "Unexpected error occured";
            }

            return View();
        }
    }
}