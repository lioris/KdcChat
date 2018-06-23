using LinqToSql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using common;
using Contracts.logicClasses;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IClientKdcCallBack))]
    public interface IKdcService
    {
        [OperationContract]
        User RegisterApp(string userName, string Password);

        [OperationContract]
        CKdcToClientLogInData LogInApp(string userName);

        [OperationContract(IsOneWay=true)]
        void LogOutApp(string name);

        [OperationContract]
        CSessionKeyResponse GetChatSessionParams(CSessionParams sessionParams);

        [OperationContract]
        List<string> getAllConnectedUsers();

        [OperationContract]
        bool SetLoginStatus(CLogInStatus logginStatus);


        //[OperationContract(IsOneWay = true)]
        //void sendMassage(int tableid, string massage);

    }
}
