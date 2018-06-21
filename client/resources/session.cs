using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.resources
{
    public class session
    {
        private byte[] m_sessionKye;
        public session(byte[] sessionKey)
        {
            m_sessionKye = sessionKey;
        }
    }
}
