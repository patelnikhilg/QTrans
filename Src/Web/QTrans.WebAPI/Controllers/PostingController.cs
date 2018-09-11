using QTrans.Logging;
using QTrans.Models;
using QTrans.Repositories;
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
            }
            else
            {
                message = "OK";
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
            }
            else
            {
                message = "OK";
            }

            return Ok(new { Status = message, data = result });
        }


        [Route("GetPostingDetailsById")]
        [HttpGet]
        public IHttpActionResult GetpostingById(long postingId, long userId)
        {
            string message = string.Empty;
            PostingRepository repository = new PostingRepository(userId);
            var result = repository.GetPostingDetailById(postingId, out message);
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

        [Route("GetPostingProfileById")]
        [HttpGet]
        public IHttpActionResult GetPostingProfileById(long postingId, long userId)
        {
            string message = string.Empty;
            PostingRepository repository = new PostingRepository(userId);
            var result = repository.GetPostingProfileById(postingId, out message);
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
        
        [Route("GetPostingListById")]
        [HttpGet]
        public IHttpActionResult GetPostingProfileListById(long userId)
        {
            string message = string.Empty;
            PostingRepository repository = new PostingRepository(userId);
            var result = repository.GetPostingListByUserId(userId, out message);
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
