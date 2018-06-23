using Contracts.logicClasses;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IClientFtpCallBack))]
    public interface IFtpService
    {
        [OperationContract]
        void printKoko();

        [OperationContract]
        bool requstForConnectionWithSessionKey(FtpTicketRequst ftpTicketRequst); // blocking



    }
}
