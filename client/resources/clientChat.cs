using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client.resources
{
    class clientChat
    {
        private int m_localPort;
        private int m_partnerPort;
        public clientChat(int localPort, int partnerPort)
        {
            m_localPort = localPort;
            m_partnerPort = partnerPort;
        }

        public void sendMessage()
        {
            while (true)
            {
                UdpClient udpClient = new UdpClient();
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, m_partnerPort);

                Byte[] sendBytes = Encoding.ASCII.GetBytes(Console.ReadLine());
                try
                {
                    udpClient.Send(sendBytes, sendBytes.Length, ipEndPoint);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

        }

        public void readMessage()
        {
            UdpClient client = null;
            try
            {
                client = new UdpClient(m_localPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            IPEndPoint server = new IPEndPoint(IPAddress.Broadcast, m_partnerPort);

            Console.WriteLine("Trying to receive...");

            while (true)
            {
                try
                {
                    byte[] packet = client.Receive(ref server);
                    Console.WriteLine(Encoding.ASCII.GetString(packet));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

        }

    }
}
