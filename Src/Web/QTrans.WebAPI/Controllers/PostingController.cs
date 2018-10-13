using QTrans.Logging;
using QTrans.Models;
using QTrans.Repositories;
using QTrans.WebAPI.Models;
using System.Web.Http;

namespace QTrans.WebAPI.Controllers
{
    [RoutePrefix("api/posting")]
    public class PostingController : ApiController
    {
        private AppLogger log = new AppLogger("QTransAPILogger");

        [Route("PostingProfile")]
        [HttpPost]
        public IHttpActionResult SubmitPosting(PostingProfile posting)
        {
            string message = string.Empty;
            PostingRepository repository = new PostingRepository(posting.userid);
            var result = repository.PostingPorfileCreation(posting, out message);
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

        [Route("PostingDetail")]
        [HttpPost]
        public IHttpActionResult SubmitPostingDetails(PostingDetails postingDetails)
        {
            string message = string.Empty;
            PostingRepository repository = new PostingRepository(0);
            var result = repository.PostingDetailCreation(postingDetails, out message);
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


        [Route("GetPostingDetailsById")]
        [HttpGet]
        public IHttpActionResult GetpostingById([FromUri] PostingParam param)
        {
            string message = string.Empty;
            PostingRepository repository = new PostingRepository(param.UserId);
            var result = repository.GetPostingDetailById(param.PostingId, out message);
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

        [Route("GetPostingProfileById")]
        [HttpGet]
        public IHttpActionResult GetPostingProfileById([FromUri] PostingParam param)
        {
            string message = string.Empty;
            PostingRepository repository = new PostingRepository(param.UserId);
            var result = repository.GetPostingProfileById(param.PostingId, out message);
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

        [Route("GetPostingListById")]
        [HttpGet]
        public IHttpActionResult GetPostingProfileListById([FromUri] PostingParam param)
        {
            string message = string.Empty;
            PostingRepository repository = new PostingRepository(param.UserId);
            var result = repository.GetPostingListByUserId(param.UserId, param.IsPast, out message);
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

        [Route("SubmitRatingByDtlPostId")]
        [HttpPost]
        public IHttpActionResult SubmitRatingByDtlPostId([FromUri] RatingParam param)
        {
            string message = string.Empty;
            PostingRepository repository = new PostingRepository(param.UserId);
            var result = repository.SubmitRatingByDtlPostId(param.DtlPostingId, param.UserId, param.Rating, param.RatingComment, param.IsRate);
            if (result)
            {
                log.Info("Bidding Rating operation is fail");
                message = Constants.WebApiStatusFail;
            }
            else
            {
                message = Constants.WebApiStatusOk;
            }
            return Ok(new { Status = message, data = result });
        }

        [Route("PendingPostingRatebyUserId")]
        [HttpGet]
        public IHttpActionResult PendingPostingRatebyUserId(long Userid)
        {
            PostingRepository repository = new PostingRepository(Userid);
            var result = repository.PendingPostRatingByUserId(Userid);          
            return Ok(new { Status = Constants.WebApiStatusOk, data = result });
        }
    }
}
