using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using System.ServiceModel.Channels;
using Tecnomips.Ara2_Dev_VS;

using System.ComponentModel.Design;
using Microsoft.VisualStudio;

namespace Ara2.Dev.AraDesign.Edit.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                 ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class PakVisualStudio : IPakVisualStudio
    {
        MyEditor MyEditor;
        public PakVisualStudio(MyEditor vMyEditor)
        {
            MyEditor=vMyEditor;
        }


        public void ChangePendingToSave()
        {
            try
            {
                MyEditor.ChangePendingToSaveInvoke();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakVisualStudio.ChangePendingToSave: " + err.ToDetailedString());
                throw err;
            }
        }


        public void ChangeCatUndo()
        {
            try
            {
                MyEditor.EditorPane.Invoke_onQueryUndo();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakVisualStudio.ChangeCatUndo: " + err.ToDetailedString());
                throw err;
            }
        }

        
        public void ChangeCatRedo()
        {
            try
            {
                MyEditor.EditorPane.Invoke_onQueryRedo();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakVisualStudio.ChangeCatRedo: " + err.ToDetailedString());
                throw err;
            }
        }

        public void ChangeObjectsList()
        {
            try
            {
                MyEditor.EditorPane.ChangeObjectsList();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakVisualStudio.ChangeObjectsList:" + err.ToDetailedString());
                throw err;
            }
        }

        public void SetObjectsEdit(string[] Names)
        {
            try
            {
                MyEditor.EditorPane.SetObjectsEdit(Names);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro  PakVisualStudio.SetObjectsEdit Names: '" + string.Join(",", Names) + "'\n " + err.ToDetailedString());
                throw err;
            }
        }

        public void FinallySetValue(int ID, Exception vErr)
        {
            try
            {
                MyEditor.EditorPane.FinallySetValue(ID, vErr);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro  PakVisualStudio.FinallySetValue ID:" + ID + " vErr:'\n\n" + vErr.ToDetailedString() + "\n\n' :" + err.ToDetailedString());
                throw err;
            }
        }

    }
    
}
