using client.resources;
using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace client
{
    class ClientKdcCallBack : IClientKdcCallBack
    {
        public EventHandler<List<string>> newConnectedUserEvnt;
        public EventHandler<userPortData> openSessionChatEvnt;

        #region Singelton
        private static ClientKdcCallBack _instance;
        
        private ClientKdcCallBack(){

        }

        public static ClientKdcCallBack Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClientKdcCallBack();

                return _instance;
            }
        }
        #endregion


        public void printHello(string massage)
        {
            Console.WriteLine("lior");
        }

        public void addNewConnectedUser(string username, List<string> allUsers, int port)
        {
            List<string> nameList = new List<string>();

            allUsers.ForEach(delegate (String name)
            {
                if (ClientAllData.Instance.getMyUsername() != name)
                {
                    ClientAllData.Instance.addUserToPartnerUsers(name);
                    nameList.Add(name);
                }
            });

            var evt = newConnectedUserEvnt;
            if(evt != null)
            {
                evt(this, nameList);
            }

        }

        public void startChatSession(int remotePort, string remoteUserName, bool isMaster)
        {
            userPortData userPort = new userPortData();
            userPort.port = remotePort;
            userPort.userName = remoteUserName;
            userPort.isMaster = isMaster;
            var evt = openSessionChatEvnt;
            if (evt != null)
            {
                evt(this, userPort);
            }
        }

        public void removeDisconnectedUser(string massage)
        {
            throw new NotImplementedException();
        }

        public void startSession(string userName, int port)
        {
            if (ClientAllData.Instance.getMyUsername() != userName)
            {
                //clientAllData.Instance.addUserToPartnerUsers(massage);
                //session newSession = new session(clientAllData.Instance.getMyPort(), port);
                //clientAllData.Instance.addNewSession(newSession, massage);
            }
        }
    }
}



