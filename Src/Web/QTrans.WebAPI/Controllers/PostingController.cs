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
            using (var repository = new PostingRepository(posting.userid))
            {
                var result = repository.PostingPorfileCreation(posting, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("PostingDetail")]
        [HttpPost]
        public IHttpActionResult SubmitPostingDetails(PostingDetails postingDetails)
        {
            string message = string.Empty;
            using (var repository = new PostingRepository(0))
            {
                var result = repository.PostingDetailCreation(postingDetails, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }


        [Route("GetPostingDetailsById")]
        [HttpGet]
        public IHttpActionResult GetpostingById([FromUri] PostingParam param)
        {
            string message = string.Empty;
            using (var repository = new PostingRepository(param.UserId))
            {
                var result = repository.GetPostingDetailById(param.PostingId, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetPostingProfileById")]
        [HttpGet]
        public IHttpActionResult GetPostingProfileById([FromUri] PostingParam param)
        {
            string message = string.Empty;
            using (var repository = new PostingRepository(param.UserId))
            {
                var result = repository.GetPostingProfileById(param.PostingId, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetPostingListById")]
        [HttpGet]
        public IHttpActionResult GetPostingProfileListById([FromUri] PostingParam param)
        {
            string message = string.Empty;
            using (var repository = new PostingRepository(param.UserId))
            {
                var result = repository.GetPostingListByUserId(param.UserId, param.IsPast, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        #region ============= Rating ===================
        [Route("SubmitRatingByDtlPostId")]
        [HttpPost]
        public IHttpActionResult SubmitRatingByDtlPostId([FromUri] RatingParam param)
        {
            string message = string.Empty;
            using (var repository = new PostingRepository(param.UserId))
            {
                var result = repository.SubmitRatingByDtlPostId(param.DtlPostingId, param.UserId, param.Rating, param.RatingComment, param.IsRate, param.CreatedBy);
                if (!result.Response)
                {
                    log.Info("Bidding Rating operation is fail");
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("PendingPostingRatebyUserId")]
        [HttpGet]
        public IHttpActionResult PendingPostingRatebyUserId(long Userid)
        {
            using (var repository = new PostingRepository(Userid))
            {
                var result = repository.PendingPostRatingByUserId(Userid);
                return Ok(new { result.Status, data = result });
            }
        }

        #endregion

        #region===================== Posting Status====================
        [Route("UpdatePostingStatus")]
        [HttpPost]
        public IHttpActionResult UpdatePostingStatus([FromUri] PostingStatusParam param)
        {
            string message = string.Empty;
            using (var repository = new PostingRepository(param.UserId))
            {
                var result = repository.UpdatePostingStatus(param.DtlPostingId, param.Status);
                if (!result.Response)
                {
                    log.Info("Update Posting status operation is fail");
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetPostingStatsByUserId")]
        [HttpGet]
        public IHttpActionResult GetPostingStatsByUserId([FromUri] PostingStatusParam param)
        {
            using (var repository = new PostingRepository(param.UserId))
            {
                var result = repository.GetPostingStatsByUserId(param.UserId);
                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetPostingStatusByUserId")]
        [HttpGet]
        public IHttpActionResult GetPostingStatusByUserId([FromUri] PostingStatusParam param)
        {
            using (var repository = new PostingRepository(param.UserId))
            {
                var result = repository.GetPostingStatusByUserId(param.UserId, param.Status);
                return Ok(new { result.Status, data = result });
            }
        }

        #endregion
    }
}
