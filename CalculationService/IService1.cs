using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace interfaceMethods
{

    [ServiceContract]
    public interface IService1
    {
        //interface connection to flightCost calculation
        [OperationContract]
        [WebInvoke]
        double flightCost(string json);

        //interface connection to hotelCost calculation
        [OperationContract]
        [WebInvoke]
        double hotelCost(string json);

        [OperationContract]
        [WebInvoke]
        double carCost(string json);

        //interface connection to totalCost calculation
        [OperationContract]
        [WebInvoke]
        double totalCost(string json);


    }

}
