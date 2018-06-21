using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.resources;
using LinqToSql;

namespace client
{
    class clientAllData
    {
        //private DuplexChannelFactory<IKdcService> channel;
        //private IKdcService proxy;
        private User m_myUser;
        private List<string> m_partnerUser;
        private Dictionary<string, session> m_partnerSession;
        #region Singelton
        private static clientAllData _instance;

        private clientAllData()
        {
            m_myUser = new User();
            m_partnerUser = new List<string>();
            m_partnerSession = new Dictionary<string, session>();
        }

        public static clientAllData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new clientAllData();

                return _instance;
            }
        }
        #endregion


        internal User getMyClient()
        {
            return m_myUser;
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

    }
}
