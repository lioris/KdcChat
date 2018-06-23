using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.resources;
using LinqToSql;

namespace client
{
    class ClientAllData
    {
        //private DuplexChannelFactory<IKdcService> channel;
        //private IKdcService proxy;
        private clientPrivateData m_myUser;
        private List<string> m_partnerUser;
        private Dictionary<string, session> m_partnerSession;
        #region Singelton
        private static ClientAllData _instance;

        private ClientAllData()
        {
            m_myUser = new clientPrivateData();
            m_partnerUser = new List<string>();
            m_partnerSession = new Dictionary<string, session>();
        }

        public static ClientAllData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClientAllData();

                return _instance;
            }
        }
        #endregion


        internal clientPrivateData getMyClient()
        {
            return m_myUser;
        }

        internal void setMyClient(clientPrivateData user)
        {
            m_myUser = user;
        }

        internal List<string> getAllPartnersUsers()
        {
            return m_partnerUser;
        }

        internal void addUserToPartnerUsers(string partnerUsername)
        {
            if(!m_partnerUser.Contains(partnerUsername))
            {
                m_partnerUser.Add(partnerUsername);
            }
        }

        internal void addNewSession(session sessionData, string username)
        {
            if(!m_partnerSession.ContainsKey(username))
            {
                m_partnerSession.Add(username, sessionData);
            }
        }

        internal session getSession( string username)
        {
            return m_partnerSession[username];
        }

        internal void setSessionSessionKey(string username, byte[] key)
        {
            m_partnerSession[username].m_sessionKey = key;
        }

        internal void setMyUsername(string username)
        {
            m_myUser.username = username;
        }

        internal string getMyUsername()
        {
           return m_myUser.username;
        }
    }
}
