﻿using QTrans.Models;
using System;
using System.Collections.Generic;
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
                connector.AddInParameterWithValue("@RCBookCopyPath", vehicle.rcbookcopypath);
                connector.AddInParameterWithValue("@RTORegistrationNumber", vehicle.rtoregistrationnumber);
                connector.AddInParameterWithValue("@CompanyId", vehicle.companyid);
                connector.AddInParameterWithValue("@RegistrationDate", vehicle.registrationdate);
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
