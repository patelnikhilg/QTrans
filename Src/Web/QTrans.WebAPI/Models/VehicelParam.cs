using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTrans.WebAPI.Models
{
    public class VehicelParam
    {
        public long userId { get; set; }
        public long vehicleId { get; set; }
        public long insuranceId { get; set; }
    }
}