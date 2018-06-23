using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.resources
{
    public class clientPrivateData
    {
        public  string username;
        public byte[] m_kdcAsSessionKey;
        public bool m_loginSucccess;
        public int m_localPort;
        public clientPrivateData()
        {
            m_kdcAsSessionKey = new byte[32];
        }
    }
}
