using client.resources;
using common;
using Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
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
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        public EventHandler <string> setShowChatWindowEvnt;
        public EventHandler <string> addLineToChatEvnt;
        userPortData m_userPort;
        CSessionKeyResponse m_sessionRespons;
        clientPrivateData m_myUseruser;


        private int m_localPort;
        private int m_partnerPort;
        private IPEndPoint ipPartnerEndPointSend;
        private IPEndPoint ipPartnerEndPointRcv;
        UdpClient udpClient;
        private byte[] m_sessionKey;
        private byte[] m_remoteKey;

        MemoryStream m_memStreamer;
        BinaryFormatter m_binryFormatter;

        ThreadStart threadDelegate;
        Thread newThread;

        public ChatWindow(userPortData userPort)
        {
            InitializeComponent();
            m_userPort = userPort;
            MainWin mainWindow = (MainWin)WindowsMgr.Instance.GetWindow(Constants.MAIN_WINDOW);
            mainWindow.setChatSessionKeyEvnt += (sender, sessionRespons) =>
            {
                Dispatcher.Invoke(() => setChatSessionKey(sessionRespons));
            };

            this.setShowChatWindowEvnt += (sender, sessionRespons) =>
            {
                Dispatcher.Invoke(() => showChatWindow(sessionRespons));
            };

            this.addLineToChatEvnt += (sender, sessionRespons) =>
            {
                Dispatcher.Invoke(() => addLineToChat(sessionRespons));
            };

            partnerNamelabel.Content = "PARTNER NAME:  " + userPort.userName;


            threadDelegate = new ThreadStart(this.startChatHandshake);
            newThread = new Thread(threadDelegate);
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();

        }

        public void setChatSessionKey(CSessionKeyResponse sessionRespons)
        {
            m_sessionRespons = sessionRespons;
        }

        public void startChatHandshake()
        {
            m_memStreamer = new MemoryStream();
            m_binryFormatter = new BinaryFormatter();
            clientPrivateData user = ClientAllData.Instance.getMyClient();
            m_myUseruser = ClientAllData.Instance.getMyClient();
            m_localPort = m_myUseruser.m_localPort;
            m_partnerPort = m_userPort.port;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            ipPartnerEndPointSend = new IPEndPoint(ipAddress, m_partnerPort);
            ipPartnerEndPointRcv = new IPEndPoint(ipAddress, m_partnerPort);

            udpClient = null;
            try
            {
                udpClient = new UdpClient(m_localPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if(m_userPort.isMaster)
            {
                masterHandshake();
            }
            else
            {
                remoteHandshake();
            }
        }

        private void masterHandshake()
        {
            while(m_sessionRespons == null)
            {
                Thread.Sleep(100);
            }

            m_sessionKey = CAes.SimpleDecrypt(m_sessionRespons.m_sessionKeyA, m_myUseruser.m_kdcAsSessionKey, m_myUseruser.m_kdcAsSessionKey);
            m_remoteKey = CAes.SimpleDecrypt(m_sessionRespons.m_sessionKeyB, m_myUseruser.m_kdcAsSessionKey, m_myUseruser.m_kdcAsSessionKey);

            bool sessionOk = false;
            try
            {

                udpClient.Send(m_remoteKey, m_remoteKey.Length, ipPartnerEndPointSend);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            byte[] readBuffer = null;
            try
            {
                readBuffer = udpClient.Receive(ref ipPartnerEndPointRcv);
                byte[] clientData = new byte[1024];
                MemoryStream readSteam = new MemoryStream(readBuffer);
                messageCreateChatRemoteToMaster retMsgFromMaster = (messageCreateChatRemoteToMaster)m_binryFormatter.Deserialize(readSteam);
                byte[] sessionKey = CAes.SimpleDecrypt(retMsgFromMaster.sessionKey, m_sessionKey, m_sessionKey);
                if (StructuralComparisons.StructuralEqualityComparer.Equals(sessionKey, m_sessionKey))
                {
                    string challenge = CAes.SimpleDecrypt(retMsgFromMaster.challenge, m_sessionKey, m_sessionKey);
                    string retChallengeEncryted = CAes.SimpleEncrypt(challenge + challenge, m_sessionKey, m_sessionKey);
                    byte[] encryptedSession = CAes.SimpleEncrypt(m_sessionKey, m_sessionKey, m_sessionKey);
                    messageCreateChatRemoteToMaster retMsgToMaster = new messageCreateChatRemoteToMaster();
                    retMsgToMaster.challenge = retChallengeEncryted;
                    retMsgToMaster.sessionKey = encryptedSession;
                    m_binryFormatter.Serialize(m_memStreamer, retMsgToMaster);
                    byte[] buffer = m_memStreamer.ToArray();
                    udpClient.Send(buffer, buffer.Length, ipPartnerEndPointSend);
                    sessionOk = true;
                }
                else
                {
                    //error
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            if (sessionOk)
            {
                byte[] readBufferConfirm = new byte[1024];
                readBufferConfirm = udpClient.Receive(ref ipPartnerEndPointRcv);

                byte[] readBufferConfirmBytes = CAes.SimpleDecrypt(readBufferConfirm, m_sessionKey, m_sessionKey);
                string retStringConfirm = Encoding.ASCII.GetString(readBufferConfirmBytes);
                if (retStringConfirm == "confirm")
                {
                    var evt = setShowChatWindowEvnt;
                    if (evt != null)
                    {
                        evt(this, "");
                    }
                    readChatMsg();
                }
                else;
                {
                    return;
                }





            }
        }

        private void remoteHandshake()
        {
            byte[] readBuffer = new byte[1024];
            bool sessionOk = false;
            try
            {
                readBuffer = udpClient.Receive(ref ipPartnerEndPointRcv);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            byte[] sessionKey = null;

            if (readBuffer != null)
            {
                
                sessionKey = CAes.SimpleDecrypt(readBuffer, m_myUseruser.m_kdcAsSessionKey, m_myUseruser.m_kdcAsSessionKey);
                m_sessionKey = sessionKey;
                string challenge = System.IO.Path.GetRandomFileName();
                messageCreateChatRemoteToMaster sendMsgFromMaster = new messageCreateChatRemoteToMaster(); ;

                sendMsgFromMaster.challenge = CAes.SimpleEncrypt(challenge, sessionKey, sessionKey);
                sendMsgFromMaster.sessionKey = CAes.SimpleEncrypt(sessionKey, sessionKey, sessionKey);
                m_binryFormatter.Serialize(m_memStreamer, sendMsgFromMaster);
                byte[] buffer = m_memStreamer.ToArray();

                udpClient.Send(buffer, buffer.Length, ipPartnerEndPointSend);

                byte[] readBuffer2 = new byte[1024];
                try
                {
                    readBuffer2 = udpClient.Receive(ref ipPartnerEndPointRcv);
                    byte[] clientData = new byte[1024];
                    MemoryStream readSteam = new MemoryStream(readBuffer2);
                    messageCreateChatRemoteToMaster retMsgFromRemote = (messageCreateChatRemoteToMaster)m_binryFormatter.Deserialize(readSteam);
                    byte[] sessionKey2 = CAes.SimpleDecrypt(retMsgFromRemote.sessionKey, sessionKey, sessionKey);
                    string challenge2 = CAes.SimpleDecrypt(retMsgFromRemote.challenge, sessionKey, sessionKey);
                    if (StructuralComparisons.StructuralEqualityComparer.Equals(sessionKey2, sessionKey) && challenge2 == challenge + challenge)
                    {
                        sessionOk = true;
                    }


                }
                catch (Exception e)
                {
                    //set session key
                    Console.WriteLine(e.ToString());
                }
            }

            if (sessionOk)
            {

                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("confirm chat with " + m_userPort.userName , "Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    string confirmStr = "confirm";
                    byte[] confirmStringBytes = Encoding.ASCII.GetBytes(confirmStr);
                    byte[] encryptedMsg = CAes.SimpleEncrypt(confirmStringBytes, m_sessionKey, m_sessionKey);
                    udpClient.Send(encryptedMsg, encryptedMsg.Length, ipPartnerEndPointSend);

                    var evt = setShowChatWindowEvnt;
                    if (evt != null)
                    {
                        evt(this, "");
                    }
                    readChatMsg();
                }
                else
                {
                    return;
                }



                
            }
        }

        public void readChatMsg()
        {
            while(true)
            {
                byte[] readBuffer = new byte[1024];
                readBuffer = udpClient.Receive(ref ipPartnerEndPointRcv);

                byte[] readStringBytes = CAes.SimpleDecrypt(readBuffer, m_sessionKey, m_sessionKey);
                string retString = Encoding.ASCII.GetString(readStringBytes);

                var evt = addLineToChatEvnt;
                if (evt != null)
                {
                    evt(this, retString);
                }
            }

        }

        public void showChatWindow(string str)
        {
            this.Show();
        }

        private void sendMessageBtn(object sender, RoutedEventArgs e)
        {
            string sendString = m_myUseruser.username + ": " + enterMessageTxtBox.Text + "\n";
            byte[] stringBytes = Encoding.ASCII.GetBytes(sendString);
            byte[] encryptedMsg = CAes.SimpleEncrypt(stringBytes, m_sessionKey, m_sessionKey);
            udpClient.Send(encryptedMsg, encryptedMsg.Length, ipPartnerEndPointSend);
            AllChatTxtBox.AppendText(sendString);

        }

        public void addLineToChat(string line)
        {
            AllChatTxtBox.AppendText(line);
        }
    }
}
