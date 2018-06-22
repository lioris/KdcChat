﻿using common;
using Contracts;
using LinqToSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace KdcService
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class KdcService : IKdcService
    {
        public static Dictionary<string, IClientKdcCallBack> user_list = new Dictionary<string, IClientKdcCallBack>();
        DBservice m_DBservice = new DBservice();

        KdcService()
        {
        }

        public CSessionKeyResponse GetSessionKey(CSessionParams sessionParams)
        {

            User retUser1FromDB = m_DBservice.getUserByName(sessionParams.client1UserName);
            User retUser2FromDB = m_DBservice.getUserByName(sessionParams.client2UserName);

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

            byte[] key = common.CAes.NewKey();

            byte[] keyB = common.CAes.SimpleEncryptWithPassword(key, retUser2FromDB.PassWord);
            byte[] keyA = common.CAes.SimpleEncryptWithPassword(key, retUser1FromDB.PassWord);
            byte[] keyAB = common.CAes.SimpleEncryptWithPassword(keyB, retUser1FromDB.PassWord);

            CSessionKeyResponse retVal = new CSessionKeyResponse();
            retVal.m_sessionKeyA = keyA;
            retVal.m_sessionKeyB = keyAB;
            return retVal;
        }

        public User LogInApp(string userName)
        {
            if (user_list.ContainsKey(userName))
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
                byte[] key = common.CAes.NewKey();
                string keyString = System.Text.Encoding.UTF8.GetString(key);
                msgKdcToClientLoggin = new User();

                msgKdcToClientLoggin.Name = common.CAes.SimpleEncryptWithPassword(userName, retUserFromDB.PassWord);
                msgKdcToClientLoggin.PassWord = common.CAes.SimpleEncryptWithPassword(keyString, retUserFromDB.PassWord);
                user_list.Add(userName, OperationContext.Current.GetCallbackChannel<IClientKdcCallBack>());
                sendNewUserToAllClients(userName, 1000);
            }
            
            return msgKdcToClientLoggin;
        }

        public void sendNewUserToAllClients(string userName, int port)
        {
            List<string> nameList = new List<string>();
            foreach (KeyValuePair<string, IClientKdcCallBack> client in user_list)
            {
                nameList.Add(client.Key);
            }

            foreach (KeyValuePair<string, IClientKdcCallBack> client in user_list)
            {
                client.Value.addNewConnectedUser(userName, nameList, port);
            }
        }

        public List<string> getAllConnectedUsers()
        {
            List<string> nameList = new List<string>();
            foreach (KeyValuePair<string, IClientKdcCallBack> client in user_list)
            {
                nameList.Add(client.Key);
            }
            return nameList;
        }

        public void LogOutApp(string name)
        {
            if (user_list.ContainsKey(name))
                user_list.Remove(name);
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
