using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    [Serializable]
    public class CMsgKdcToClientLoggin
    {
        public CMsgKdcToClientLoggin(string username, string password)
        {
            m_username = username;
            m_password = password;
        }
        public string m_username;
        public string m_password;
    }
}
