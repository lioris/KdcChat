using Contracts.logicClasses;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IClientFtpCallBack))]
    public interface IFtpService
    {

        [OperationContract(IsOneWay = true)]
        void requstForConnectionWithSessionKey(FtpTicketRequst ftpTicketRequst);

        [OperationContract(IsOneWay = true)]
        void requstForDownloadFile(string fileName, string clientName);



    }
}
