using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ResponseModel;
using QTrans.Models.ViewModel.Common;
using QTrans.Utility.Common;
using System.Collections.Generic;
using System.Linq;

namespace QTrans.Repositories.Repositories
{
    public class CommonRepository
    {
        private CommonDataAccess instance;
        public CommonRepository()
        {
            this.instance = new CommonDataAccess();
        }

        public bool InsertContactDetails(Contact contact)
        {
            return this.instance.InsertContactDetails(contact);
        }

        #region ==================== System Type ====================
        public List<MaterialType> GetMaterialType()
        {
            if (InMemoryStorage.Instance.MaterialTypeStorage.Count > 0)
            {
                return InMemoryStorage.Instance.MaterialTypeStorage.Values.ToList();
            }
            else
            {
                var data = instance.GetMaterialType();
                var lst = DataAccessUtility.ConvertToList<MaterialType>(data);
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

                return lst;
            }
        }

        public List<PackageType> GetPackageType()
        {
            if (InMemoryStorage.Instance.PackageTypeStorage.Count > 0)
            {
                return InMemoryStorage.Instance.PackageTypeStorage.Values.ToList();
            }
            else
            {
                var data = instance.GetPackageType();
                var lst = DataAccessUtility.ConvertToList<PackageType>(data);
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

                return lst;
            }
        }

        public List<VehicleType> GetVehicleType()
        {
            if (InMemoryStorage.Instance.VehicleTypeStorage.Count > 0)
            {
                return InMemoryStorage.Instance.VehicleTypeStorage.Values.ToList();
            }
            else
            {
                var data = instance.GetVehicleType();
                var lst = DataAccessUtility.ConvertToList<VehicleType>(data);
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

                return lst;
            }
        }

        #endregion

        #region =============== Area Peference ============
        public bool InsertAreaPeference(long userId, int cityId)
        {
            return instance.InsertAreaPeference(userId, cityId);
        }

        public bool DeleteAreaPeference(long userId, int cityId)
        {
            return instance.DeleteAreaPeference(userId, cityId);
        }

        public List<AreaPreference> GetAreaPeferenceByUserId(long userId)
        {
            var data = instance.GetAreaPeferenceByUserId(userId);
            return DataAccessUtility.ConvertToList<AreaPreference>(data);
        }

        #endregion

        #region =========== State,City,Pincode==============

        public List<CountryState> GetState()
        {
            if (InMemoryStorage.Instance.StateStorage.Count > 0)
            {
                return InMemoryStorage.Instance.StateStorage.Values.ToList();
            }
            else
            {
                var data = instance.GetState();
                return DataAccessUtility.ConvertToList<CountryState>(data);
            }
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

        public List<CityPincode> GetPincode()
        {
            if (InMemoryStorage.Instance.PincodeStorage.Count > 0)
            {
                return InMemoryStorage.Instance.PincodeStorage.Values.ToList();
            }
            else
            {
                var data = instance.GetPincode();
                return DataAccessUtility.ConvertToList<CityPincode>(data);
            }
        }

        public List<StateCity> GetCityByStateId(int stateId)
        {
            if (InMemoryStorage.Instance.CityStorage.Count == 0)
            {
                InMemoryStorage.Instance.LoadLocationDetails(2);
            }

            return InMemoryStorage.Instance.CityStorage.Values.Where(x => x.StateId == stateId).ToList();
        }

        public List<CityPincode> GetPincodeByCityId(int cityId)
        {
            if (InMemoryStorage.Instance.PincodeStorage.Count == 0)
            {
                InMemoryStorage.Instance.LoadLocationDetails(3);
            }

            return InMemoryStorage.Instance.PincodeStorage.Values.Where(x => x.CityId == cityId).ToList();
        }

        #endregion
    }
}
