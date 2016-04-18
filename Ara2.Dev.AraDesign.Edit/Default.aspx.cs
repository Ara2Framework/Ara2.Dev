using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ara2;
using System.Web.Configuration;
using Ara2.Dev.AraDesign.Edit.Tools;

namespace Ara2.Dev.AraDesign.Edit
{
    public partial class _Default : AraPageMain
    {
        public override Ara2.Memory.IAraMemoryArea GetMemoryArea()
        {
            if (Config.Get["AraMemory"] == "AraMemoryAreaFile")
                return new Ara2.Memory.AraMemoryAreaFile();
            else if (Config.Get["AraMemory"] == "AraMemoryAreaPool")
                return new Ara2.Memory.AraMemoryAreaPool();
            else if (Config.Get["AraMemory"] == "AraMemoryAreaPoolFile")
                return new Ara2.Memory.AraMemoryAreaPoolFile();
            else
                return new Ara2.Memory.AraMemoryAreaPool();
        }

        public override Ara2.Components.WindowMain GetWindowMain(Ara2.Session Session)
        {
            return new MainEdit(Session);
        }

        public override string GetJQueryUICss()
        {
            if (Config.Get["Skin"] != "")
                return Config.Get["Skin"];
            else
                return base.GetJQueryUICss();
        }
    }
}