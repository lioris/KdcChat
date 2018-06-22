using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IClientFtpCallBack
    {
        [OperationContract(IsOneWay = true)]
        void printHelloFtp(string massage);
    }
}
