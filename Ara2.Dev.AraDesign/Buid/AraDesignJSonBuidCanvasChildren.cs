using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Dev.AraDesign
{
    [Serializable]
    public class AraDesignJSonBuidCanvasChildren : AraDesignJSonCanvasChildren , IAraDesignJSonBuidCanvasChildren
    {
        public AraDesignJSonBuidCanvasChildren(AraDesignJSonBuid AraDesignJSonBuid, IAraDesignJSonFather vFather, dynamic vChildren)
            :base(vFather,(object)vChildren)
        {

            Propertys = AraDesignJSonBuid.GetListPropertys(this, vChildren.Propertys);
            Children = AraDesignJSonBuid.GetListChildren(this, vChildren.Children);

            Name = Propertys.Where(a => a.Name == "Name").FirstOrDefault().Value;

        }
        
        public string Name { get; set; }
        

        public List<IAraDesignJSonBuidCanvasPropertys> Propertys { get; set; }
        public List<IAraDesignJSonBuidCanvasChildren> Children { get; set; }

        public string GetTypeToString(AraDesignProjectReferences ProjectReferences)
        {
            
            return this.TypeName;
        }
    }

    public interface IAraDesignJSonBuidCanvasChildren : IAraDesignJSonCanvasChildren
    {
        string Name { get; set; }
        Type GetTypeObject(AraDesignProjectReferences ProjectReferences);
        List<IAraDesignJSonBuidCanvasPropertys> Propertys { get; set; }
        List<IAraDesignJSonBuidCanvasChildren> Children { get; set; }

        string GetTypeToString(AraDesignProjectReferences ProjectReferences);
    }
}
