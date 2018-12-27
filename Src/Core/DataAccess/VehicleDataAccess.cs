using QTrans.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.DataAccess
{
    public class VehicleDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public VehicleDataAccess()
        {
        }

        ~VehicleDataAccess()
        {
            this.Dispose(false);
        }
        #endregion

        public bool InsertUpdateVehicle(Vehicle vehicle, out long identity, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertUpdateVehicle", true))
            {
                connector.AddInParameterWithValue("@VehicleId", vehicle.vehicleid);
                connector.AddInParameterWithValue("@VehicleType", vehicle.vehicletype);
                connector.AddInParameterWithValue("@manufacturerName", vehicle.manufacturername);
                connector.AddInParameterWithValue("@Descrition", vehicle.descrition);
                connector.AddInParameterWithValue("@manufacturerYear", vehicle.manufactureryear);
                connector.AddInParameterWithValue("@TotalWheel", vehicle.totalwheel);
                connector.AddInParameterWithValue("@weightCapacity", vehicle.weightcapacity);
                connector.AddInParameterWithValue("@rcbookcopypath", vehicle.rcbookcopypath);
                //connector.AddInParameterWithValue("@RCBookCopyPath", vehicle.rcbookcopypath);
                connector.AddInParameterWithValue("@rtoregistrationnumber", vehicle.rtoregistrationnumber);
                connector.AddInParameterWithValue("@companyid", vehicle.companyid);
                connector.AddInParameterWithValue("@DriverName", vehicle.drivername);
                connector.AddInParameterWithValue("@DriverNumber", vehicle.drivernumber);
                connector.AddInOutParameterWithValue("@identity", SqlDbType.BigInt);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                connector.AddInParameterWithValue("@userid", vehicle.userid);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = vehicle.vehicleid == 0 ? Convert.ToInt64(connector.GetParamaeterValue("@identity")) : vehicle.vehicleid;
            }

            return rowEffected > 0;
        }

        public bool InsertUpdateInsurance(InsuranceDetails insurance, out long identity, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_InsertUpdateInsurance", true))
            {
                connector.AddInParameterWithValue("@InsuranceId", insurance.insuranceid);
                connector.AddInParameterWithValue("@InsuranceName", insurance.insurancename);
                connector.AddInParameterWithValue("@InsuranceNumber", insurance.insurancenumber);
                connector.AddInParameterWithValue("@CompanyName", insurance.companyname);
                connector.AddInParameterWithValue("@Description", insurance.description);
                connector.AddInParameterWithValue("@InsuranceDate", insurance.insurancedate);
                connector.AddInParameterWithValue("@ExpireDate", insurance.expiredate);
                connector.AddInParameterWithValue("@VehicleId", insurance.vehicleid);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
                identity = insurance.vehicleid == 0 ? Convert.ToInt64(connector.GetParamaeterValue("@insurance")) : insurance.vehicleid;
            }

            return rowEffected > 0;
        }

        public DataTable GetById(long vehicleId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetVehicleById", true))
            {
                connector.AddInParameterWithValue("@vehicleId", vehicleId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetInsuranceById(long vehicleId, long InsuranceId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetInsuranceById", true))
            {
                connector.AddInParameterWithValue("@InsuranceId", InsuranceId);
                connector.AddInParameterWithValue("@vehicleId", vehicleId);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetVehicleListByUserId(long userId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("Usp_GetVehicleListByUserId", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                dt = connector.GetDataTable();
            }

            return dt;
        }




        public int updateRcPhoto(Int64 truckid, Int64 userID, string filePath, bool isDefault, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("Usp_UpdateRCPhoto", true))
            {
                connector.AddInParameterWithValue("@truckID", truckid );
                connector.AddInParameterWithValue("@photo", filePath);
                connector.AddInParameterWithValue("@UserId", userID);
                connector.AddInParameterWithValue("@IsDefault", isDefault);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return rowEffected;
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
