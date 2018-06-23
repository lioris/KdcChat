using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.logicClasses
{
    public class CKdcToClientLogInData
    {
        public string m_username; // encrypted
        public byte[] m_kdcAsSessionKey; // encrypted
        public string m_challenge;
        public byte[] m_localPort;

        public CKdcToClientLogInData()
        {
            //m_kdcAsSessionKey = new byte[128];
            //m_localPort = new byte[32];
        }
    }
}
