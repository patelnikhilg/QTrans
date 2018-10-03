using System;
using System.ComponentModel.DataAnnotations;

namespace QTrans.Models
{
    public class AreaPreference
    {
        public long preferenceId { get; set; }
        public long UserId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string State { get; set; }
        [DisplayFormat(DataFormatString = Constants.FormatDateTime)]
        public DateTime CreateDate { get; set; }
    }
    public class AreaPreferenceParam
    {
        public long UserId { get; set; }
        public int CityId { get; set; }
    }
}
