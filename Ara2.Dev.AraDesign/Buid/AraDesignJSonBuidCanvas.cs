using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Dev.AraDesign
{
    [Serializable]
    public class AraDesignJSonBuidCanvas : AraDesignJSonCanvas , IAraDesignJSonBuidCanvas
    {
        public AraDesignJSonBuidCanvas(AraDesignJSonBuid AraDesignJSonBuid, dynamic vCanvas, IAraDesignJSonProjeto Projeto)
            :base((object)vCanvas, Projeto)
        {
            _AraDesignJSonBuid = AraDesignJSonBuid;

            Name = null;
            Propertys = _AraDesignJSonBuid.GetListPropertys(this, vCanvas.Propertys);
            Children = _AraDesignJSonBuid.GetListChildren(this, vCanvas.Children);
        }

        public string Name { get; set; }
        public List<IAraDesignJSonBuidCanvasPropertys> Propertys { get; set; }
        public List<IAraDesignJSonBuidCanvasChildren> Children { get; set; }

        AraDesignJSonBuid _AraDesignJSonBuid;
        public AraDesignJSonBuid AraDesignJSonBuid
        {
            get
            {
                return _AraDesignJSonBuid;
            }
        }
    }

    public interface IAraDesignJSonBuidCanvas : IAraDesignJSonCanvas
    {
        List<IAraDesignJSonBuidCanvasPropertys> Propertys { get; set; }
        List<IAraDesignJSonBuidCanvasChildren> Children { get; set; }
    }
}
