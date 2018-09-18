﻿
namespace QTrans.WebPortal.Infrastructure
{
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;

    using Castle.MicroKernel;

    /// <summary>
    /// The windsor controller factory.
    /// </summary>
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// The kernel.
        /// </summary>
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorControllerFactory"/> class.
        /// </summary>
        /// <param name="kernel">
        /// The kernel.
        /// </param>
        public WindsorControllerFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        /// <summary>
        /// The release controller.
        /// </summary>
        /// <param name="controller">
        /// The controller.
        /// </param>
        public override void ReleaseController(IController controller)
        {
            this.kernel.ReleaseComponent(controller);
        }

        /// <summary>
        /// The get controller instance.
        /// </summary>
        /// <param name="requestContext">
        /// The request context.
        /// </param>
        /// <param name="controllerType">
        /// The controller type.
        /// </param>
        /// <returns>
        /// The <see cref="IController"/>.
        /// </returns>
        /// <exception cref="HttpException">
        /// HttpException is thrown.
        /// </exception>
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                if (requestContext == null)
                {
                    throw new ArgumentNullException("requestContext", @"Input parameter can't be NULL here.");
                }

                throw new HttpException(404, string.Format(CultureInfo.InvariantCulture, "The controller path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }

            return (IController)this.kernel.Resolve(controllerType);
        }
    }
}