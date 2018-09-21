using QTrans.DataAccess;
using QTrans.Models;
using QTrans.Models.ViewModel.Common;
using QTrans.Utility.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<MaterialType> GetMaterialType()
        {
            var data = instance.GetMaterialType();
            return DataAccessUtility.ConvertToList<MaterialType>(data);
        }

        public List<PackageType> GetPackageType()
        {
            var data = instance.GetPackageType();
            return DataAccessUtility.ConvertToList<PackageType>(data);
        }

        public List<VehicleType> GetVehicleType()
        {
            var data = instance.GetVehicleType();
            return DataAccessUtility.ConvertToList<VehicleType>(data);
        }

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

        public List<StateCity> GetCity()
        {
            if (InMemoryStorage.Instance.CityStorage.Count > 0)
            {
                return InMemoryStorage.Instance.CityStorage.Values.ToList();
            }
            else
            {
                var data = instance.GetCity();
                return DataAccessUtility.ConvertToList<StateCity>(data);
            }
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
            if (InMemoryStorage.Instance.CityStorage.Count > 0)
            {
                return InMemoryStorage.Instance.CityStorage.Values.Where(x => x.StateId == stateId).ToList();
            }

            return null;
        }

        public List<CityPincode> GetPincodeByCityId(int cityId)
        {
            if (InMemoryStorage.Instance.PincodeStorage.Count > 0)
            {
                return InMemoryStorage.Instance.PincodeStorage.Values.Where(x => x.CityId == cityId).ToList();  
            }

            return null;
        }

        #endregion
    }
}
