using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IClientFtpCallBack))]
    public interface IFtpService
    {
        [OperationContract]
        void printKoko();

       

    }
}
