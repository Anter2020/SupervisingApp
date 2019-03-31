using NajmDefault.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NajmDefault
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMsgWcfService" in both code and config file together.
    [ServiceContract]
    public interface IMsgWcfService
    {
        [OperationContract]
        string SetLevel(string message);

        [OperationContract]
        string UpdateSignalForce(string state);

        [OperationContract]
        SeqModel chkSeq(string stationID);

        [OperationContract]
        string ConfirmSeq(string seqID, string state);

        [OperationContract]
        string SetAlarm(int stationID, string message);

        [OperationContract]
        List<SeqModel> chkAllSeq();
    }
}
