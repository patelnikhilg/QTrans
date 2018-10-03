using QTrans.Contract;
using QTrans.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Alert
{
    /// <summary>
    /// Defeine Alert service class.
    /// </summary>
    public class AlertService : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;

        /// <summary>
        /// Concurrent dictionary to maintain the worker process.
        /// </summary>
        private ConcurrentDictionary<int, IManager> activeComponent;

        private AppLogger logger;

        #region "=================== Constructor =============================="

        public AlertService()
        {
            this.logger = new AppLogger("QTransAlertService");
            //this.billerComponent = new BillerManager();
            this.activeComponent = new ConcurrentDictionary<int, IManager>();
        }

        ~AlertService()
        {
            this.Dispose(false);
        }

        #endregion

        /// <summary>
        /// Start the service
        /// </summary>
        public void Start()
        {
            try
            {
                ////TODO: start the instnace.
            }
            catch (Exception exception)
            {
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool LoadConfig()
        {
            var flg = false;
            try
            {
                Console.WriteLine("Load Master configuration");
                if (AppConfig.Instance.Init())
                {
                    Console.WriteLine("Completed configuration loading.");
                }
                else
                {
                    Console.WriteLine("Fail configuration loading.");
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return flg;
        }

        /// <summary>
        /// Initialize the component.
        /// </summary>
        /// <returns></returns>
        private bool init()
        {
            var flg = true;
            this.logger.Info("Service initialization is started");

           

            this.logger.Info("Alert Service initialization is completed");
            return flg;
        }

        /// <summary>
        /// Stop the service
        /// </summary>
        public void Stop()
        {
            this.logger.Info("Alert Service is stopped");
            this.Dispose();
        }

        /// <summary>
        /// Dispose the resource which is allocated for this object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (disposing)
            {               
                IManager manager = null;
                foreach (var item in this.activeComponent)
                {
                    if (this.activeComponent.TryRemove(item.Key, out manager))
                    {
                        if (manager != null)
                        {
                            manager.Stop();
                            manager.Dispose();
                            manager = null;
                        }
                    }
                }
            }

            // Free any unmanaged objects here.
            disposed = true;
        }
    }
}
