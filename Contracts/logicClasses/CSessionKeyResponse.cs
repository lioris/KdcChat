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
        }

    }
}
