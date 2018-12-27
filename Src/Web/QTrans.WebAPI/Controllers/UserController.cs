using QTrans.Logging;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Repositories;
using QTrans.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
            var result = new ResponseSingleModel<bool>();
            string returnStatus = string.Empty;
            string Message = string.Empty;
            string imageUri = string.Empty;

            var httpRequest = HttpContext.Current.Request;

            try
            {
                //Fetch Form Data
                Int64 UserID = Convert.ToInt64(httpRequest.Form["userid"]);
                string originalFileName = string.Empty;
                string newFileName = string.Empty;

                #region Fetch File(s)
                string DocumentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ,ConfigurationManager.AppSettings["FileUploadPath"].ToString() ,"UserProfile" , UserID.ToString());
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
                            imageUri = ConfigurationManager.AppSettings["DefaultImageUri"] + "UserProfile/" + UserID + "/" + file.FileName;

                            string filePath = Path.Combine(DocumentPath, originalFileName);

                            file.SaveAs(filePath);
                            UserRepository res = new UserRepository();
                            res.UpdateUserPhoto(UserID, imageUri, out Message);
                        }
                    }
                    else
                    {
                        result.Status = Constants.WebApiStatusFail;
                        result.Message = "No File To Upload";
                        result.Response = false;
                        return Ok(new { result.Status, data = result });
                    }
                }
                else
                {
                    result.Status = Constants.WebApiStatusFail;
                    result.Message = "User id is required";
                    result.Response = false;
                    return Ok(new { result.Status, data = result });
                }

                #endregion
            }
            catch (Exception ex)
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = "Exception occurred in uploading profile photo.  Exception: " + ex.Message;
                result.Response = false;
                return Ok(new { result.Status, data = result });
            }
            result.Status = Constants.WebApiStatusOk;
            result.Response = true;
            result.Message = "";
            return Ok(new { result.Status, data = result });

            //return Ok(new { Status = "Success", Message = "Profile photo uploaded successfully" });
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
                            res.UpdateIdentityDocument(UserID, DocumentType, profileImagePath, out Message);
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
