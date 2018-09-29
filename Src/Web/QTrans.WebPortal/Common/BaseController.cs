using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QTrans.WebPortal.Common
{
    /// <summary>
    /// Base controller for all controllers
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// An object of the session storage information
        /// </summary>
        protected readonly ISessionStorage sessionStorage;            

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController" /> class.
        /// </summary>
        public BaseController() : this(DependencyResolver.Current.GetService<ISessionStorage>())
        {
        }

        public long UserId
        {
            get { return (this.sessionStorage.GetValue("UserSession") as UserSession).UserId; }
        }

        /// <summary>
        /// Initialization of the base controller
        /// </summary>
        /// <param name="sessionStorage"></param>        
        public BaseController(ISessionStorage sessionStorage)
        {
            this.sessionStorage = sessionStorage;
        }

        /// <summary>
        /// Executed when any action gets executed
        /// </summary>
        /// <param name="filterContext">Provides filter context</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext == null)
            {
                return;
            }

            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// Executed when action is executing
        /// </summary>
        /// <param name="filterContext">Provides filter context</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext", @"Input parameter can't be NULL here.");
            }
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                          || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {
                var userName = string.Empty;
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    //TODO: Need to check if fesible than use it otherwise make login and store user session in storage
                    userName = filterContext.HttpContext.User.Identity.Name;
                    userName = userName.Substring(userName.IndexOf('\\') + 1);
                    UserSession userInformation = null;
                    if (this.sessionStorage.IsSessionHasValue("UserSession"))
                    {
                        userInformation = this.sessionStorage.GetValue("UserSession") as UserSession;
                    }
                    else
                    {
                        ///TODO: redirect ot login screen.
                        RedirectToAction("../login/login");
                        ///this.sessionStorage.SetValue("UserSession", userInformation);
                    }

                    if (userInformation != null)
                    {
                       ViewBag.LoginUserName = userInformation.LoginUserName;// string.Concat(userInformation.FirstName, " ", userInformation.LastName);
                       ViewBag.UserId = userInformation.UserId;
                    }                   
                }
                else
                {
                    RedirectToAction("../login/login");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}