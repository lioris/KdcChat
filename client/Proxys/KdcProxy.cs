using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class KdcProxy
    {
        private DuplexChannelFactory<IKdcService> channel;
        private IKdcService proxy;
        #region Singelton
        private static KdcProxy _instance;

        private KdcProxy()   {
            // creat a channel to comunicate with the game server
            channel =  new DuplexChannelFactory<IKdcService>(ClientKdcCallBack.Instance, "KdcServiceEndpoint");
            proxy = channel.CreateChannel();
        }

        public static KdcProxy Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new KdcProxy();

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
