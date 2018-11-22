using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models
{
    public class Product
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
        public Nullable<System.DateTime> ModifyDate { get; set; }

    }
}
