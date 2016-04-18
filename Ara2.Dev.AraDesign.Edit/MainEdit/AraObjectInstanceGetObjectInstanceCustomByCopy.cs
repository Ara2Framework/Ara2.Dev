using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ara2.Components;

namespace Ara2.Dev.AraDesign.Edit
{
    public static class AraObjectInstanceGetObjectInstanceCustomByCopy
    {

        #region AraObjectInstance GetObjectInstanceCustom
        public static object AraObjectInstance_GetObjectInstanceCustom(string IdInstance)
        {
            if (vEditorPaneByThead.ContainsKey(System.Threading.Thread.CurrentThread.ManagedThreadId))
            {
                object[] EditorPane;
                if (vEditorPaneByThead.TryGetValue(System.Threading.Thread.CurrentThread.ManagedThreadId, out EditorPane))
                {
                    var vBs = EditorPane.Select(a=>(IAraObject)a).Where(a=>a.InstanceID == IdInstance).FirstOrDefault();
                    if (vBs != null)
                        return vBs;
                    else
                        throw new Exception("EventGetObjectInstanceCustom não chamado");
                }
                else
                    throw new Exception("EventGetObjectInstanceCustom não chamado");
            }
            else if (OldGetObjectInstanceCustom != null)
                return OldGetObjectInstanceCustom(IdInstance);
            else
                throw new Exception("EventGetObjectInstanceCustom não chamado");
        }

        private static Dictionary<int, object[]> vEditorPaneByThead = new Dictionary<int, object[]>();
        public static void EventGetObjectInstanceCustom(ref object[] vObjs, Action vEvent)
        {
            EventGetObjectInstanceCustom<object>(ref vObjs, () => { vEvent(); return null; });
        }

        private static Func<string, object> OldGetObjectInstanceCustom = null;
        public static object EventGetObjectInstanceCustom<T>(ref object[] vObjs, Func<T> vEvent)
        {
            if (Ara2.Components.AraObjectInstanceStatic.GetObjectInstanceCustom == null)
            {
                if (Ara2.Components.AraObjectInstanceStatic.GetObjectInstanceCustom != AraObjectInstance_GetObjectInstanceCustom)
                    OldGetObjectInstanceCustom = Ara2.Components.AraObjectInstanceStatic.GetObjectInstanceCustom;
                else
                    OldGetObjectInstanceCustom = null;

                Ara2.Components.AraObjectInstanceStatic.GetObjectInstanceCustom += AraObjectInstance_GetObjectInstanceCustom;
            }

            object[] vTmpObjs;
            if (!vEditorPaneByThead.TryGetValue(System.Threading.Thread.CurrentThread.ManagedThreadId, out vTmpObjs))
            {

                lock (vEditorPaneByThead)
                {
                    vEditorPaneByThead.Add(System.Threading.Thread.CurrentThread.ManagedThreadId, vObjs);
                }

                try
                {
                    return vEvent();
                }
                finally
                {
                    lock (vEditorPaneByThead)
                    {
                        vEditorPaneByThead.Remove(System.Threading.Thread.CurrentThread.ManagedThreadId);
                    }

                    Ara2.Components.AraObjectInstanceStatic.GetObjectInstanceCustom -= AraObjectInstance_GetObjectInstanceCustom;
                    if (OldGetObjectInstanceCustom != null)
                        Ara2.Components.AraObjectInstanceStatic.GetObjectInstanceCustom = OldGetObjectInstanceCustom;
                    else
                        Ara2.Components.AraObjectInstanceStatic.GetObjectInstanceCustom = null;

                    OldGetObjectInstanceCustom = null;
                }
            }
            else
                return vEvent();
        }
        #endregion
    }
}