using QTrans.Logging;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Repositories;
using QTrans.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
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

        [Route("GetPostingOrderByUserId")]
        [HttpGet]
        public IHttpActionResult GetPostingOrderByUserId([FromUri] BiddingStatusParam param)
        {
            using (var repository = new PostingRepository(param.UserId))
            {
                var result = repository.GetPostingOrderByUserId(param.UserId, param.Status);

                return Ok(new { result.Status, data = result });
            }
        }

        #region ============= Rating ===================
        [Route("SubmitRatingByDtlPostId")]
        [HttpPost]
        public IHttpActionResult SubmitRatingByDtlPostId( RatingParam param)
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
//        public IHttpActionResult UpdatePostingStatus([FromUri] PostingStatusParam param)
        public IHttpActionResult UpdatePostingStatus(PostingStatusParam param)
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


        #region ==============================Upload Posting Photo===============================
        [Route("UploadPostingPhoto")]
        [HttpPost]
        public IHttpActionResult UploadPostingPhoto()
        {
            var result = new ResponseSingleModel<bool>();
            string returnStatus = string.Empty;
            string Message = string.Empty;
            string imageUri = string.Empty;

            var httpRequest = HttpContext.Current.Request;

            try
            {
                //Fetch Form Data
                Int64 UserID = Convert.ToInt64(httpRequest.Form["userid"]);
                Int64 PostingID = Convert.ToInt64(httpRequest.Form["postingid"]);
                bool IsDefault = false;
                if (httpRequest.Form["isdefault"] != null)
                    IsDefault = Convert.ToBoolean(httpRequest.Form["isdefault"]);

                string originalFileName = string.Empty;
                string newFileName = string.Empty;

                #region Fetch File(s)
                //string DocumentPath = HttpContext.Current.Server.MapPath("/") + ConfigurationManager.AppSettings["DocumentPath"].ToString() + UserID;
                string DocumentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["FileUploadPath"].ToString(), "Postings", PostingID.ToString());
                if (!Directory.Exists(DocumentPath))
                    Directory.CreateDirectory(DocumentPath);

                if (UserID > 0)
                {
                    if (httpRequest.Files != null && httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        foreach (string fileName in httpRequest.Files)
                        {
                            HttpPostedFile file = httpRequest.Files[fileName];
                            originalFileName = file.FileName;
                            imageUri = ConfigurationManager.AppSettings["DefaultImageUri"] + "Postings/" + PostingID + "/" + file.FileName;

                            string filePath = Path.Combine(DocumentPath, originalFileName);
                            file.SaveAs(filePath);

                            PostingRepository res = new PostingRepository(UserID);
                            res.AddPostingPhoto(PostingID, UserID, imageUri, IsDefault, out Message);
                        }
                    }
                    else
                    {
                        result.Status = Constants.WebApiStatusFail;
                        result.Response = true;
                        result.Message = "No File To Upload";
                        return Ok(new { result.Status, data = result });
                    }
                }
                else
                {
                    result.Status = Constants.WebApiStatusFail;
                    result.Response = true;
                    result.Message = "User id is required";
                    return Ok(new { result.Status, data = result });
                }

                #endregion
            }
            catch (Exception ex)
            {
                result.Status = Constants.WebApiStatusFail;
                result.Response = true;
                result.Message = "Exception occurred in uploading profile photo.";
                return Ok(new { result.Status, data = result });
            }

            result.Status = Constants.WebApiStatusOk;
            result.Response = true;
            result.Message = "";
            return Ok(new { result.Status, data = result });
        }

        #endregion

        [Route("GetPostingSummary")]
        [HttpGet]
        public IHttpActionResult GetPostingSummary([FromUri] long userId)
        {
            using (var repository = new PostingRepository(userId))
            {

                var result = repository.GetPostingSummary(userId);
                return Ok(new { result.Status, data = result });
            }
        }
    }
}
