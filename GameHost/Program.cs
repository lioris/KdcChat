using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("wait while KDC SERVER is starting......");
            ServiceHost host = new ServiceHost(typeof(KdcService.KdcService));

            host.Open();

            Console.WriteLine("The KDC Server is ON");
            Console.ReadLine();

            host.Close();
            Console.WriteLine("The KDC Server is OFF");
        }
    }
}
