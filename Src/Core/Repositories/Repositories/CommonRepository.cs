using QTrans.DataAccess;
using QTrans.Models;
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
    }
}
