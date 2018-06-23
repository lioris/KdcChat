using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class CLogInStatus
    {
        public string m_username;
        public string m_challenge;
        public bool m_logInFail;
        public CLogInStatus()
        {
        }
    }
}
