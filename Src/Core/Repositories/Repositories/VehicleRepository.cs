using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Repositories.Repositories
{
    public class VehicleRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private VehicleDataAccess instance;
        private long UserId;
        #region "=================== Constructor =============================="
        public VehicleRepository(long userid)
        {
            this.UserId = userid;
            this.instance = new VehicleDataAccess();
        }

        ~VehicleRepository()
        {
            this.Dispose(false);
        }
        #endregion
        public ResponseSingleModel<Vehicle> VehicleRegistration(Vehicle vehicle, out string message)
        {
            var result = new ResponseSingleModel<Vehicle>();
            long vehicleId = 0;
            message = string.Empty;
            if (this.instance.InsertUpdateVehicle(vehicle, out vehicleId, out message))
            {
                vehicle.vehicleid = vehicleId;
                result.Status = Constants.WebApiStatusOk;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = message;
            }
            result.Response = vehicle;

            return result;
        }

        public ResponseSingleModel<InsuranceDetails> Insurancedetails(InsuranceDetails insurance, out string message)
        {
            var result = new ResponseSingleModel<InsuranceDetails>();
            long insuranceId = 0;
            message = string.Empty;
            if (this.instance.InsertUpdateInsurance(insurance, out insuranceId, out message))
            {
                insurance.insuranceid = insuranceId;
                result.Status = Constants.WebApiStatusOk;
            }
            else
            {
                result.Status = Constants.WebApiStatusFail;
                result.Message = message;
            }
            result.Response = insurance;

            return result;
        }

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

                ////TODO: Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

        #endregion
    }
}
