using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace client.resources
{
    public class session
    {
        public byte[] m_sessionKey;
        public byte[] m_remoteKey;
        clientChat myClientChat;

        //ThreadStart threadDelegate;
        ThreadStart threadDelegate;
        Thread newThread;

        public session(int localPort, int partnerPort)
        {

            myClientChat = new clientChat(localPort, partnerPort);
        }

        public void startReceving()
        {

            threadDelegate = new ThreadStart(myClientChat.startSessionChatRemote);
            Thread newThread = new Thread(threadDelegate);
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }


        public void startSending()
        {

            threadDelegate = new ThreadStart(myClientChat.startChatSessionMaster);
            newThread = new Thread( threadDelegate);
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }

        public void setSessionKey(byte[] localKey, byte[] remoteKey)
        {
            m_sessionKey = localKey;
            m_remoteKey = remoteKey;

            myClientChat.setSessionKey(localKey, remoteKey);


        }
    }
}
