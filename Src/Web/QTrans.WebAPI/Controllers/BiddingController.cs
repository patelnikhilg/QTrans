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
            using (var repository = new BiddingRepository(posting.userid))
            {
                var result = repository.BiddingSubmition(posting, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }     


        [Route("GetBiddingDetailById")]
        [HttpGet]
        public IHttpActionResult GetBiddingById([FromUri] BiddingDetailsParam param)
        {
            using (var repository = new BiddingRepository(param.UserId))
            {
                var result = repository.GetBiddingDetailById(param.biddingId);
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetPostingDetailsByDtlPostId")]
        [HttpGet]
        public IHttpActionResult GetPostingDetailsByDtlPostId([FromUri] BiddingParam param)
        {
            using (var repository = new BiddingRepository(param.UserId))
            {
                var result = repository.GetPostingDetailByDtlPostingId(param.DtlPostingId);
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetPostingListByUserId")]
        [HttpGet]
        public IHttpActionResult GetPostingListByUserId([FromUri] BiddingUserParam param)
        {
            using (var repository = new BiddingRepository(param.UserId))
            {
                var result = repository.GetPostingList(param.UserId, param.IsPast);

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetBiddingListByUserId")]
        [HttpGet]
        public IHttpActionResult GetBiddingListByUserId(long UserId)
        {
            using (var repository = new BiddingRepository(UserId))
            {
                var result = repository.GetBiddingDetailListByUserId(UserId);

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetBiddingListByDtlPostinId")]
        [HttpGet]
        public IHttpActionResult GetBiddingListByDtlPostinId([FromUri] BiddingParam param)
        {
            using (var repository = new BiddingRepository(param.UserId))
            {
                var result = repository.GetBiddingListByDtPostingId(param.DtlPostingId);

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetBiddingOrderByUserId")]
        [HttpGet]
        public IHttpActionResult GetBiddingOrderByUserId([FromUri] BiddingStatusParam param)
        {
            using (var repository = new BiddingRepository(param.UserId))
            {
                var result = repository.GetBiddingOrderByUserId(param.UserId, param.Status);

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetPostingListByUserPef")]
        [HttpGet]
        public IHttpActionResult GetPostingByUserPef([FromUri] long userId)
        {
            using (var repository = new BiddingRepository(userId))
            {
                var result = repository.GetPostByUserPef(userId);

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetBidMinMaxAmountById")]
        [HttpGet]
        public IHttpActionResult GetBidMinMaxAmountById([FromUri] BiddingParam param)
        {
            using (var repository = new BiddingRepository(param.UserId))
            {
                var result = repository.GetBidMinMaxByDtlPostId(param.DtlPostingId);

                return Ok(new { result.Status, data = result });
            }
        }

        #region ================= Bidding Rating ==============================

        [Route("SubmitRatingByDtlPostId")]
        [HttpPost]
        public IHttpActionResult SubmitRatingByDtlPostId([FromUri] RatingParam param)
        {
            string message = string.Empty;
            using (var repository = new BiddingRepository(param.UserId))
            {
                var result = repository.SubmitRatingByDtlPostId(param.DtlPostingId, param.UserId, param.Rating, param.RatingComment, param.IsRate);
                if (!result.Response)
                {
                    log.Info("Bidding Rating operation is fail");
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("PendingBidRatebyUserId")]
        [HttpGet]
        public IHttpActionResult PendingBidRatebyUserId(long Userid)
        {
            using (var repository = new BiddingRepository(Userid))
            {
                var result = repository.PendingBidRatingByUserId(Userid);
                return Ok(new { result.Status, data = result });
            }
        }

        #endregion

        #region =============== Bidding Status ====================
        [Route("BiddingStatusByUserId")]
        [HttpPost]
        public IHttpActionResult BiddingStatusByUserId([FromUri] BiddingConfirmParam param)
        {
            string message = string.Empty;
            using (var repository = new BiddingRepository(param.UserId))
            {
                var result = repository.BiddingStatusByUserId(param.DtlPostingId, param.BidUserId, param.BidStatus);
                if (!result.Response)
                {
                    log.Info("Bidding Status operation is fail");
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("BiddingStatusByBiddingId")]
        [HttpPost]
        public IHttpActionResult BiddingStatusByBiddingId([FromUri] BiddingRCStatusParam param)
        {
            string message = string.Empty;
            using (var repository = new BiddingRepository(param.UserId))
            {
                var result = repository.BiddingStatusByBiddingId(param.BiddingId, param.UserId, param.BidStatus,param.Reason);
                if (!result.Response)
                {
                    log.Info("Bidding Status operation is fail");
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetBiddingStatsByUserId")]
        [HttpGet]
        public IHttpActionResult GetBiddingStatsByUserId([FromUri] BiddingStatusParam param)
        {
            using (var repository = new BiddingRepository(param.UserId))
            {
                var result = repository.GetBiddingStatsByUserId(param.UserId);
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetBiddingStatusByUserId")]
        [HttpGet]
        public IHttpActionResult GetBiddingStatusByUserId([FromUri] BiddingStatusParam param)
        {
            using (var repository = new BiddingRepository(param.UserId))
            {
                var result = repository.GetBidStatusByUserId(param.UserId, param.Status);
                return Ok(new { result.Status, data = result });
            }
        }

        #endregion
    }
}
