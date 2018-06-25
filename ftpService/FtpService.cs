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
using System.IO;

namespace ftpService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class FtpService : IFtpService
    {
        FtpDBservice m_FtpDBservice = new FtpDBservice();
        public static Dictionary<string, UserFtpServiceData> users_list = new Dictionary<string, UserFtpServiceData>(); // user, key_S

        public void requstForConnectionWithSessionKey(FtpTicketRequst ftpTicketRequst)
        {
            IClientFtpCallBack iClientFtpCallBack = OperationContext.Current.GetCallbackChannel<IClientFtpCallBack>();
            KdcFtpKey retKeyFromDB = m_FtpDBservice.getKdcFtpKey("KDC");

            if (retKeyFromDB == null) {
                iClientFtpCallBack.finishRequstConnectionProcess(null);
                return;
            }

            byte[] sessionKey = CAes.SimpleDecryptWithPassword(ftpTicketRequst.SessionKeyClientFTPEncryptedForFTP, retKeyFromDB.PassWord);
            string usenameDecryptedWithFtpKdcKey = CAes.SimpleDecryptWithPassword(ftpTicketRequst.UserNameencryptedForFtpWithFtpKey, retKeyFromDB.PassWord);
            string usenameDecryptedWithSessionKey = CAes.SimpleDecrypt(ftpTicketRequst.UserNameencryptedForFtpWithSessionKey, sessionKey, sessionKey);

            if (usenameDecryptedWithFtpKdcKey == usenameDecryptedWithSessionKey) // OK 
            {
                UserFtpServiceData userData = new UserFtpServiceData(sessionKey, iClientFtpCallBack);
                users_list.Add(usenameDecryptedWithFtpKdcKey, userData);

                string fileNameResult;
                string[] filesPaths = Directory.GetFiles("..\\..\\files\\", 
                                                    "*.*", SearchOption.AllDirectories);
                List<string> newFilesNames = new List<string>();

                // Display all the files.
                foreach (string path in filesPaths)
                {
                    fileNameResult = Path.GetFileName(path);
                    string fileNameResultString = CAes.SimpleEncrypt(fileNameResult, sessionKey, sessionKey);
                    newFilesNames.Add(fileNameResultString);
                }

                iClientFtpCallBack.finishRequstConnectionProcess(newFilesNames);
                return;
            }

            iClientFtpCallBack.finishRequstConnectionProcess(null);
        }

        public void requstForDownloadFile(string fileName, string clientName)
        {
            if (!users_list.ContainsKey(clientName)) { 
                return;
            }

            string decrryptedFileName = CAes.SimpleDecrypt(fileName, users_list[clientName].SessionKey, users_list[clientName].SessionKey);

            string[] filesPaths = Directory.GetFiles("..\\..\\files\\",
                                                    decrryptedFileName, SearchOption.AllDirectories);

            byte[] fileStream = File.ReadAllBytes(filesPaths[0]);

            byte[] fileStreamEncrypted = CAes.SimpleEncrypt(fileStream, users_list[clientName].SessionKey, users_list[clientName].SessionKey);

            users_list[clientName].clientFtpCallBack.finishRequstForDownloadFile(fileStreamEncrypted, fileName);
        }
    }
}
