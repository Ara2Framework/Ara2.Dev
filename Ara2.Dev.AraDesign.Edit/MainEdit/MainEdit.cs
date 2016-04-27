using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ara2;
using Ara2.Components;
using System.IO;
using Ara2.Dev.AraDesign.Edit.Service;

namespace Ara2.Dev.AraDesign.Edit
{
    [Serializable]
    public class MainEdit : MainEditAraDesign
    {
        public ClienteServerChannel<IPakVisualStudio, PakServerAraDevEdit, IPakServerAraDevEdit> VS;

        public MainEdit(Ara2.Session Session)
            : base(Session)
        {
            Tick Tick = Tick.GetTick();

            //if (string.IsNullOrEmpty(Tick.Page.Request["FileProject"]) || !File.Exists(Tick.Page.Request["FileProject"]))
            //    throw new Exception("Parametro get 'FileProject' invalido");

            //if (string.IsNullOrEmpty(Tick.Page.Request["File"]) || !File.Exists(Tick.Page.Request["File"]))
            //    throw new Exception("Parametro get 'File' invalido");
            AraDevPropertyLayoutCurrent.CustomWindowType = typeof(Ara2.Components.Layout.FrmLayout);
            AraCustomEditorFrmLayout.CustomWindowType = typeof(Ara2.Components.Layout.FrmLayout);
            

            if (
                !string.IsNullOrEmpty(Tick.Page.Request["FileProject"]) && File.Exists(Tick.Page.Request["FileProject"])
                && !string.IsNullOrEmpty(Tick.Page.Request["File"]) && File.Exists(Tick.Page.Request["File"]))
            {
                Timer.tick += Timer_tick;
                Timer.Interval = 1000;
                Timer.Enabled = true;

                Edit.ChangePendeteSalvar += ChangePendeteSalvar;
                Edit.ChangeCatRedo += ChangeCatRedo;
                Edit.ChangeCatUndo += ChangeCatUndo;
                Edit.ChangeObjectsList += ChangeObjectsList;
                Edit.OnObjetoSendoEditado += OnObjetoSendoEditado;

                VS = new ClienteServerChannel<IPakVisualStudio,  PakServerAraDevEdit, IPakServerAraDevEdit>(new PakServerAraDevEdit(this), Convert.ToInt32(Tick.Page.Request["AraDevEditPort"]), Convert.ToInt32(Tick.Page.Request["VSPort"]));

                Edit.FileProject = new FileInfo(Tick.Page.Request["FileProject"]);
                Edit.FileAraDesign = new FileInfo(Tick.Page.Request["File"]);
            }
        }

        private void Timer_tick()
        {
            // Ant Close
            VS.Server.Instance.TickInterface();
        }

        public static MainEdit GetInstance()
        {
            return (MainEdit)(Tick.GetTick().Session.GetObjectsByType(typeof(MainEdit)).First().Object);
        }

        public void ChangePendeteSalvar()
        {
            VS.Cliente.Channel(a => a.ChangePendingToSave()); 
        }

        public void ChangeCatRedo()
        {
            VS.Cliente.Channel(a => a.ChangeCatRedo()); 
        }

        public void ChangeCatUndo()
        {
            VS.Cliente.Channel(a => a.ChangeCatUndo()); 
        }

        public void ChangeObjectsList()
        {
            VS.Cliente.Channel(a => a.ChangeObjectsList());
        }

        public void OnObjetoSendoEditado(string[] Names)
        {
            VS.Cliente.Channel(a => a.SetObjectsEdit(Names));
        }
        
        public new void Dispose()
        {
            Edit.Dispose();
            base.Dispose();
        } 
    }
}  