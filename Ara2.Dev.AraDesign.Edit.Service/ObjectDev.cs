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
    [Serializable]
    public class ObjectDev
    {
        public object Object { get; set; }
        public ObjectDevPropertys[] Propertys { get; set; }
        public ObjectDev(object vObject ,ObjectDevPropertys[] P)
        {
            Object = vObject;
            Propertys = P;
        }
    }   
    

}
