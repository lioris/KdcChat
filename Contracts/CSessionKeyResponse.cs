using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class CSessionKeyResponse
    {
        public CSessionKeyResponse()
        {
            m_sessionKeyA = new byte[256];
            m_sessionKeyB = new byte[256];
        }
        public byte[] m_sessionKeyA;
        public byte[] m_sessionKeyB;
    }
}
