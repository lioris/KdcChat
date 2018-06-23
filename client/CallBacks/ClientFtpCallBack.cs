using Contracts;
using System;

namespace client.CallBacks
{
    class ClientFtpCallBack : IClientFtpCallBack
    {
        public event EventHandler<bool> finishRequstConnectionProcessEvent;

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

        public void finishRequstConnectionProcess(bool finishStatus)
        {
            finishRequstConnectionProcessEvent?.Invoke(this, finishStatus);
        }
        #endregion


    }
}
