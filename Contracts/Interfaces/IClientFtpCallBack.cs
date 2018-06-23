using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface IClientFtpCallBack
    {
        [OperationContract(IsOneWay = true)]
        void finishRequstConnectionProcess(bool finishStatus); // should be encrypted - i smimplified it

    }
}
