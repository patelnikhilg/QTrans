using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Models.ViewModel.Common
{
    public class LoggingMessage
    {
        public int LogType { get; set; }

        public long UserId { get; set; }

        public string LogMessage {get;set;}

        public string ModuleName { get; set; }

        public string Operation { get; set; }       

        public DateTime LogDate { get; set; }

        public Exception ExceptionObj { get; set; }

        public string PrepareLogMessage()
        {
            return string.Format("Date : {0}=>Userid {1}, operation:{2}, Module:{3} => {4}", UserId, LogDate, Operation, ModuleName, LogMessage);
        }
    }
}
