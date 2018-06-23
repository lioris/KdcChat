using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace client.resources
{
    [Serializable]
    public class messageCreateChatRemoteToMaster
    {
        public string challenge;
        public byte[] sessionKey;
        public messageCreateChatRemoteToMaster()
        {
        }
    }
}
