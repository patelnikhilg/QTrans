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

        [Route("SubmitBiddingProfile")]
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


        [Route("GetBiddingDetailById")]
        [HttpGet]
        public IHttpActionResult GetBiddingById([FromUri] BiddingParam param)
        {
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.GetBiddingDetailById(param.DtlPostingId);
            return Ok(new { Status = "OK", data = result });
        }

        [Route("GetBiddingByDtlPostId")]
        [HttpGet]
        public IHttpActionResult GetBiddingByDtlPostId([FromUri] BiddingParam param)
        {
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.GetPostingDetailByDtlPostingId(param.DtlPostingId); 
            return Ok(new { Status = "OK", data = result });
        }

        [Route("GetPostingListByUserId")]
        [HttpGet]
        public IHttpActionResult GetPostingListByUserId([FromUri] BiddingUserParam param)
        {
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.GetPostingList(param.UserId, param.IsPast);

            return Ok(new { Status = "OK", data = result });
        }

        [Route("GetBiddingListByUserId")]
        [HttpGet]
        public IHttpActionResult GetBiddingListByUserId(long UserId)
        {
            BiddingRepository repository = new BiddingRepository(UserId);
            var result = repository.GetBiddingDetailListByUserId(UserId);          

            return Ok(new { Status = "OK", data = result });
        }
    }
}
