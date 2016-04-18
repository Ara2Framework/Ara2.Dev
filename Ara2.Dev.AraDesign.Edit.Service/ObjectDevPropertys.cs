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
    public class ObjectDevPropertys
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Type Type { get; set; }
        public Attribute[] Attribute { get; set; }
    }

}
