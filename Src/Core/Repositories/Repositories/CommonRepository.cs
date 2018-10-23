using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Models.ViewModel.Common;
using QTrans.Utility.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QTrans.Repositories.Repositories
{
    public class CommonRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private CommonDataAccess instance;
        #region "=================== Constructor =============================="
        public CommonRepository()
        {
            this.instance = new CommonDataAccess();
        }
        ~CommonRepository()
        {
            this.Dispose(false);
        }
        #endregion

        public bool InsertContactDetails(Contact contact)
        {
            return this.instance.InsertContactDetails(contact);
        }

        #region ==================== System Type ====================
        public ResponseCollectionModel<MaterialType> GetMaterialType()
        {
            var response = new ResponseCollectionModel<MaterialType>();
            if (InMemoryStorage.Instance.MaterialTypeStorage.Count > 0)
            {
                var lst = InMemoryStorage.Instance.MaterialTypeStorage.Values.ToList();
                response.Response = lst;
            }
            else
            {
                var data = instance.GetMaterialType();
                var lst = DataAccessUtility.ConvertToList<MaterialType>(data);
                response.Response = lst;
                foreach (var item in lst)
                {
                    if (InMemoryStorage.Instance.MaterialTypeStorage.Keys.Contains(item.materialtypeid))
                    {
                        InMemoryStorage.Instance.MaterialTypeStorage.AddOrUpdate(item.materialtypeid, item, (key, oldValue) => item);
                    }
                    else
                    {
                        InMemoryStorage.Instance.MaterialTypeStorage.TryAdd(item.materialtypeid, item);
                    }
                }
            }

            response.Status = Constants.WebApiStatusOk;

            return response;
        }

        public ResponseCollectionModel<PackageType> GetPackageType()
        {
            var response = new ResponseCollectionModel<PackageType>();
            if (InMemoryStorage.Instance.PackageTypeStorage.Count > 0)
            {
                var lst = InMemoryStorage.Instance.PackageTypeStorage.Values.ToList();
                response.Response = lst;
            }
            else
            {
                var data = instance.GetPackageType();
                var lst = DataAccessUtility.ConvertToList<PackageType>(data);
                response.Response = lst;
                foreach (var item in lst)
                {
                    if (InMemoryStorage.Instance.PackageTypeStorage.Keys.Contains(item.packagetypeid))
                    {
                        InMemoryStorage.Instance.PackageTypeStorage.AddOrUpdate(item.packagetypeid, item, (key, oldValue) => item);
                    }
                    else
                    {
                        InMemoryStorage.Instance.PackageTypeStorage.TryAdd(item.packagetypeid, item);
                    }
                }
            }

            response.Status = Constants.WebApiStatusOk;

            return response;
        }

        public ResponseCollectionModel<VehicleType> GetVehicleType()
        {
            var response = new ResponseCollectionModel<VehicleType>();
            if (InMemoryStorage.Instance.VehicleTypeStorage.Count > 0)
            {
                var lst = InMemoryStorage.Instance.VehicleTypeStorage.Values.ToList();
                response.Response = lst;
            }
            else
            {
                var data = instance.GetVehicleType();
                var lst = DataAccessUtility.ConvertToList<VehicleType>(data);
                response.Response = lst;
                foreach (var item in lst)
                {
                    if (InMemoryStorage.Instance.VehicleTypeStorage.Keys.Contains(item.vehicletypeid))
                    {
                        InMemoryStorage.Instance.VehicleTypeStorage.AddOrUpdate(item.vehicletypeid, item, (key, oldValue) => item);
                    }
                    else
                    {
                        InMemoryStorage.Instance.VehicleTypeStorage.TryAdd(item.vehicletypeid, item);
                    }
                }
            }

            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        #endregion

        #region =============== Area Peference ============
        public ResponseSingleModel<bool> InsertAreaPeference(long userId, int cityId)
        {
            var response = new ResponseSingleModel<bool>();
            response.Response = instance.InsertAreaPeference(userId, cityId);
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        public ResponseSingleModel<bool> DeleteAreaPeference(long userId, int cityId)
        {
            var response = new ResponseSingleModel<bool>();
            response.Response = instance.DeleteAreaPeference(userId, cityId);
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        public ResponseCollectionModel<AreaPreference> GetAreaPeferenceByUserId(long userId)
        {
            var response = new ResponseCollectionModel<AreaPreference>();
            var data = instance.GetAreaPeferenceByUserId(userId);
            response.Response = DataAccessUtility.ConvertToList<AreaPreference>(data);
            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        #endregion

        #region =========== State,City,Pincode==============

        public ResponseCollectionModel<CountryState> GetState()
        {
            var response = new ResponseCollectionModel<CountryState>();
            if (InMemoryStorage.Instance.StateStorage.Count > 0)
            {
                var lst = InMemoryStorage.Instance.StateStorage.Values.ToList();
                response.Response = lst;

            }
            else
            {
                var data = instance.GetState();
                var lst = DataAccessUtility.ConvertToList<CountryState>(data);
                response.Response = lst;
            }

            response.Status = Constants.WebApiStatusOk;
            return response;
        }

        public ResponseCollectionModel<StateCity> GetCity()
        {
            var result = new ResponseCollectionModel<StateCity>();
            if (InMemoryStorage.Instance.CityStorage.Count > 0)
            {
                result.Response = InMemoryStorage.Instance.CityStorage.Values.ToList();
            }
            else
            {
                var data = instance.GetCity();
                result.Response = DataAccessUtility.ConvertToList<StateCity>(data);
            }

            result.Status = Constants.WebApiStatusOk;
            return result;
        }

        public ResponseCollectionModel<CityPincode> GetPincode()
        {
            var result = new ResponseCollectionModel<CityPincode>();
            if (InMemoryStorage.Instance.PincodeStorage.Count > 0)
            {
                result.Response = InMemoryStorage.Instance.PincodeStorage.Values.ToList();
            }
            else
            {
                var data = instance.GetPincode();
                result.Response = DataAccessUtility.ConvertToList<CityPincode>(data);
            }

            result.Status = Constants.WebApiStatusOk;
            return result;
        }

        public ResponseCollectionModel<StateCity> GetCityByStateId(int stateId)
        {
            var result = new ResponseCollectionModel<StateCity>();
            if (InMemoryStorage.Instance.CityStorage.Count == 0)
            {
                InMemoryStorage.Instance.LoadLocationDetails(2);
            }

            result.Response = InMemoryStorage.Instance.CityStorage.Values.Where(x => x.StateId == stateId).ToList();
            result.Status = Constants.WebApiStatusOk;
            return result;
        }

        public ResponseCollectionModel<CityPincode> GetPincodeByCityId(int cityId)
        {
            var result = new ResponseCollectionModel<CityPincode>();
            if (InMemoryStorage.Instance.PincodeStorage.Count == 0)
            {
                InMemoryStorage.Instance.LoadLocationDetails(3);
            }

            result.Response = InMemoryStorage.Instance.PincodeStorage.Values.Where(x => x.CityId == cityId).ToList();
            result.Status = Constants.WebApiStatusOk;
            return result;
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
                if (this.instance != null)
                {
                    this.instance.Dispose();
                    this.instance = null;
                }

                ////Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }
        #endregion
    }
}
