using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using System.ServiceModel.Channels;
using Ara2.Dev;
using System.Reflection;

namespace Ara2.Dev.AraDesign.Edit.Service
{
    [ServiceContract] 
    public interface IPakVisualStudio
    {
        [OperationContract]
        void ChangePendingToSave();

        [OperationContract]
        void ChangeCatUndo();

        [OperationContract]
        void ChangeCatRedo();

        [OperationContract]
        void ChangeObjectsList();

        [OperationContract]
        void SetObjectsEdit(string[] Names);

        [OperationContract]
        void FinallySetValue(int ID, Exception Names);
    }

}
