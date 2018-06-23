using client.resources;
using common;
using Contracts;
using LinqToSql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWin.xaml
    /// </summary>
    public partial class MainWin : Window
    {
        private readonly BackgroundWorker getSessionKeyWorker = new BackgroundWorker();
        private readonly BackgroundWorker startChatWindowWorker = new BackgroundWorker();
        private IKdcService proxy;
        public MainWin()
        {
            InitializeComponent();

            getSessionKeyWorker.DoWork += getSessionKeyWorker_DoWork;
            getSessionKeyWorker.RunWorkerCompleted += getSessionKeyWorker_RunWorkerCompleted;

            ClientKdcCallBack.Instance.newConnectedUserEvnt += (sender, message_contant) =>
            {
                Dispatcher.Invoke(() => addUserToPartnerUsers(message_contant));
            };


            ClientKdcCallBack.Instance.openSessionChatEvnt += (sender, message_contant) =>
            {
                Dispatcher.Invoke(() => openChatSession(message_contant));
            };



            proxy = KdcProxy.Instance.GetProxy();
            List<string> allUsers = proxy.getAllConnectedUsers();
            allUsers.Remove(ClientAllData.Instance.getMyUsername());
            addUserToPartnerUsers(allUsers);
        }

        private void getSessionKeyWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void getSessionKeyWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
            string partnerUsernameInvoked = string.Empty;
            

            Dispatcher.BeginInvoke(new Action(delegate
            {
                partnerUsernameInvoked = connectedUserComboBox.Text;
            }));


            Thread.Sleep(100);

            clientPrivateData user = ClientAllData.Instance.getMyClient();
            CSessionParams sessionPrm = new CSessionParams();
            sessionPrm.client1UserName = user.username; 
            sessionPrm.client2UserName = partnerUsernameInvoked;
            CSessionKeyResponse sessionRespons = proxy.GetChatSessionParams(sessionPrm); // blocking
            if(sessionRespons == null)
            {
                //report error
                return;
            }
            byte[] sessionKey = CAes.SimpleDecrypt(sessionRespons.m_sessionKeyA, user.m_kdcAsSessionKey, user.m_kdcAsSessionKey);
            byte[] sessionPartnerData = CAes.SimpleDecrypt(sessionRespons.m_sessionKeyB, user.m_kdcAsSessionKey, user.m_kdcAsSessionKey);
            session sessionData = ClientAllData.Instance.getSession(partnerUsernameInvoked);
            sessionData.setSessionKey(sessionKey, sessionPartnerData);
            sessionData.startSending();
        }

        private void start_chat_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!getSessionKeyWorker.IsBusy)
            {
                getSessionKeyWorker.RunWorkerAsync();
            }
        }

        public void openChatSession(userPortData remoteUserData)
        {
            session chatSession = new session(ClientAllData.Instance.getMyClient().m_localPort, remoteUserData.port);
            ClientAllData.Instance.addNewSession(chatSession, remoteUserData.userName);
            if(!remoteUserData.isMaster)
            {
                session retChatSession = ClientAllData.Instance.getSession(remoteUserData.userName);
                retChatSession.startReceving();
            }

        }

        public void openChatWindow(chatWindowParams chatWinParams)
        {

        }

        public void addUserToPartnerUsers(List<string> usernames)
        {
            connectedUserComboBox.Items.Clear();
            usernames.ForEach(delegate (String name)
            {
                connectedUserComboBox.Items.Add(name);
            });
        }

        private void ftpConnectButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
