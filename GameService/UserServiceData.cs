using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KdcService
{
    public class UserServiceData
    {
        IClientKdcCallBack m_clientKdcCallBack;
        byte[] m_sessionKey;
        string m_logginChallenge;

        public UserServiceData(byte[] userSessionKey, IClientKdcCallBack clientKdcCallBack)
        {
            m_clientKdcCallBack = clientKdcCallBack;
            m_sessionKey = userSessionKey;
        }

        public IClientKdcCallBack clientKdcCallBack
        {
            get { return m_clientKdcCallBack; }
            set { m_clientKdcCallBack = value; }
        }

        public byte[] SessionKey
        {
            get { return m_sessionKey; }
            set { m_sessionKey = value; }
        }

        public string logginChallenge
        {
            get { return m_logginChallenge; }
            set { m_logginChallenge = value; }
        }
    }
}
