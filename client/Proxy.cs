using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Proxy
    {
        private DuplexChannelFactory<IKdcService> channel;
        private IKdcService proxy;
        #region Singelton
        private static Proxy _instance;

        private Proxy()   {
            // creat a channel to comunicate with the game server
            channel =  new DuplexChannelFactory<IKdcService>(ClientCallBack.Instance, "KdcServiceEndpoint");
            proxy = channel.CreateChannel();
        }

        public static Proxy Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Proxy();

                return _instance;
            }
        }
        #endregion

        internal IKdcService GetProxy()
        {
            return proxy;
        }
    }
}
