using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace QTrans.Repositories.Common
{
    public class TrackingCollection : IDisposable
    {
        private Timer dumpData;

        /// <summary>
        /// Flg for database operation.
        /// </summary>
        bool dbOpertion;

        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;

        /// <summary>
        /// Default milisecods of an Hour
        /// </summary>
        private int TimeInterval;

        private static TrackingCollection instance;

        /// <summary>
        /// Flg for dumping in database.
        /// </summary>
        private bool isDumping;

        private IList<LocationDetails> activeStorage;
        private IList<LocationDetails> passiveStorage;

        private TrackingRepository trackingRepository;

        #region "=================== Constructor =============================="

        private TrackingCollection()
        {
            this.activeStorage = new List<LocationDetails>();
            this.passiveStorage = new List<LocationDetails>();
            this.trackingRepository = new TrackingRepository();
        }

        ~TrackingCollection()
        {
            this.Dispose(false);
        }

        #endregion

        public void Init(int TimeInterval)
        {
            this.TimeInterval = TimeInterval > 5000 ? TimeInterval : 10000;
            this.StartTimer();
        }

        public static TrackingCollection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TrackingCollection();
                }
                return instance;
            }
        }


        public void DeviceLocation(DeviceMessage deviceMessage)
        {
            var locationdetail = new LocationDetails();
            locationdetail.UnitId = deviceMessage.uid;
            locationdetail.fix = deviceMessage.gps.fix;
            locationdetail.loc = string.Join(",", deviceMessage.gps.loc);
            locationdetail.speed = deviceMessage.gps.speed;
            locationdetail.sat = deviceMessage.gps.sat;
            locationdetail.alt = deviceMessage.gps.alt;
            locationdetail.dir = deviceMessage.gps.dir;
            locationdetail.odo = deviceMessage.gps.odo;
            locationdetail.deviceDatetime = DateTime.Now;//deviceMessage.dt;
            locationdetail.msgid = deviceMessage.info.msgid;
            this.LocationDetailSubmit(locationdetail);
        }

        /// <summary>
        /// Add location details for bulk insertion
        /// </summary>
        /// <param name="locationDetails">Object of location details.</param>
        public void LocationDetailSubmit(LocationDetails locationDetails)
        {
            if (!this.isDumping)
            {
                this.activeStorage.Add(locationDetails);
            }
            else
            {
                this.passiveStorage.Add(locationDetails);
            }
        }

        #region "============================= Timer==========================
        /// <summary>
        /// To Start the new timer 
        /// </summary>
        private void StartTimer()
        {
            this.dumpData = new Timer();
            this.dumpData.Elapsed += this.objInsertionTimer_Elapsed;
            this.dumpData.Interval = this.TimeInterval;
            this.dumpData.Start();
        }

        /// <summary>
        /// Timer Elapsed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objInsertionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.dbOpertion) return;
            this.dbOpertion = true;
            this.isDumping = !this.isDumping;
            //// dump all the location details in database. 

            try
            {
                DataTable dt = new DataTable();
                if (this.isDumping)
                {
                    if (this.activeStorage.Count > 0)
                    {
                        dt = DataAccessUtility.ToDataTable<LocationDetails>(this.activeStorage.ToList());
                    }
                }
                else
                {
                    if (this.passiveStorage.Count > 0)
                    {
                        dt = DataAccessUtility.ToDataTable<LocationDetails>(this.passiveStorage.ToList());
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {

                    if (this.trackingRepository.BulkInsertion(dt))
                    {
                        if (this.isDumping)
                        {
                            this.activeStorage.Clear();
                        }
                        else
                        {
                            this.passiveStorage.Clear();
                        }

                        ////TODO: log the operation with details like number data dump
                    }
                    else
                    {
                        ////TODO: log the operation
                    }
                }

            }
            catch
            {
                ////TODO: log the error
            }
            finally
            {
                this.dbOpertion = false;
            }
        }

        /// <summary>
        /// Dispose Clean Up timer
        /// </summary>
        private void DisposeTimer()
        {
            if (this.dumpData == null)
            {
                return;
            }

            this.dumpData.Enabled = false;
            this.dumpData.Stop();
            this.dumpData.Elapsed -= this.objInsertionTimer_Elapsed;
            this.dumpData.Dispose();
            this.dumpData = null;
        }

        #endregion     

        #region ========================= Dispose Method ==============
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (disposing)
            {
                this.DisposeTimer();
                if (this.activeStorage != null)
                {
                    this.activeStorage.Clear();
                    this.activeStorage = null;
                }

                if (this.passiveStorage != null)
                {
                    this.passiveStorage.Clear();
                    this.passiveStorage = null;
                }

                if (this.trackingRepository != null)
                {
                    this.trackingRepository.Dispose();
                }
                ////TODO: Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

        #endregion
    }
}
