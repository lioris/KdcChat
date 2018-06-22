using System.ServiceModel;

namespace Contracts
{
    //[ServiceContract(CallbackContract = typeof(IClientCallBack))]
    [ServiceContract]
    public interface IFtpService
    {
        [OperationContract]
        void printKoko();

       

    }
}
