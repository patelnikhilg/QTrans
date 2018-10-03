using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Alert
{
    internal class AppConfig
    {
        private static AppConfig instance;
        private AppConfig()
        {

        }
        public static AppConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppConfig();
                }

                return instance;
            }
        }

        public bool Init()
        {
            var flg = false;          

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ScheduleInterval"]))
            {
                this.SCheduleTimeInterval = 5;
            }
            else
            {
                this.SCheduleTimeInterval = Convert.ToInt32(ConfigurationManager.AppSettings["ScheduleInterval"]); ;
            }            

            return flg;
        }

        public int SCheduleTimeInterval { get; set; }
    }
}
