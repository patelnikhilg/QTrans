using QTrans.Logging;
using QTrans.Models;
using QTrans.Repositories;
using QTrans.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QTrans.WebAPI.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private AppLogger log = new AppLogger("QTransAPILogger");

        [Route("GetDetails")]
        [HttpGet]
        public IHttpActionResult GetUserDetailsById(long userId)
        {
            string message = string.Empty;
            using (var userRepository = new UserRepository())
            {
                var result = userRepository.GetUserDetailById(userId, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("TransportTypeRegistration")]
        [HttpPost]
        public IHttpActionResult TransportTypeRegistration([FromBody] TransportTypeRegistration transportTypeRegistration)
        {
            string message = string.Empty;
            using (var userRepository = new UserRepository())
            {
                var result = userRepository.UserTypeRegistration(transportTypeRegistration.userId, transportTypeRegistration.companyId, transportTypeRegistration.TransportType, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("GetTransportType")]
        [HttpGet]
        public IHttpActionResult GetTransportType()
        {
            using (var userRepository = new UserRepository())
            {
                var result = userRepository.GetTransportType();
                return Ok(new { result.Status, data = result });
            }
        }


        [Route("GetTransportTypeByuserId")]
        [HttpGet]
        public IHttpActionResult GetTransportTypeByUserId(long userId)
        {
            string message = string.Empty;
            using (var userRepository = new UserRepository())
            {
                var result = userRepository.GetTransportTypeByUserId(userId, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("ChangePassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword([FromBody] ChangePassword changePassword)
        {
            string message = string.Empty;
            using (var userRepository = new UserRepository())
            {
                var result = userRepository.ChangePassword(changePassword.mobilenumber, changePassword.emailaddres, changePassword.oldpassword, changePassword.password, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("UpdateUserProfile")]
        [HttpPost]
        public IHttpActionResult UpdateUserProfile([FromBody] UserProfile userProfile)
        {
            string message = string.Empty;
            using (var userRepository = new UserRepository())
            {
                var result = userRepository.UpdateUserProfile(userProfile, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    log.Info(message);
                }

                return Ok(new { result.Status, data = result });
            }
        }

        [Route("UploadProfilePhoto")]
        [HttpPost]
        public IHttpActionResult UploadProfilePhoto()
        {
            string returnStatus = string.Empty;
            string Message = string.Empty;
            string profileImagePath = string.Empty;

            var httpRequest = HttpContext.Current.Request;

            try
            {
                //Fetch Form Data
                Int64 UserID = Convert.ToInt64(httpRequest.Form["userid"]);

                string originalFileName = string.Empty;
                string newFileName = string.Empty;

                #region Fetch File(s)
                //string DocumentPath = HttpContext.Current.Server.MapPath("/") + ConfigurationManager.AppSettings["DocumentPath"].ToString() + UserID;
                string DocumentPath = ConfigurationManager.AppSettings["FileUploadPath"].ToString() + UserID.ToString();
                if (!Directory.Exists(DocumentPath))
                    Directory.CreateDirectory(DocumentPath);

                //delete all existing files for current user
                DirectoryInfo di = new DirectoryInfo(DocumentPath);

                foreach (FileInfo fileToDelete in di.GetFiles())
                {
                    fileToDelete.Delete();
                }

                if (UserID > 0)
                {
                    if (httpRequest.Files != null && httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        foreach (string fileName in httpRequest.Files)
                        {
                            HttpPostedFile file = httpRequest.Files[fileName];
                            originalFileName = file.FileName;
                            profileImagePath = ConfigurationManager.AppSettings["ProfileImagePath"] + UserID + "/" + file.FileName;

                            //newFileName = Guid.NewGuid().ToString().ToUpperInvariant() + Path.GetExtension(file.FileName);

                            //string filePath = DocumentPath + newFileName;
                            string filePath = Path.Combine(DocumentPath, originalFileName);

                            file.SaveAs(filePath);
                            UserRepository res = new UserRepository();
                            //int rowsAffected = res.UpdateUserPhoto(UserID, profileImagePath, out Message);
                            res.UpdateUserPhoto(UserID, profileImagePath, out Message);
                            //if (returnStatus.ToLower() == "success")
                            //{
                            //    UserRepository res = new UserRepository();
                            //    int rowsAffected = res.UpdateUserPhoto(UserID, profileImagePath, out Message);
                            //}
                            //else
                            //{
                            //}
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


        [Route("UploadIdentityDocument")]
        [HttpPost]
        public IHttpActionResult UploadIdentityDocument()
        {
            string returnStatus = string.Empty;
            string Message = string.Empty;
            string profileImagePath = string.Empty;

            var httpRequest = HttpContext.Current.Request;

            try
            {
                //Fetch Form Data
                Int64 UserID = Convert.ToInt64(httpRequest.Form["userid"]);
                string DocumentType = httpRequest.Form["documenttype"].ToString();
                string originalFileName = string.Empty;
                string newFileName = string.Empty;

                #region Fetch File(s)
                //string DocumentPath = HttpContext.Current.Server.MapPath("/") + ConfigurationManager.AppSettings["DocumentPath"].ToString() + UserID;
                string DocumentPath = ConfigurationManager.AppSettings["FileUploadPath"].ToString() + UserID.ToString() + "\\" + DocumentType;
                if (!Directory.Exists(DocumentPath))
                    Directory.CreateDirectory(DocumentPath);

                //delete all existing files for current user
                DirectoryInfo di = new DirectoryInfo(DocumentPath);

                foreach (FileInfo fileToDelete in di.GetFiles())
                {
                    fileToDelete.Delete();
                }

                if (UserID > 0)
                {
                    if (httpRequest.Files != null && httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        foreach (string fileName in httpRequest.Files)
                        {
                            HttpPostedFile file = httpRequest.Files[fileName];
                            originalFileName = file.FileName;
                            profileImagePath = ConfigurationManager.AppSettings["ProfileImagePath"] + UserID + "/" + DocumentType + "/" + file.FileName;

                            //newFileName = Guid.NewGuid().ToString().ToUpperInvariant() + Path.GetExtension(file.FileName);

                            //string filePath = DocumentPath + newFileName;
                            string filePath = Path.Combine(DocumentPath, originalFileName);

                            file.SaveAs(filePath);
                            UserRepository res = new UserRepository();
                            //res.UpdateUserIDDocuments(UserID, DocumentType, profileImagePath, out Message);
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

    }
}
