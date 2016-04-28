using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ara2;
using System.Web.Configuration;
using Ara2.Dev.AraDesign.Edit.Tools;
using System.IO;

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

        //public override void BeforeReceivingTick()
        //{
        //    Tick Tick = Tick.GetTick();
        //    if (Tick.Session.UrlRedirectFiles ==null && !string.IsNullOrWhiteSpace(Tick.Page.Request["FileProject"]))
        //        Tick.Session.UrlRedirectFiles = "?FileKey=" + this.GetKeySendFile() + "&File=" + Path.GetDirectoryName(Tick.Page.Request["FileProject"]) + "\\";
        //        //Tick.Session.UrlRedirectFiles = "file:///" + Path.GetDirectoryName(Tick.Page.Request["FileProject"]).Replace("\\", "/") + "/";
        //}

        public override string GetUrlRedirectFiles(string vFile)
        {
            MainEdit vMainEdit = MainEdit.GetInstance();
            if (vMainEdit!=null && vMainEdit.Edit!=null && vMainEdit.Edit.FileProject !=null)
                return "?FileKey=" + this.GetKeySendFile() + "&File=" + Path.GetDirectoryName(vMainEdit.Edit.FileProject.FullName) + "\\" + vFile;
            else
                return vFile;
        }
    }
}