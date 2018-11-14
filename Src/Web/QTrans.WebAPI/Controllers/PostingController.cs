using QTrans.Logging;
using QTrans.Models;
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


        #region ==============================Upload Posting Photo===============================
        [Route("UploadPostingPhoto")]
        [HttpPost]
        public IHttpActionResult UploadPostingPhoto()
        {
            string returnStatus = string.Empty;
            string Message = string.Empty;
            string profileImagePath = string.Empty;

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
                string DocumentPath = ConfigurationManager.AppSettings["FileUploadPath"].ToString() + UserID.ToString() + "\\Postings";
                if (!Directory.Exists(DocumentPath))
                    Directory.CreateDirectory(DocumentPath);

                //delete all existing files for current user
                DirectoryInfo di = new DirectoryInfo(DocumentPath);

                //foreach (FileInfo fileToDelete in di.GetFiles())
                //{
                //    fileToDelete.Delete();
                //}

                if (UserID > 0)
                {
                    if (httpRequest.Files != null && httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        foreach (string fileName in httpRequest.Files)
                        {
                            HttpPostedFile file = httpRequest.Files[fileName];
                            originalFileName = file.FileName;
                            profileImagePath = ConfigurationManager.AppSettings["ProfileImagePath"] + UserID + "/Postings/" + file.FileName;

                            //newFileName = Guid.NewGuid().ToString().ToUpperInvariant() + Path.GetExtension(file.FileName);

                            //string filePath = DocumentPath + newFileName;
                            string filePath = Path.Combine(DocumentPath, originalFileName);

                            file.SaveAs(filePath);
                            PostingRepository res = new PostingRepository(UserID);
                            //int rowsAffected = res.UpdateUserPhoto(UserID, profileImagePath, out Message);
                            //res.UpdateUserPhoto(UserID, profileImagePath, out Message);
                            res.AddPostingPhoto(PostingID, UserID, profileImagePath, IsDefault, out Message);
                        }
                    }
                    else
                    {
                        return Ok(new { Status = "Error", Message = "No File To Upload" });
                    }
                }
                else
                {
                    return Ok(new { Status = "Error", Message = "User id is required" });
                }

                #endregion
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "Error", Message = "Exception occurred in uploading profile photo.  Exception: " + ex.Message });
            }

            return Ok(new { Status = "Success", Message = "Profile photo uploaded successfully" });
        }

        #endregion
    }
}
