using QTrans.Models;
using QTrans.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var l = new LocationDetails();
            l.fix = '0';
            l.UnitId = 212;
            l.loc= "123.152,158.0";
            l.speed = 789;
            l.sat = 454;
            l.alt = 2342;
            l.msgid = 12;
            l.odo = 53;
            l.deviceDatetime = DateTime.Now;
            TrackingCollection.Instance.Init(6000);
            TrackingCollection.Instance.LocationDetailSubmit(l);
            var t= new Random(6).Next();
            Console.ReadKey(); 
        }
    }
}
