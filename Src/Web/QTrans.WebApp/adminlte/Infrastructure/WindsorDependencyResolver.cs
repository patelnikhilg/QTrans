
namespace adminlte.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Castle.Windsor;

    /// <summary>
    /// The windsor dependency resolver.
    /// </summary>
    public class WindsorDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// The container.
        /// </summary>
        private readonly IWindsorContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public WindsorDependencyResolver(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// The get service.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return this.container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// The get services.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return (object[])this.container.ResolveAll(serviceType);
        }
    }
}