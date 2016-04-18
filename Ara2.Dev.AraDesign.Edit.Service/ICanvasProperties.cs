using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Dev.AraDesign.Edit.Service
{
    public interface ICanvasProperties
    {
        string NameSpace { get; set; }
        string ClassName { get; set; }
        string NameSpaceAraDesign { get; set; }
        string ClassNameAraDesign { get; set; }
        Type TypeCanas { get; set; }
    }

    public class CanvasProperties : ICanvasProperties
    {
        public string NameSpace { get; set; }
        public string ClassName { get; set; }
        public string NameSpaceAraDesign { get; set; }
        public string ClassNameAraDesign { get; set; }
        public Type TypeCanas { get; set; }
    }
}
