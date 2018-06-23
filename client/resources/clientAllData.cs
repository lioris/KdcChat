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
        private User m_myUser;
        private int m_localPort;
        private List<string> m_partnerUser;
        private Dictionary<string, session> m_partnerSession;
        #region Singelton
        private static ClientAllData _instance;

        private ClientAllData()
        {
            m_myUser = new User();
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


        internal User getMyClient()
        {
            return m_myUser;
        }

        internal void setMyClient(User user)
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

        internal void setMyUsername(string username)
        {
            m_myUser.Name = username;
        }

        internal string getMyUsername()
        {
           return m_myUser.Name;
        }

        internal void setMyPort(int port)
        {
            m_localPort = port;
        }

        internal int getMyPort()
        {
            return m_localPort;
        }
    }
}
