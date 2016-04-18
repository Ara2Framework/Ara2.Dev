using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Dev.AraDesign
{
    [Serializable]
    public class AraDesignJSonProjeto : IAraDesignJSonProjeto
    {
        public AraDesignJSonProjeto(string vName, int vVersion)
        {
            Name = vName;
            DateLastModified = DateTime.Now;
            Version = vVersion;
        }

        public AraDesignJSonProjeto(dynamic vProjeto)
        {
            Name = vProjeto.Name;
            DateLastModified = Convert.ToDateTime(vProjeto.DateLastModified);
            Version = Convert.ToDouble(vProjeto.Version);
        }

        public string Name { get; set; }
        public DateTime DateLastModified { get; set; }
        public double Version { get; set; }
    }

    public interface IAraDesignJSonProjeto
    {
        string Name { get; set; }
        DateTime DateLastModified { get; set; }
        double Version { get; set; }
    }
}
