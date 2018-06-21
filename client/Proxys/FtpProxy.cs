using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class FtpProxy
    {
        private ChannelFactory<IFtpService> channel;
        private IFtpService proxy;
        #region Singelton
        private static FtpProxy _instance;

        private FtpProxy()
        {
            // creat a channel to comunicate with the game server
            channel = new ChannelFactory<IFtpService>("FtpServiceEndpoint");
            proxy = channel.CreateChannel();
        }

        public static FtpProxy Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FtpProxy();

                return _instance;
            }
        }
        #endregion

        internal IFtpService GetFtpProxy()
        {
            return proxy;
        }
    }
}

