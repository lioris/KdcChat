using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KdcService
{
    public class UserFtpServiceData
    {
        IClientFtpCallBack m_clientFtpCallBack;
        byte[] m_sessionKey;

        public UserFtpServiceData(byte[] userSessionKey, IClientFtpCallBack clientFtpCallBack)
        {
            m_clientFtpCallBack = clientFtpCallBack;
            m_sessionKey = userSessionKey;
        }

        public IClientFtpCallBack clientFtpCallBack
        {
            get { return m_clientFtpCallBack; }
            set { m_clientFtpCallBack = value; }
        }

        public byte[] SessionKey
        {
            get { return m_sessionKey; }
            set { m_sessionKey = value; }
        }
    }
}
