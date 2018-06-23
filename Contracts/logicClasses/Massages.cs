using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.logicClasses
{

    [DataContract]
    public class FtpKeyRequst // client send to the kdc 
    {
        [DataMember]
        string m_userName;

    public FtpKeyRequst(string user)
        {
            m_userName = user;
        }

        public string UserName 
        {
            get { return m_userName; }
            set { m_userName = value; }
        }
    }

    [DataContract]
    public class FtpTicketResponse // kdc send to the client after key requst
    {
        //ticket for ftp dervice
        // key_ftp [key_client_ftp]
        [DataMember]
        byte[] m_sessionKeyClientFTPEncryptedForFTP;
        [DataMember]
        string m_userNameencryptedForFtpWithFtpKey;
        [DataMember]
        byte[] m_sessionKeyClientFTPEncryptedForClient;

        public byte[] SessionKeyClientFTPEncryptedForFTP
        {
            get { return m_sessionKeyClientFTPEncryptedForFTP; }
            set { m_sessionKeyClientFTPEncryptedForFTP = value; }
        }

        public string UserNameencryptedForFtpWithFtpKey
        {
            get { return m_userNameencryptedForFtpWithFtpKey; }
            set { m_userNameencryptedForFtpWithFtpKey = value; }
        }

        public byte[] SessionKeyClientFTPEncryptedForClient
        {
            get { return m_sessionKeyClientFTPEncryptedForClient; }
            set { m_sessionKeyClientFTPEncryptedForClient = value; }
        }

    }

    [DataContract]
    public class FtpTicketRequst // client send to the ftp 
    {
        [DataMember]
        byte[] m_sessionKeyClientFTPEncryptedForFTP;
        [DataMember]
        string m_userNameencryptedForFtpWithFtpKey;

        [DataMember]
        string m_userNameencryptedForFtpWithSessionKey;

        public byte[] SessionKeyClientFTPEncryptedForFTP
        {
            get { return m_sessionKeyClientFTPEncryptedForFTP; }
            set { m_sessionKeyClientFTPEncryptedForFTP = value; }
        }

        public string UserNameencryptedForFtpWithFtpKey
        {
            get { return m_userNameencryptedForFtpWithFtpKey; }
            set { m_userNameencryptedForFtpWithFtpKey = value; }
        }

        public string UserNameencryptedForFtpWithSessionKey
        {
            get { return m_userNameencryptedForFtpWithSessionKey; }
            set { m_userNameencryptedForFtpWithSessionKey = value; }
        }


    }
}
