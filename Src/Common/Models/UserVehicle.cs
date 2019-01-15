using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models
{
    public class UserVehicle
    {
        public long userid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string mobilenumber { get; set; }

        public ICollection<Vehicle> vehicles { get; set; }
        
    }
}
