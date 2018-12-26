using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTrans.WebAPI.Models
{
    public class ProductParam
    {
        public long userId { get; set; }
        public long productId { get; set; }
        public long DtlproductId { get; set; }
        public long vehicleId { get; set; }
    }

    public class ProductInstallParam
    {
        public long userId { get; set; }
        public long installerId { get; set; }
    }
}