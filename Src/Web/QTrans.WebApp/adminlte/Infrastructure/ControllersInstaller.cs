
namespace adminlte.Infrastructure
{    
    using Castle.MicroKernel.Registration;
    using adminlte.Common;
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// The controllers installer.
    /// </summary>
    public class ControllersInstaller : IWindsorInstaller
    {
        /// <summary>
        /// The install.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="store">
        /// The store.
        /// </param>
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container != null)
            {
                container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());                               
              

                container.Register(Component.For<ISessionStorage>().ImplementedBy<SessionStorage>().LifestyleSingleton());
            }
        }
    }
}