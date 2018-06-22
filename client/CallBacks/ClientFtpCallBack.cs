using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.CallBacks
{
    class ClientFtpCallBack : IClientFtpCallBack
    {
        #region Singelton
        private static ClientFtpCallBack _instance;

        private ClientFtpCallBack()  {

        }

        public static ClientFtpCallBack Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClientFtpCallBack();

                return _instance;
            }
        }
        #endregion


        public void printHelloFtp(string massage)
        {
            Console.WriteLine("lior");
        }
    }
}
