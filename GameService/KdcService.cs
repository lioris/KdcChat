using common;
using Contracts;
using LinqToSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KdcService
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class KdcService : IKdcService
    {
        public static Dictionary<string, IClientCallBack> user_list = new Dictionary<string, IClientCallBack>();
        //DBservice m_DBservice;

        KdcService()
        {
        }

        public User LogInApp(string userName)
        {
            if (user_list.ContainsKey(userName))
            {
                Console.WriteLine("you already logged");
                return null;
            }

            Console.WriteLine("LOGIN: NAME {0} ", userName);

            String password1 = "12345678912345";
            //String password2 = "12345678912345";
            User msgKdcToClientLoggin = new User();

            msgKdcToClientLoggin.Name = common.CAes.SimpleEncryptWithPassword(userName, password1);
            msgKdcToClientLoggin.PassWord = common.CAes.SimpleEncryptWithPassword(password1, password1);

            return msgKdcToClientLoggin;

            //IDbService DBproxy = DBchannel.CreateChannel();
            //User user = DBproxy.login(userName, Password);

            //if (user != null)
            //{
            //    Console.WriteLine("ADD TO USER LIST");
            //    user_list.Add(userName, OperationContext.Current.GetCallbackChannel<IClientCallBack>());
            //    return user;
            //}
            //else
            //    return null;


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
