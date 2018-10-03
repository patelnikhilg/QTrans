using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace QTrans.Alert
{
    class Program
    {
        static void Main(string[] args)
        {
            Initializ();
        }

        private static void Initializ()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<AlertService>(service =>
                {
                    service.ConstructUsing(() => new AlertService());
                    // the start and stop methods for the service
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.StartAutomaticallyDelayed();
                configure.EnableServiceRecovery(rc =>
                {
                    rc.RestartService(2); // restart the service after 2 minute                   
                });
                configure.SetServiceName(ConfigurationManager.AppSettings["ServiceName"]);
                configure.SetDisplayName(ConfigurationManager.AppSettings["ServiceDisplayName"]);
                configure.SetDescription(ConfigurationManager.AppSettings["ServiceDescription"]);
            });
        }
    }
}
