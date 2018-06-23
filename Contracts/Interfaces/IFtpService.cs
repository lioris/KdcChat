using Contracts.logicClasses;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IClientFtpCallBack))]
    public interface IFtpService
    {
        [OperationContract]
        void printKoko();

        [OperationContract(IsOneWay = true)]
        void requstForConnectionWithSessionKey(FtpTicketRequst ftpTicketRequst); // blocking



    }
}
