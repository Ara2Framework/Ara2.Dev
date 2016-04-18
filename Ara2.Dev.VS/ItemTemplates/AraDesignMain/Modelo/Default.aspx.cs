using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ara2;
using System.Web.Configuration;
using System.IO;

namespace $rootnamespace$
{
    public partial class _$fileinputname$ : AraPageMain
    {
        public override Ara2.Memory.IAraMemoryArea GetMemoryArea()
        {
            return new Ara2.Memory.AraMemoryAreaPool();
        }

        public override Ara2.Components.WindowMain GetWindowMain(Ara2.Session Session)
        {
            return new $fileinputname$(Session);
        }

        public override void ExceptionAplication(Exception err)
        {
            try
            {
                if (!Directory.Exists(Path.Combine(AraTools.GetPath(), "log")))
                    Directory.CreateDirectory(Path.Combine(AraTools.GetPath(), "log"));

                using (StreamWriter sw = File.AppendText(Path.Combine(AraTools.GetPath(), "log", DateTime.Now.ToString("yyyy-MM-dd") + ".log")))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + " " + Tick.GetTick().Session.Id + " " + err.ToDetailedString() + "\n");
                }

                AraTools.Alert(err.Message);
            }
            catch { }
        }

        public override string GetJQueryUICss()
        {
            //if (Config.Get["Skin"] != "")
            //    return Config.Get["Skin"];
            //else
                return base.GetJQueryUICss();
        }

        public override void OnEndTick(Tick vTick)
        {
            // Aqui você pode finalizar alguma conexão em aberto ao algo aberto netse Tick
            //$rootnamespace$.Database.DB.DBContextClose();
            base.OnEndTick(vTick);
        }
    }
}