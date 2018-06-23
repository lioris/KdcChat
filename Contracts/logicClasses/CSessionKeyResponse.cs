using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class CSessionKeyResponse
    {
        public byte[] m_dstPort;
        public byte[] m_sessionKeyA;
        public byte[] m_sessionKeyB;
        public CSessionKeyResponse()
        {
            m_sessionKeyA = new byte[128];
            m_sessionKeyB = new byte[128];
            m_dstPort = new byte[8];
        }

    }
}
