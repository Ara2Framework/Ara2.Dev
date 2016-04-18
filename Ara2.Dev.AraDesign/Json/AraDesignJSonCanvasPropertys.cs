using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Dev.AraDesign
{
    [Serializable]
    public class AraDesignJSonCanvasPropertys : IAraDesignJSonCanvasPropertys
    {
        public AraDesignJSonCanvasPropertys()
        {

        }

        public AraDesignJSonCanvasPropertys(IAraDesignJSonFather vFather, dynamic vProperty)
        {
            _Father = vFather;
            Name = vProperty.Name;
            ValueType = vProperty.ValueType;
            Value = vProperty.Value; ValueDefault = vProperty.ValueDefault;
            CustomEdition = Convert.ToBoolean(vProperty.CustomEdition);
        }

        public string Name { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }
        public string ValueDefault { get; set; }
        public bool CustomEdition { get; set; }

        private IAraDesignJSonFather _Father;
        public IAraDesignJSonFather Father()
        {
            return _Father;
        }
    }

    public interface IAraDesignJSonCanvasPropertys
    {
        string Name { get; set; }
        string ValueType { get; set; }
        string Value { get; set; }
        string ValueDefault { get; set; }
        bool CustomEdition { get; set; }

        IAraDesignJSonFather Father();
    }
}
