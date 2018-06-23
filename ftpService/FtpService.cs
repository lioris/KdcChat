using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Contracts.logicClasses;
using LinqToSql;
using KdcService;
using common;

namespace ftpService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class FtpService : IFtpService
    {
        FtpDBservice m_FtpDBservice = new FtpDBservice();

        public void printKoko()
        {
            Console.WriteLine("lior is awsom");
        }

        public void requstForConnectionWithSessionKey(FtpTicketRequst ftpTicketRequst)
        {
            IClientFtpCallBack iClientFtpCallBack = OperationContext.Current.GetCallbackChannel<IClientFtpCallBack>();
            KdcFtpKey retKeyFromDB = m_FtpDBservice.getKdcFtpKey("KDC");

            if (retKeyFromDB == null) {
                iClientFtpCallBack.finishRequstConnectionProcess(false);
            }

            byte[] sessionKey = CAes.SimpleDecryptWithPassword(ftpTicketRequst.SessionKeyClientFTPEncryptedForFTP, retKeyFromDB.PassWord);
            string usenameDecryptedWithFtpKdcKey = CAes.SimpleDecryptWithPassword(ftpTicketRequst.UserNameencryptedForFtpWithFtpKey, retKeyFromDB.PassWord);
            string usenameDecryptedWithSessionKey = CAes.SimpleDecrypt(ftpTicketRequst.UserNameencryptedForFtpWithSessionKey, sessionKey, sessionKey);

            if (usenameDecryptedWithFtpKdcKey == usenameDecryptedWithSessionKey) // OK 
            {
                iClientFtpCallBack.finishRequstConnectionProcess(true);
                return;
            }

            iClientFtpCallBack.finishRequstConnectionProcess(false);
        }
    }
}
