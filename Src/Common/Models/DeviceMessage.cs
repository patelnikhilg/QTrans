using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models
{
    public class DeviceMessage
    {
        public DeviceMessage()
        {
            this.info = new Info();
            this.gps = new GPS();
            this.io = new IO();
            this.pwr = new Power();
            this.dbg = new DBG(); 
        }
        public int uid { get; set; }
        public Info info { get; set; }
        public GPS gps { get; set; }
        public IO io { get; set; }
        public Power pwr { get; set; }
        public DBG dbg { get; set; }
    }

    public class Info
    {
        public long dt { get; set; }

        public char txn { get; set; }
        public int msgekey { get; set; }
        public int msgid { get; set; }
        public string cmdkey { get; set; }

        public string cmdval { get; set; }
    }

    public class GPS
    {
        public char fix { get; set; }
        public decimal[] loc { get; set; }
        public int speed { get; set; }
        public int sat { get; set; }
        public int alt { get; set; }
        public int dir { get; set; }
        public long odo { get; set; }
    }

    public class IO
    {
        public int box { get; set; }
        public int ign { get; set; }
        public int gpi { get; set; }
        public int analog { get; set; }
        public int fuel { get; set; }
    }

    public class Power
    {
        public int main { get; set; }
        public int batt { get; set; }
        public int volt { get; set; }
    }

    public class DBG
    {
        public int[] status { get; set; }
        public string[] ver { get; set; }
        public string lib { get; set; }
    }
}
