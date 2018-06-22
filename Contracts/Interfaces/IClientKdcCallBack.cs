using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IClientKdcCallBack
    {

        [OperationContract(IsOneWay = true)]
        void printHello(string massage);

        [OperationContract(IsOneWay = true)]
        void addNewConnectedUser(string username, List<string> allUsers, int port);

        [OperationContract(IsOneWay = true)]
        void removeDisconnectedUser(string massage);
    }
}
