using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Dev.AraDesign.Tools;

namespace Ara2.Dev.AraDesign
{
    [Serializable]
    public class AraDesignJSon : IAraDesignJSon
    {
        public IAraDesignJSonProjeto Projeto { get; set; }
        public IAraDesignJSonCanvas Canvas { get; set; }

        public dynamic JSon;

        public AraDesignJSon()
        {

        }

        public AraDesignJSon(string JsonScript)
        {
            
            try
            {
                JSon = Json.DynamicJson.Parse(JsonScript);
            }
            catch (Exception err)
            {
                throw new Exception("Json malformed\n" + err.Message);
            }

            try
            {
                Projeto = new AraDesignJSonProjeto(JSon.Projeto);

                Canvas = new AraDesignJSonCanvas(JSon.Canvas, Projeto);
            }
            catch (Exception err)
            {
                throw new Exception("Error load json.\n" + err.Message);
            }
        }

        public static IEnumerable<IAraDesignJSonCanvasPropertys> GetListPropertys(IAraDesignJSonFather vFather, dynamic vTmp)
        {
            List<IAraDesignJSonCanvasPropertys> vTmpP = new List<IAraDesignJSonCanvasPropertys>();
            foreach (dynamic vProperty in vTmp)
            {
                vTmpP.Add(new AraDesignJSonCanvasPropertys(vFather,vProperty));
            }

            return vTmpP;
        }

        public static IEnumerable<IAraDesignJSonCanvasChildren> GetListChildren(IAraDesignJSonFather vFather, dynamic vTmp)
        {
            List<IAraDesignJSonCanvasChildren> vTmpC = new List<IAraDesignJSonCanvasChildren>();
            foreach (dynamic Children in vTmp)
            {
                vTmpC.Add(new AraDesignJSonCanvasChildren(vFather,Children));
            }

            return vTmpC;
        }

        public string GetScript()
        {
            return Json.DynamicJson.Serialize(this);
        }

        public IEnumerable<IAraDesignJSonCanvasChildren> GetChildsMany()
        {
            List<IAraDesignJSonCanvasChildren> vTmp = new List<IAraDesignJSonCanvasChildren>();
            vTmp.AddRange(Canvas.Children.OrderBy(a=>Convert.ToInt32((a.Propertys.Where(b => b.Name == "Index").FirstOrDefault() != null ? a.Propertys.Where(b => b.Name == "Index").FirstOrDefault().Value : "-1"))).ToArray());

            foreach (var vC in Canvas.Children)
                vTmp.AddRange(GetChildsMany(vC));

            return vTmp;
        }

        private static IEnumerable<IAraDesignJSonCanvasChildren> GetChildsMany(IAraDesignJSonCanvasChildren vFather)
        {
            List<IAraDesignJSonCanvasChildren> vTmp = new List<IAraDesignJSonCanvasChildren>();
            vTmp.AddRange(vFather.Children.OrderBy(a => Convert.ToInt32((a.Propertys.Where(b => b.Name == "Index").FirstOrDefault() != null ? a.Propertys.Where(b => b.Name == "Index").FirstOrDefault().Value : "-1"))).ToArray());
            foreach (var vC in vFather.Children)
                vTmp.AddRange(GetChildsMany(vC));

            return vTmp;
        }
    }

    public interface IAraDesignJSon
    {
        IAraDesignJSonProjeto Projeto { get; set; }
        IAraDesignJSonCanvas Canvas { get; set; }

        //IEnumerable<IAraDesignJSonCanvasPropertys> GetListPropertys(dynamic vTmp);
        //IEnumerable<IAraDesignJSonCanvasChildren> GetListChildren(dynamic vTmp);
    }
}
