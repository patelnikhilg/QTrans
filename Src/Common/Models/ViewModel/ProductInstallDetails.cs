using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.ViewModel
{
    public class ProductInstallDetails
    {
        public long productId { get; set; }
        public int UnitId { get; set; }
        public string IMEI { get; set; }
        public System.DateTime RegisterDate { get; set; }
        public string Version { get; set; }
        public string FirmwareVersion { get; set; }
        public System.DateTime WarrentyExpDate { get; set; }
        public Int16 Status { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public long dtlProductId { get; set; }
        public long VehicleId { get; set; }
        public System.DateTime InsttalationDate { get; set; }
        public string UnitName { get; set; }
        public bool InstallStatus { get; set; }

        public string rtoregistrationnumber { get; set; }
        public string vehicletype { get; set; }
        public string manufacturername { get; set; }
        public long installer { get; set; }
    }
}
