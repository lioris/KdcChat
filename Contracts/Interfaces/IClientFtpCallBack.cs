using System.Collections.Generic;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface IClientFtpCallBack
    {
        [OperationContract(IsOneWay = true)]
        void finishRequstConnectionProcess(List<string> finishStatus);

        [OperationContract(IsOneWay = true)]
        void finishRequstForDownloadFile(byte[] fileStream, string fileNameEncrypted);

    }
}
