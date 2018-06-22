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
        private byte[] m_sessionKye;
        clientChat myClientChat;

        //ThreadStart threadDelegate;
        //Thread newThread;

        public session(int localPort, int partnerPort)
        {
            /*myClientChat = new clientChat(localPort, partnerPort);

            threadDelegate = new ThreadStart(myClientChat.sendMessage);
            newThread = new Thread(threadDelegate);
            newThread.Start();
      
            threadDelegate = new ThreadStart(myClientChat.readMessage);
            newThread = new Thread(threadDelegate);
            newThread.Start();*/


        }

        public void setSessionKey(byte[] sessionKey)
        {
            m_sessionKye = sessionKey;
        }
    }
}
