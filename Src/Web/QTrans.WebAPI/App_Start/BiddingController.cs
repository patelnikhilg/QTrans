using QTrans.Logging;
using QTrans.Models;
using QTrans.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QTrans.WebAPI.App_Start
{
    [RoutePrefix("api/bidding")]
    public class BiddingController : ApiController
    {
        private AppLogger log = new AppLogger("QTransAPILogger");
        [Route("TripPost")]
        [HttpPost]
        public IHttpActionResult SubmitBiddingByPostingId(long postingId, BiddingProfile bidding, long userId)
        {
            string message = string.Empty;
            BiddingRepository repository = new BiddingRepository(userId);
            var result = repository.PostingCreation(postingId, bidding, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            return Ok(new { Status = message, data = result });
        }


        [Route("GetPost")]
        [HttpGet]
        public IHttpActionResult GetBiddingDetailById(long biddingId, long userId)
        {
            string message = string.Empty;
            BiddingRepository repository = new BiddingRepository(userId);
            var result = repository.GetBiddingDetailById(biddingId, out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            return Ok(new { Status = message, data = result });
        }
    }
}
