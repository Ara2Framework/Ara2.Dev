using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Ara2.Dev.AraDesign
{
    [Serializable]
    public class AraDesignJSonCanvasChildren : IAraDesignJSonCanvasChildren
    {
        public AraDesignJSonCanvasChildren(string vTypeName, string vTypeNameInternal)
        {
            _Father = null;
            TypeName = vTypeName;
            TypeNameInternal = vTypeNameInternal;
            Propertys = new List<IAraDesignJSonCanvasPropertys>();
            _Children = new List<IAraDesignJSonCanvasChildren>();
        }

        public AraDesignJSonCanvasChildren(IAraDesignJSonFather vFather, dynamic Children)
        {
            Name = null;
            _Father = vFather;
            TypeName = Children.TypeName;
            TypeNameInternal = Children.TypeNameInternal;
            Propertys = AraDesignJSon.GetListPropertys(this,Children.Propertys);
            _Children = AraDesignJSon.GetListChildren(this,Children.Children);
        }


        public string Name { get; set; }
        public string TypeName { get; set; }
        public string TypeNameInternal { get; set; }
        public IEnumerable<IAraDesignJSonCanvasPropertys> Propertys { get; set; }

        IEnumerable<IAraDesignJSonCanvasChildren> _Children;
        public IEnumerable<IAraDesignJSonCanvasChildren> Children
        {
            get { return _Children; }
            set { 
                _Children = value; 
            }
        }

        private IAraDesignJSonFather _Father;
        public IAraDesignJSonFather Father()
        {
            return _Father;
        }

        public Type GetTypeObject(AraDesignProjectReferences ProjectReferences)
        {
            return ProjectReferences.Components.Where(a => a.Name == this.TypeName || a.FullName == this.TypeName).FirstOrDefault();
        }
    }

    public interface IAraDesignJSonCanvasChildren : IAraDesignJSonFather
    {
        string TypeName { get; set; }
        string TypeNameInternal { get; set; }
        IEnumerable<IAraDesignJSonCanvasPropertys> Propertys { get; set; }
        IEnumerable<IAraDesignJSonCanvasChildren> Children { get; set; }
    }
}
