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
                message = Constants.WebApiStatusFail;
            }
            else
            {
                message = Constants.WebApiStatusOk;
            }

            return Ok(new { Status = message, data = result });
        }     


        [Route("GetBiddingDetailById")]
        [HttpGet]
        public IHttpActionResult GetBiddingById([FromUri] BiddingDetailsParam param)
        {
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.GetBiddingDetailById(param.biddingId);
            return Ok(new { Status = Constants.WebApiStatusOk, data = result });
        }

        [Route("GetPostingDetailsByDtlPostId")]
        [HttpGet]
        public IHttpActionResult GetPostingDetailsByDtlPostId([FromUri] BiddingParam param)
        {
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.GetPostingDetailByDtlPostingId(param.DtlPostingId); 
            return Ok(new { Status = Constants.WebApiStatusOk, data = result });
        }

        [Route("GetPostingListByUserId")]
        [HttpGet]
        public IHttpActionResult GetPostingListByUserId([FromUri] BiddingUserParam param)
        {
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.GetPostingList(param.UserId, param.IsPast);

            return Ok(new { Status = Constants.WebApiStatusOk, data = result });
        }

        [Route("GetBiddingListByUserId")]
        [HttpGet]
        public IHttpActionResult GetBiddingListByUserId(long UserId)
        {
            BiddingRepository repository = new BiddingRepository(UserId);
            var result = repository.GetBiddingDetailListByUserId(UserId);          

            return Ok(new { Status = Constants.WebApiStatusOk, data = result });
        }

        [Route("GetBiddingListByDtlPostinId")]
        [HttpGet]
        public IHttpActionResult GetBiddingListByDtlPostinId([FromUri] BiddingParam param)
        {
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.GetBiddingListByDtPostingId(param.DtlPostingId);

            return Ok(new { Status = Constants.WebApiStatusOk, data = result });
        }

        [Route("GetBidMinMaxAmountById")]
        [HttpGet]
        public IHttpActionResult GetBidMinMaxAmountById([FromUri] BiddingParam param)
        {
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.GetBidMinMaxByDtlPostId(param.DtlPostingId);

            return Ok(new { Status = Constants.WebApiStatusOk, data = result });
        }

        [Route("SubmitRatingByDtlPostId")]
        [HttpPost]
        public IHttpActionResult SubmitRatingByDtlPostId([FromUri] BidRateParam param)
        {
            string message = string.Empty;
            BiddingRepository repository = new BiddingRepository(param.UserId);
            var result = repository.SubmitRatingByDtlPostId(param.DtlPostingId, param.UserId, param.Rating, param.RatingComment);
            if (result)
            {
                log.Info("Bidding Rating operation is fail");
                message = Constants.WebApiStatusFail;
            }
            else
            {
                message = Constants.WebApiStatusOk;
            }
            return Ok(new { Status = Constants.WebApiStatusOk, data = result });
        }


    }
}
