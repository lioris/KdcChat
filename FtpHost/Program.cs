using ftpService;
using System;
using System.ServiceModel;

namespace FtpHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("wait while FTP SERVER is starting......");
            ServiceHost host = new ServiceHost(typeof(FtpService));

            host.Open();

            Console.WriteLine("The FTP Server is ON");
            Console.ReadLine();

            host.Close();
            Console.WriteLine("The FTP Server is OFF");
        }
    }
}
