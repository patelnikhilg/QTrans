using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.ViewModel.Common
{
    public class CountryState
    {
        public int StateId { get; set; }

        public string State { get; set; }
    }

    public class StateCity
    {
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; }
    }

    public class CityPincode
    {
        public int PincodeId { get; set; }
        public int CityId { get; set; }
        public string Pincode { get; set; }
    }
}
