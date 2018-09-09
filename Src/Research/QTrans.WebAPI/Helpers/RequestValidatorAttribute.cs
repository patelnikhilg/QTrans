using QTrans.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace QTrans.WebAPI.Helpers
{
    public class RequestValidatorAttribute : ActionFilterAttribute
    {
        private AppLogger log = new AppLogger("QTransAPILogger");

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            bool _isApiKey = false;
            bool _isValidUser = false;
            //XmlConfigurator.Configure();
            log.Info("New request: ");

            if (ConfigurationManager.AppSettings["app.environment"] == "local")
            {
                _isApiKey = true;
                _isValidUser = true;
            }
            else if (actionContext.Request.Headers.Contains("platform"))
            {
                string platform = actionContext.Request.Headers.GetValues("platform").FirstOrDefault();
                if (platform.ToLower() == "web")
                {
                    if (actionContext.Request.Headers.Contains("webApiKey"))
                    {
                        //Fetch web api key from header and validate
                        string _apiKeyEncoded = actionContext.Request.Headers.GetValues("webApiKey").FirstOrDefault();
                        _isApiKey = validateAPIKey(_apiKeyEncoded);

                        //If api key is valid then validate user
                        if (_isApiKey)
                            _isValidUser = validateUser(actionContext);
                    }
                }
                else if (platform.ToLower() == "mobile")
                {
                    if (actionContext.Request.Headers.Contains("mobileApiKey"))
                    {
                        //Fetch mobile api key from header and validate
                        string _apiKeyEncoded = actionContext.Request.Headers.GetValues("mobileApiKey").FirstOrDefault();
                        _isApiKey = validateAPIKey(_apiKeyEncoded);

                        //If api key is valid then validate user
                        if (_isApiKey)
                            _isValidUser = validateUser(actionContext);
                    }
                }
                else
                {
                    _isApiKey = false;
                    _isValidUser = false;
                }
            }

            if (!_isApiKey)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Credentials");
            }
            if (!_isValidUser)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Session Expired");
            }
        }

        public bool validateUser(HttpActionContext actionContext)
        {
            bool _isValidUser = false;


            bool skipAuthorization = actionContext.ActionDescriptor.GetCustomAttributes<System.Web.Http.AllowAnonymousAttribute>().Any() ||
                    actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<System.Web.Http.AllowAnonymousAttribute>().Any();

            if (!skipAuthorization)
            {
                if (actionContext.Request.Headers.Contains("AuthorizationToken"))
                {
                    string value = actionContext.Request.Headers.GetValues("AuthorizationToken").FirstOrDefault();
                    if (value != null)
                    {
                        try
                        {
                            List<string> userDetails = EncryptDecryptHelper.Decrypt(value).Split(new string[] { "::" }, StringSplitOptions.None).ToList();
                            if (userDetails.Count == 4)
                            {
                                DateTime datetime = DateTime.ParseExact(userDetails[3], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                                if (datetime != null)
                                {
                                    TimeSpan diff = DateTime.Now - datetime;
                                    if (diff.TotalMinutes < Convert.ToInt32(ConfigurationManager.AppSettings["SessionMinutes"].ToString()))
                                    {
                                        _isValidUser = true;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _isValidUser = false;
                        }
                    }
                }
                else
                    _isValidUser = false;
            }
            else
                _isValidUser = true;



            return _isValidUser;
        }


        public bool validateAPIKey(string _apiKeyEncoded)
        {
            bool _isApiKey = false;

            //string _apiKeyEncoded = actionContext.Request.Headers.GetValues("API_KEY").FirstOrDefault();

            //Decrypt Api Key data from base64 string (Api Key is sent as base64 encoded string)
            byte[] data = Convert.FromBase64String(_apiKeyEncoded);
            string _apiKey = System.Text.Encoding.UTF8.GetString(data);

            //Get Data from configuration
            string apiKey = ConfigurationManager.AppSettings["apiKey"].ToString();

            if (!string.IsNullOrWhiteSpace(_apiKey) && _apiKey == apiKey) //if api key sent in header is same as api key in configuration, then it is valid
            {
                _isApiKey = true;
            }

            return _isApiKey;

        }
    }
}