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
    public interface IPakServerAraDevEdit
    {
        [OperationContract]
        bool GetPendingToSave();

        [OperationContract]
        void Save();

        [OperationContract]
        void SaveAs(string vFile);

        [OperationContract]
        void Undo();

        [OperationContract]
        bool CatUndo();


        [OperationContract]
        void Redo();

        [OperationContract]
        bool CatRedo();

        [OperationContract]
        byte[] GetOjectsDevAll();

        [OperationContract]
        void SetValue(int ID,string Name, string Property, byte[] vValueB, bool IsNull);

        [OperationContract]
        byte[] GetObjectByIdInstance(string IdInstance);

        [OperationContract]
        void AddObject(string vNameType);

        [OperationContract]
        void SelectEditObject(string[] vNames);

        [OperationContract]
        void SelectAll();

        [OperationContract]
        bool GetQueryCopy();

        [OperationContract]
        bool GetQueryCutOrDelete();

        [OperationContract]
        bool GetQueryPaste(int vFormatId, string  vFormatName);

        [OperationContract]
        byte[] GetObjectBySendoEditado();

        [OperationContract]
        void Paste(byte[] vObjs);

        [OperationContract]
        void Delete();

        [OperationContract]
        string GetTypeCananvas();

        [OperationContract]
        void SetTypeCananvas(string vTypeName);

        [OperationContract]
        string GetNameSpace();

        [OperationContract]
        void SetNameSpace(string vTypeName);

        [OperationContract]
        string GetClassName();

        [OperationContract]
        void SetClassName(string vTypeName);

        [OperationContract]
        string GetNameSpaceAraDesign();

        [OperationContract]
        void SetNameSpaceAraDesign(string vTypeName);

        [OperationContract]
        string GetClassNameAraDesign();

        [OperationContract]
        void SetClassNameAraDesign(string vTypeName);

    }
    

}
