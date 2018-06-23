using LinqToSql;
using System;
using System.Linq;

namespace KdcService
{

    public class FtpDBservice
    {
        KdcFtpClientsDBDataContext kdcFtpdbContext = new KdcFtpClientsDBDataContext(); 

        public KdcFtpKey getKdcFtpKey(string name)
        {
            try
            {
                KdcFtpKey exsistUser = kdcFtpdbContext.KdcFtpKeys.Single(user => user.Name == name);

                Console.WriteLine("user with name " + name + " is exsists");
                return exsistUser;
            }
            catch
            {
                Console.WriteLine("user with name " + name + " not exsists");
                return null;
            }
        }
        

    }
}
