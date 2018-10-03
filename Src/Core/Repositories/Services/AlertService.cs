using QTrans.Contract;
using QTrans.DataAccess.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Repositories.Services
{
    public class AlertService : IService
    {
        private AlertDataAccess alert;

        private int BatchSize;      

        public AlertService(int batchSize)
        {
            this.IsRunning = false;
            this.IsRunning = false;
            this.BatchSize = batchSize;
        }
        private bool IsRunning { get; set; }

        private bool IsStopped { get; set; }

        bool IService.Init()
        {
            this.alert = new AlertDataAccess();

            return true;
        }

        bool IService.Start()
        {
            this.IsRunning = true;
            while (true)
            {
                var dt=this.alert.GetPostAlert(this.BatchSize);
                if (dt.Rows.Count > 0)
                {
                    if(this.IsStopped)
                    {
                        break;
                    }

                    ////TODO: read the table and send 
                }
                else
                {
                    break;
                }
            }
            this.IsRunning = false;
            return true;
        }

        bool IService.Stop()
        {
            this.IsStopped = true;
            return true;
        }
    }
}
