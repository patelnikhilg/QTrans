using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models
{
    public class ProductInstall
    {
        public long dtlProductId { get; set; }
        public long productId { get; set; }
        public long VehicleId { get; set; }
        public System.DateTime InsttalationDate { get; set; }
        public string UnitName { get; set; }
        public bool Status { get; set; }
        public System.DateTime createddate { get; set; }
        public Nullable<System.DateTime> modifydate { get; set; }

    }
}
