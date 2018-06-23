using LinqToSql;
using System.Collections.Generic;
using System.ServiceModel;
using Contracts.logicClasses;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IClientKdcCallBack))]
    public interface IKdcService
    {
        //GLOBAL OPERTIONS
        [OperationContract]
        User RegisterApp(string userName, string Password);

        [OperationContract]
        CKdcToClientLogInData LogInApp(string userName);

        [OperationContract(IsOneWay=true)]
        void LogOutApp(string name);


        // CHAT OPERTIONS
        [OperationContract]
        CSessionKeyResponse GetSessionKeyForChatConnection(CSessionParams sessionParams);

        [OperationContract]
        List<string> getAllConnectedUsers();

        [OperationContract]
        bool SetLoginStatus(CLogInStatus logginStatus);


        //FTP OPERTIONS     
        [OperationContract]
        FtpTicketResponse RequstSessionKeyForFtpConnection(FtpKeyRequst ftpKeyRequst);

        //[OperationContract(IsOneWay = true)]
        //void sendMassage(int tableid, string massage);

    }
}
