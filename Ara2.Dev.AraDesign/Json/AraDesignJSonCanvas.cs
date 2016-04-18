using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Ara2.Dev.AraDesign
{
    [Serializable]
    public class AraDesignJSonCanvas : IAraDesignJSonCanvas
    {
        public AraDesignJSonCanvas(
                string vTypeName, 
                string vName, 
                string vNameSpace, 
                string vClassName, 
                string vNameSpaceAraDesign, 
                string vClassNameAraDesign
            )
        {
            _Father = null;
            Name = null;
            TypeName = vName;
            TypeAssembly = vTypeName;
            Propertys = new List<IAraDesignJSonCanvasPropertys>();
            Children = new List<IAraDesignJSonCanvasChildren>();

            NameSpace = vNameSpace;
            ClassName = vClassName;
            NameSpaceAraDesign = vNameSpaceAraDesign;
            ClassNameAraDesign = vClassNameAraDesign;
        }

        public AraDesignJSonCanvas(dynamic vCanvas, IAraDesignJSonProjeto Projeto)
        {
            _Father = null;
            Name = null;
            TypeName = vCanvas.TypeName;
            TypeAssembly = vCanvas.TypeAssembly;
            Propertys = AraDesignJSon.GetListPropertys(this, vCanvas.Propertys);
            Children = AraDesignJSon.GetListChildren(this, vCanvas.Children);

            try
            {
                NameSpace = vCanvas.NameSpace;
                if (string.IsNullOrEmpty(NameSpace))
                    throw new NullReferenceException();
            }
            catch
            {
                NameSpace = "Canvas.NameSpace";
            }

            try
            {
                ClassName = vCanvas.ClassName;
                if (string.IsNullOrEmpty(ClassName))
                    throw new NullReferenceException();
            }
            catch
            {
                ClassName = Projeto.Name;
            }
            try
            {
                NameSpaceAraDesign = vCanvas.NameSpaceAraDesign;
                if (string.IsNullOrEmpty(NameSpaceAraDesign))
                    throw new NullReferenceException();
            }
            catch
            {
                NameSpaceAraDesign = "AraDesign";
            }
            try
            {
                ClassNameAraDesign = vCanvas.ClassNameAraDesign;
                if (string.IsNullOrEmpty(ClassNameAraDesign))
                    throw new NullReferenceException();
            }
            catch
            {
                ClassNameAraDesign = ClassName + "AraDesign";
            }
        }

        public string Name { get; set; }

        public string TypeName { get; set; }
        public string TypeAssembly { get; set; }
        public IEnumerable<IAraDesignJSonCanvasPropertys> Propertys { get; set; }
        public IEnumerable<IAraDesignJSonCanvasChildren> Children { get; set; }

        public string NameSpace { get; set; }
        public string ClassName { get; set; }
        public string NameSpaceAraDesign { get; set; }
        public string ClassNameAraDesign { get; set; }

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

    public interface IAraDesignJSonCanvas : IAraDesignJSonFather
    {
        string TypeName { get; set; }
        string TypeAssembly { get; set; }
        IEnumerable<IAraDesignJSonCanvasPropertys> Propertys { get; set; }
        IEnumerable<IAraDesignJSonCanvasChildren> Children { get; set; }

        string NameSpace { get; set; }
        string ClassName { get; set; }
        string NameSpaceAraDesign { get; set; }
        string ClassNameAraDesign { get; set; }
    }

    public interface IAraDesignJSonFather
    {
        string Name { get; set; }
        IAraDesignJSonFather Father();
        Type GetTypeObject(AraDesignProjectReferences ProjectReferences);
    }
}
