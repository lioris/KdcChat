using common;
using Contracts;
using LinqToSql;
using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace KdcService
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class KdcService : IKdcService
    {
        public static Dictionary<string, UserServiceData> users_list = new Dictionary<string, UserServiceData>(); // user, key_S

        DBservice m_DBservice = new DBservice();

        KdcService()
        {
        }

        public CSessionKeyResponse GetSessionKey(CSessionParams sessionParams)
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

        public User LogInApp(string userName)
        {
            if (users_list.ContainsKey(userName))
            {
                Console.WriteLine("you already logged");
                return null;
            }

            Console.WriteLine("LOGIN: NAME {0} ", userName);

            User msgKdcToClientLoggin = null;

            //msgKdcToClientLoggin.Name = common.CAes.SimpleEncryptWithPassword(userName, password1);
            //msgKdcToClientLoggin.PassWord = common.CAes.SimpleEncryptWithPassword(password1, password1);

            User retUserFromDB = m_DBservice.getUserByName(userName);
            if (retUserFromDB != null)
            {
                msgKdcToClientLoggin = new User();
                byte[] userSessionKey = common.CAes.NewKey();
                msgKdcToClientLoggin.Name = common.CAes.SimpleEncryptWithPassword(userName, retUserFromDB.PassWord);
                msgKdcToClientLoggin.PassWord = common.CAes.SimpleEncryptWithPassword(retUserFromDB.PassWord, retUserFromDB.PassWord);

                UserServiceData userServiceData = new UserServiceData(userSessionKey, OperationContext.Current.GetCallbackChannel<IClientKdcCallBack>());
                
                users_list.Add(userName, userServiceData);
                sendNewUserToAllClients(userName, 1000);
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
