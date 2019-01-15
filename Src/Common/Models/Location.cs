using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models
{
    public class Location : LocationDetails
    {
        public long LocationId { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class LocationDetails
    {
        public int UnitId { get; set; }
        public char fix { get; set; }
        public string loc { get; set; }
        public int speed { get; set; }
        public int sat { get; set; }
        public int alt { get; set; }
        public int dir { get; set; }
        public long odo { get; set; }
        public DateTime deviceDatetime { get; set; }
        public long msgid { get; set; }
    }
}
