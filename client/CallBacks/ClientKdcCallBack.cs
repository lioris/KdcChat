using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace client
{
    class ClientKdcCallBack : IClientKdcCallBack
    {
 
        #region Singelton
        private static ClientKdcCallBack _instance;
        
        private ClientKdcCallBack(){

        }

        public static ClientKdcCallBack Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClientKdcCallBack();

                return _instance;
            }
        }
        #endregion


        public void printHello(string massage)
        {
            Console.WriteLine("lior");
        }
    }
}



