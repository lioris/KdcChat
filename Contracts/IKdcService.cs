using LinqToSql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using common;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IClientCallBack))]
    public interface IKdcService
    {
        [OperationContract]
        User RegisterApp(string userName, string Password);

        [OperationContract]
        User LogInApp(string userName);

        [OperationContract(IsOneWay=true)]
        void LogOutApp(string name);

        [OperationContract]
        CSessionKeyResponse GetSessionKey(CSessionParams sessionParams);

        
        //[OperationContract(IsOneWay = true)]
        //void sendMassage(int tableid, string massage);

    }
}
