using common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace client.resources
{
    class clientChat
    {
        private int m_localPort;
        private int m_partnerPort;
        private IPEndPoint ipPartnerEndPointSend;
        private IPEndPoint ipPartnerEndPointRcv;
        UdpClient udpClient;
        private byte[] m_sessionKey;
        private byte[] m_remoteKey;

        MemoryStream m_memStreamer;
        BinaryFormatter m_binryFormatter;

        public clientChat(int localPort, int partnerPort)
        {
            m_memStreamer = new MemoryStream();
            m_binryFormatter = new BinaryFormatter();
            m_localPort = localPort;
            m_partnerPort = partnerPort;
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
        }

        public void setSessionKey(byte[] localKey, byte[] remoteKey)
        {
            m_sessionKey = localKey;
            m_remoteKey = remoteKey;
        }

        public void startChatSessionMaster()
        {
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
                if(StructuralComparisons.StructuralEqualityComparer.Equals(sessionKey, m_sessionKey))
                {
                    string challenge = CAes.SimpleDecrypt(retMsgFromMaster.challenge, m_sessionKey, m_sessionKey);
                    string retChallengeEncryted = CAes.SimpleEncrypt(challenge, m_sessionKey, m_sessionKey);
                    byte[] encryptedSession = CAes.SimpleEncrypt(m_sessionKey, m_sessionKey, m_sessionKey);
                    messageCreateChatRemoteToMaster retMsgToMaster = new messageCreateChatRemoteToMaster();
                    retMsgToMaster.challenge = retChallengeEncryted;
                    retMsgToMaster.sessionKey = encryptedSession;
                    m_binryFormatter.Serialize(m_memStreamer, retMsgToMaster);
                    byte[] buffer = m_memStreamer.ToArray();
                    udpClient.Send(buffer, buffer.Length, ipPartnerEndPointSend);
                    sessionOk = true ;
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

            if(sessionOk)
            {
                Window chatwin = new ChatWindow();
                WindowsMgr.Instance.addWindow("ChatWindow", chatwin);
                chatwin.Show();
            }
        }

        public void startSessionChatRemote()
        {
            byte[] readBuffer = new byte[1024];
            bool sessionOk = false ;
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
                clientPrivateData userData = ClientAllData.Instance.getMyClient();
                sessionKey = CAes.SimpleDecrypt(readBuffer, userData.m_kdcAsSessionKey, userData.m_kdcAsSessionKey);

                string challenge = Path.GetRandomFileName();
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
                    if (StructuralComparisons.StructuralEqualityComparer.Equals(sessionKey2, sessionKey) && challenge2 == challenge)
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

            if(sessionOk)
            {
                m_sessionKey = sessionKey;
                Window chatwin = new ChatWindow();
                WindowsMgr.Instance.addWindow("ChatWindow", chatwin);
                chatwin.Show();
                udpClient.Receive(ref ipPartnerEndPointRcv);

            }
        }
    }
}
