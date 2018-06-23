using common;
using Contracts;
using Contracts.logicClasses;
using LinqToSql;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;


namespace KdcService
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class KdcService : IKdcService
    {
        public static Dictionary<string, UserServiceData> users_list = new Dictionary<string, UserServiceData>(); // user, key_S

        DBservice m_DBservice = new DBservice();
        FtpDBservice m_FtpDBservice = new FtpDBservice();

        KdcService()
        {
        }

        public CSessionKeyResponse GetSessionKeyForChatConnection(CSessionParams sessionParams)
        {
            // get users from the data base
            User retUser1FromDB = m_DBservice.getUserByName(sessionParams.client1UserName);
            User retUser2FromDB = m_DBservice.getUserByName(sessionParams.client2UserName);

            // check validity 
            if(retUser1FromDB == null || retUser2FromDB == null)
            {
                if(retUser1FromDB == null)
                {
                    Console.Write(sessionParams.client1UserName + "not exist in DB");
                }
                if (retUser2FromDB == null)
                {
                    Console.Write(sessionParams.client2UserName + "not exist in DB");
                }
                return null;
            }

            // genrate new session key
            byte[] sessiomKey = CAes.NewKey();

            //encrypt Eka [ Ks ||  || Kb[Ks] ]
            byte[] encryptedDataForClientB = CAes.SimpleEncryptWithPassword(sessiomKey, retUser2FromDB.PassWord); //Kb[Ks]
            byte[] keyA = CAes.SimpleEncryptWithPassword(sessiomKey, retUser1FromDB.PassWord); //Ka[Ks]
            byte[] keyAB = CAes.SimpleEncryptWithPassword(encryptedDataForClientB, retUser1FromDB.PassWord); //Ka[clientB data]

            // set return value
            CSessionKeyResponse retVal = new CSessionKeyResponse();
            retVal.m_sessionKeyA = keyA;
            retVal.m_sessionKeyB = keyAB;           
            return retVal;
        }

        public CKdcToClientLogInData LogInApp(string userName)
        {
            if (users_list.ContainsKey(userName))
            {
                Console.WriteLine("you already logged");
                return null;
            }

            Console.WriteLine("LOGIN: NAME {0} ", userName);

            CKdcToClientLogInData msgKdcToClientLoggin = new CKdcToClientLogInData();

            User retUserFromDB = m_DBservice.getUserByName(userName);
            if (retUserFromDB != null)
            {
                byte[] userSessionKey = CAes.NewKey();
                string challenge = Path.GetRandomFileName();
   

                msgKdcToClientLoggin.m_username = CAes.SimpleEncryptWithPassword(userName, retUserFromDB.PassWord);
                msgKdcToClientLoggin.m_kdcAsSessionKey = CAes.SimpleEncryptWithPassword(userSessionKey, retUserFromDB.PassWord);
                msgKdcToClientLoggin.m_challenge = CAes.SimpleEncryptWithPassword(challenge, retUserFromDB.PassWord);

                UserServiceData userServiceData = new UserServiceData(userSessionKey, OperationContext.Current.GetCallbackChannel<IClientKdcCallBack>());
                userServiceData.logginChallenge = challenge;
                users_list.Add(userName, userServiceData);
            }
            else
            {
                msgKdcToClientLoggin = null;
            }
            
            return msgKdcToClientLoggin;
        }

        public void sendNewUserToAllClients(string userName, int port)
        {
            List<string> nameList = new List<string>();
            foreach (KeyValuePair<string, UserServiceData> client in users_list)
            {
                nameList.Add(client.Key);
            }

            foreach (KeyValuePair<string, UserServiceData> client in users_list)
            {
                client.Value.clientKdcCallBack.addNewConnectedUser(userName, nameList, port);
            }
        }

        public List<string> getAllConnectedUsers()
        {
            List<string> nameList = new List<string>();
            foreach (KeyValuePair<string, UserServiceData> client in users_list)
            {
                nameList.Add(client.Key);
            }
            return nameList;
        }

        public void LogOutApp(string name)
        {
            if (users_list.ContainsKey(name))
                users_list.Remove(name);
        }

        public User RegisterApp(string userName, string Password)
        {


            return null;
        }

        public bool SetLoginStatus(CLogInStatus logginStatus)
        {
            bool retVal = false;
            UserServiceData userData = users_list[logginStatus.m_username];
            if(userData != null)
            {
                if(!logginStatus.m_logInFail)
                {
                    if (userData.logginChallenge == CAes.SimpleDecrypt(logginStatus.m_challenge, userData.SessionKey, userData.SessionKey))
                    {
                        retVal = true;
                        sendNewUserToAllClients(logginStatus.m_username, 1000);
                    }
                }
                if(!retVal)
                {
                    users_list.Remove(logginStatus.m_username);
                }
            }
            return retVal;
        }

        public FtpTicketResponse RequstSessionKeyForFtpConnection(FtpKeyRequst ftpKeyRequst)
        {
            if(!users_list.ContainsKey(ftpKeyRequst.UserName))
            {
                return null;
            }

            // get users from the data base
            KdcFtpKey retKeyFromDB = m_FtpDBservice.getKdcFtpKey("KDC");

            // check validity 
            if (retKeyFromDB == null)
            {
                Console.Write("no key exist in DB");
                return null;
            }

            // genrate new session key for CLIENT - FTP
            byte[] sessiomKey = CAes.NewKey();

            FtpTicketResponse ftpTicketResponse = new FtpTicketResponse();
            ftpTicketResponse.SessionKeyClientFTPEncryptedForFTP = CAes.SimpleEncryptWithPassword(sessiomKey, retKeyFromDB.PassWord);
            ftpTicketResponse.UserNameencryptedForFtpWithFtpKey = CAes.SimpleEncryptWithPassword(ftpKeyRequst.UserName, retKeyFromDB.PassWord); 
            ftpTicketResponse.SessionKeyClientFTPEncryptedForClient = CAes.SimpleEncrypt(sessiomKey, users_list[ftpKeyRequst.UserName].SessionKey, users_list[ftpKeyRequst.UserName].SessionKey);

            return ftpTicketResponse;
        }

        //public void sendMassage(int tableid, string massage)
        //{

        //    foreach (KeyValuePair<int, IClientCallBack> client in tables_lists[tableid].playing_list)
        //    {
        //        client.Value.updateChat(massage);
        //    }

        //    foreach (var watching in tables_lists[tableid].watching_list)
        //    {
        //        watching.updateChat(massage);
        //    }

        //}
    }
}
