using QTrans.Logging;
using QTrans.Models;
using QTrans.Repositories;
using QTrans.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QTrans.WebAPI.Controllers
{
    [RoutePrefix("api/bidding")]
    public class BiddingController : ApiController
    {
        private AppLogger log = new AppLogger("QTransAPILogger");

        [Route("BiddingProfile")]
        [HttpPost]
        public IHttpActionResult SubmitBidding(BiddingProfile posting)
        {
            string message = string.Empty;
            BiddingRepository repository = new BiddingRepository(posting.userid);
            var result = repository.BiddingSubmition(posting, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            else
            {
                message = "OK";
            }

            return Ok(new { Status = message, data = result });
        }     


        [Route("GetBiddingByPostingId")]
        [HttpGet]
        public IHttpActionResult GetBiddingById([FromUri] PostingParam param)
        {
            string message = string.Empty;
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.GetBiddingDetailById(param.PostingId, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            else
            {
                message = "OK";
            }

            return Ok(new { Status = message, data = result });
        }

        [Route("GetBiddingListById")]
        [HttpGet]
        public IHttpActionResult GetBiddingListById([FromUri] PostingParam param)
        {
            string message = string.Empty;
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.GetBiddingDetailById(param.PostingId, param.UserId, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            else
            {
                message = "OK";
            }

            return Ok(new { Status = message, data = result });
        }

        [Route("GetBiddingListByUserId")]
        [HttpGet]
        public IHttpActionResult GetBiddingListByUserId([FromUri] PostingParam param)
        {
            string message = string.Empty;
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.GetListBiddingDetailByUserId(param.UserId, param.IsPast, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            else
            {
                message = "OK";
            }

            return Ok(new { Status = message, data = result });
        }
    }
}
