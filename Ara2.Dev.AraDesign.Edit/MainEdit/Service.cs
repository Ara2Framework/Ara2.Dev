using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using Ara2;
using Ara2.Components;
using System.ComponentModel;
using Ara2.Keyboard;

namespace Ara2.Dev.AraDesign.Edit.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                 ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class PakServerAraDevEdit : IPakServerAraDevEdit
    {
        #region Interno 
        MainEdit MainEdit;

        public PakServerAraDevEdit(MainEdit vMainEdit)
        {
            MainEdit = vMainEdit;
        }

        #region Sinc
        List<IEventSinc> EventSinc = new List<IEventSinc>();

        public void Sinc(Action Event)
        {
            EventSinc S = new EventSinc()
            {
                Event = Event
            };

            EventSinc.Add(S);
        }

        public void TickInterface()
        {
            lock (EventSinc)
            {
                foreach (IEventSinc ES in EventSinc.ToArray())
                {
                    try
                    {
                        ES.Executa();
                    }
                    finally
                    {
                        EventSinc.Remove(ES);
                    }

                }
            }
        }
        #endregion

        #endregion

        public void Close()
        {
            MainEdit.Dispose();
        }

        public bool GetPendingToSave()
        {
            try
            {
                return MainEdit.Edit.PendeteSalvar;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.GetPendingToSave: " + err.ToDetailedString());
                throw err;
            }
        }

        public void Save()
        {
            try
            {
                MainEdit.Edit.SaveJson();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.Save: " + err.ToDetailedString());
                throw err;
            }
        }

        /// <summary>
        /// Save As...
        /// </summary>
        /// <param name="vFile"></param>
        public void SaveAs(string vFile)
        {
            try
            {
                MainEdit.Edit.SaveJson(vFile);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.SaveAs: " + err.ToDetailedString());
                throw err;
            }
        }


        public void Undo()
        {
            try
            {
                Sinc(() =>
                {
                    MainEdit.Edit.Undo();
                });
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.Undo: " + err.ToDetailedString());
                throw err;
            }
        }


        public bool CatUndo()
        {
            try
            {
                return MainEdit.Edit.CatUndo();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.CatUndo: " + err.ToDetailedString());
                throw err;
            }
        }



        public void Redo()
        {
            try
            {
                Sinc(() =>
                {
                    MainEdit.Edit.Redo();
                });
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.Redo: " + err.ToDetailedString());
                throw err;
            }
        }


        public bool CatRedo()
        {
            try
            {
                return MainEdit.Edit.CatRedo();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.CatRedo: " + err.ToDetailedString());
                throw err;
            }
        }




        public byte[] GetOjectsDevAll()
        {
            try
            {
                byte[] vTmp2 = new byte[] { };
                using (MemoryStream fs = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, MainEdit.Edit.GetOjectsDevAll().Select(a => (object)a).Union(new object[] { (object)MainEdit.Edit.CanvasReal }).ToArray());
                    vTmp2 = fs.ToArray();
                }

                return vTmp2;

                //return MainEdit.Edit.GetOjectsDevAll()
                //    .Select(a => 
                //        new ObjectDev(
                //            SAraDevProperty.GetPropertys(a)
                //            .Where(b => b.Hide == false)
                //            .ToDictionary(d=>d.Nome,e=>e.Object)
                //        ))
                //    .ToArray();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.GetOjectsDevAll: " + err.ToDetailedString());
                throw err;
            }
        }


        public void SetValue(int IDSetValue, string Name, string Property, byte[] vValueB,bool IsNull)
        {
            try
            {
                Sinc(() =>
                {
                    Exception Erro = null;
                    try
                    {
                        object vValue;
                        if (!IsNull)
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            vValue = bf.Deserialize(new MemoryStream(vValueB));
                        }
                        else
                            vValue = null;

                        var vObj = MainEdit.Edit.GetOjectsDev(Name);
                            
                        PropertyInfo vP = null;
                        if (Property.IndexOf(".") != -1)
                        {
                            object vObjTmp = vObj;
                            object vObjTmpOld = vObj;
                            foreach (var vPName in Property.Split('.'))
                            {
                                vP = vObjTmp.GetType().GetProperties().Where(a => a.Name == vPName).FirstOrDefault();
                                vObjTmpOld = vObjTmp;
                                vObjTmp = vP.GetValue(vObjTmp, null);
                            }

                            if (!IsAraCustomEditor(vP))
                                vP.SetValue(vObjTmpOld, vValue, null);
                            else
                                OpenAraCustomEditor(vObj, vP);
                        }
                        else
                        {
                            vP = vObj.GetType().GetProperties().Where(a => a.Name == Property).FirstOrDefault();
                            if (!IsAraCustomEditor(vP))
                                vP.SetValue(vObj, vValue, null);
                            else
                                OpenAraCustomEditor(vObj, vP);
                        }

                        MainEdit.VS.Cliente.Channel(a => a.ChangeObjectsList());
                        if (Property == "Name")
                        {
                            MainEdit.Edit.ObjetoSendoEditado = new IAraDev[] { vObj };
                        }
                    }
                    catch (Exception err)
                    {
                        Erro = err;
                    }
                    finally
                    {

                        MainEdit.VS.Cliente.Channel(a => a.FinallySetValue(IDSetValue, Erro));
                    }
                });
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.SetValue: " + err.ToDetailedString());
                throw err;
            }
        }

        private bool IsAraCustomEditor(PropertyInfo vP)
        {
            return vP.GetCustomAttributes(typeof(AraCustomEditor), true).Any();
        }

        private void OpenAraCustomEditor(IAraDev vObj, PropertyInfo vP)
        {
            try
            {
                var AraCustomEditor = (AraCustomEditor)(vP.GetCustomAttributes(typeof(AraCustomEditor), true).First());
                var vTmpF = Activator.CreateInstance(AraCustomEditor.GetTypeForm(), (dynamic)MainEdit.GetInstance());
                var vForm = (IFormAraCustomEditor)vTmpF;
                if (MainEdit.Edit.CanvasReal.InstanceID == vObj.InstanceID)
                    vForm.ObjectCanvas = MainEdit.Edit.Canvas;
                else
                    vForm.ObjectCanvas = vObj;
                vForm.ObjectCanvasReal = vObj;
                vForm.PropertyInfo = vP;
                vForm.onRefreshScreen = () =>
                {
                    MainEdit.VS.Cliente.Channel(a => a.ChangeObjectsList());
                };
                ((AraWindow)vForm).Show(true);

                MainEdit.VS.Cliente.Channel(a => a.ChangeObjectsList());
            }
            catch(Exception err)
            {
                throw err;
            }
        }

        public byte[] GetObjectByIdInstance(string IdInstance)
        {
            try
            {
                byte[] vTmp2 = new byte[] { };
                using (MemoryStream fs = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, (object)MainEdit.Edit.GetObjectByIDInstace(IdInstance));
                    vTmp2 = fs.ToArray();
                }

                return vTmp2;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.GetObjectByIdInstance: " + err.ToDetailedString());
                throw err;
            }
        }



        public void AddObject(string vNameType)
        {
            try
            {
                Sinc(() =>
                {
                    var vObj = MainEdit.Edit.AddObject(vNameType);
                    MainEdit.VS.Cliente.Channel(a => a.ChangeObjectsList());
                    System.Threading.Thread.Sleep(500);
                    MainEdit.Edit.ObjetoSendoEditado = new Ara2.Dev.IAraDev[] { vObj };
                });
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.AddObject: " + err.ToDetailedString());
                throw err;
            }
        }


        public void SelectEditObject(string[] vNames)
        {
            try
            {
                Sinc(() =>
                {
                    MainEdit.Edit.ObjetoSendoEditado = MainEdit.Edit.GetOjectsDev(vNames);
                });
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.SelectEditObject: " + err.ToDetailedString());
                throw err;
            }
        }


        public void SelectAll()
        {
            try
            {
                Sinc(() =>
                {
                    MainEdit.Edit.SelectAll();
                });
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.SelectAll: " + err.ToDetailedString());
                throw err;
            }
        }


        public bool GetQueryCopy()
        {
            try
            {
                return MainEdit.Edit.ObjetoSendoEditado.Any();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.GetQueryCopy: " + err.ToDetailedString());
                throw err;
            }
        }


        public bool GetQueryCutOrDelete()
        {
            try
            {
                return MainEdit.Edit.ObjetoSendoEditado.Any();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.GetQueryCutOrDelete: " + err.ToDetailedString());
                throw err;
            }
        }


        public bool GetQueryPaste(int vFormatId, string vFormatName)
        {
            try
            {
                return MainEdit.Edit.ObjetoSendoEditado.Count() == 1;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.GetQueryPaste: " + err.ToDetailedString());
                throw err;
            }
        }


        public byte[] GetObjectBySendoEditado()
        {
            try
            {
                List<Ara2.Components.IAraObject> vObjsAll = new List<Components.IAraObject>();
                foreach (var vObj in MainEdit.Edit.ObjetoSendoEditado.Where(a => a != MainEdit.Edit.Canvas).Select(a => (IAraObject)a).ToArray())
                {
                    vObjsAll.Add(vObj);
                    //AraDevComponent vAraDevComponent = (AraDevComponent)vObj.GetType().GetCustomAttributes(typeof(AraDevComponent)).FirstOrDefault();
                    //if (vAraDevComponent == null || vAraDevComponent.Conteiner == true)
                    //{
                        var vChills = MainEdit.Edit.GetAllChilds(vObj);
                        if (vChills != null && vChills.Any())
                            vObjsAll.AddRange(vChills);
                    //}
                }

                byte[] vTmp2 = new byte[] { };
                using (MemoryStream fs = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, (object)vObjsAll.ToArray());
                    vTmp2 = fs.ToArray();
                }

                return vTmp2;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.GetObjectBySendoEditado: " + err.ToDetailedString());
                throw err;
            }
        }


        public void Paste(byte[] vBytesObjects)
        {
            try
            {
                Sinc(() =>
                {
                    var vObjPai = MainEdit.Edit.ObjetoSendoEditado.Single();
                    BinaryFormatter bf = new BinaryFormatter();
                    object[] vObjsTmp = new object[] { };
                    object[] vObjs;
                    try
                    {
                        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                        vObjs = ((object[])bf.Deserialize(new MemoryStream(vBytesObjects)));
                    }
                    finally
                    {
                        AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
                    }

                    List<IAraDev> vObjsNew = new List<IAraDev>();
                    var ConteinerFatherInstanceID = ((IAraDev)vObjs.Where(a => a is IAraDev).First()).ConteinerFather.InstanceID;
                    foreach (var vObj in vObjs.Where(a => a is IAraDev && ((IAraDev)a).ConteinerFather.InstanceID == ConteinerFatherInstanceID))
                    {
                        vObjsNew.Add(Copy((IAraDev)vObj, vObjPai, vObjs));
                    }

                    MainEdit.VS.Cliente.Channel(a => a.ChangeObjectsList());
                    System.Threading.Thread.Sleep(500);
                    MainEdit.Edit.ObjetoSendoEditado = vObjsNew.ToArray();
                });
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.Paste: " + err.ToDetailedString());
                throw err;
            }
        }

        bool AssemblyResolveExec = false;
        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            bool PrimeiroAssemblyResolveExec = false;
            try
            {
                Assembly vTmp;
                if (!AssemblyResolveExec)
                {
                    PrimeiroAssemblyResolveExec = true;
                    AssemblyResolveExec = true;
                    try
                    {
                        vTmp = AppDomain.CurrentDomain.Load((new AssemblyName(args.Name)).Name);
                        if (vTmp != null)
                            return vTmp;
                    }
                    catch { }
                }


                vTmp = MainEdit.Edit.References.Assemblys.Where(a => a.GetName().Name == (new AssemblyName(args.Name)).Name).FirstOrDefault();
                if (vTmp != null)
                    return vTmp;
                else
                    throw new Exception(args.Name + " not found");
            }
            finally
            {
                if (PrimeiroAssemblyResolveExec)
                    AssemblyResolveExec = false;
            }
        }

        private IAraDev Copy(IAraDev vObjBase, IAraDev vObjPai, object[] vObjs = null)
        {
            var vObjNew = MainEdit.Edit.AddObjectInternal(MainEdit.Edit.GetTypeByName(vObjBase.GetType().AssemblyQualifiedName), vObjPai);

            AraObjectInstanceGetObjectInstanceCustomByCopy.EventGetObjectInstanceCustom(ref vObjs, () =>
             {
                 // Colocar aqui para ler arainstance do vObjs do metodo Paste
                 foreach (SAraDevProperty vP in SAraDevProperty.GetPropertys(vObjBase))
                 {
                     try
                     {
                         SAraDevProperty.SetProperty(vObjNew, vP.Nome, (vP.Nome != "Name" ? vP.Value : NewNameCopy(vP.Value)));
                     }
                     catch (Exception err)
                     {
                         throw new Exception("Error on copy property '" + vP.Nome + "' value '" + vP.Value + "' type '" + vObjNew.GetType().FullName + "\n" + err.Message);
                     }
                 }
                 //vObjNew.Name = vObjPai.Name + "_" + vObjNew.InstanceID;
                 AraDevComponent vAraDevComponent =(AraDevComponent)vObjBase.GetType().GetCustomAttributes(typeof(AraDevComponent)).FirstOrDefault();
                 if (vAraDevComponent == null || vAraDevComponent.Conteiner == true)
                 {
                     foreach (var vObjFilho in vObjBase.Childs.Where(a => a is IAraDev).Select(a => (IAraDev)a))
                     {
                         Copy(vObjFilho, vObjNew);
                     }
                 }
             });

            return vObjNew;
        }

        private string NewNameCopy(string vNameAtual)
        {
            try
            {
                var AllNames = MainEdit.Edit.GetAllChilds(MainEdit.Edit.Canvas).Select(a => a.Name).ToArray();
                int N = 1;
                string NameBusca = vNameAtual;
                while (AllNames.Where(a => a == NameBusca).Any())
                {
                    NameBusca = vNameAtual + N;
                    N++;
                }
                return NameBusca;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.NewNameCopy: " + err.ToDetailedString());
                throw err;
            }
        }


        public void Delete()
        {
            try
            {
                Sinc(() =>
                {
                    foreach (var vObj in MainEdit.Edit.ObjetoSendoEditado)
                    {
                        if (vObj != MainEdit.Edit.Canvas)
                            vObj.Dispose();
                    }

                    MainEdit.Edit.ObjetoSendoEditado = new IAraDev[] { MainEdit.Edit.Canvas };
                });
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro PakServerAraDevEdit.Delete: " + err.ToDetailedString());
                throw err;
            }
        }

        public string GetTypeCananvas()
        {
            return ((dynamic)MainEdit.Edit.CanvasReal).GetType().ToString();
        }

        

        public void SetTypeCananvas(string vTypeName)
        {
            Sinc(() =>
            {
                var vPropertys = SAraDevProperty.GetPropertys(MainEdit.Edit.CanvasReal);

                Type vTypeCanvasReal = MainEdit.Edit.GetTypeByName(vTypeName);
                AraTools.DebuggerJS();
                if (vTypeCanvasReal == typeof(WindowMain))
                    MainEdit.Edit.CanvasReal = new CWindowMain();
                else
                    MainEdit.Edit.CanvasReal = (IAraDev)Activator.CreateInstance(vTypeCanvasReal, (dynamic)MainEdit.Edit);

                if (MainEdit.Edit.CanvasReal is AraWindow)
                    ((AraWindow)MainEdit.Edit.CanvasReal).Show();

                MainEdit.Edit.CanvasReal.Width = 300;
                MainEdit.Edit.CanvasReal.Height = 300;
                MainEdit.Edit.CanvasReal.Visible = false;

                foreach (var vp in vPropertys)
                {
                    try
                    {
                        SAraDevProperty.SetProperty(MainEdit.Edit.CanvasReal, vp.PropertyInfo.Name, vp.Value);
                    }
                    catch { }

                }

                MainEdit.VS.Cliente.Channel(a => a.ChangeObjectsList());
            });
        }

        #region CanvasProperties
        public string GetNameSpace()
        {
            return MainEdit.Edit.CanvasProperties.NameSpace;
        }
        public void SetNameSpace(string NameSpace)
        {
            MainEdit.Edit.CanvasProperties.NameSpace = NameSpace;
        }
        public string GetClassName()
        {
            return MainEdit.Edit.CanvasProperties.ClassName;
        }
        public void SetClassName(string ClassName)
        {
            MainEdit.Edit.CanvasProperties.ClassName = ClassName;
        }
        public string GetNameSpaceAraDesign()
        {
            return MainEdit.Edit.CanvasProperties.NameSpaceAraDesign;
        }
        public void SetNameSpaceAraDesign(string NameSpaceAraDesign)
        {
            MainEdit.Edit.CanvasProperties.NameSpaceAraDesign = NameSpaceAraDesign;
        }
        public string GetClassNameAraDesign()
        {
            return MainEdit.Edit.CanvasProperties.ClassNameAraDesign;
        }
        public void SetClassNameAraDesign(string ClassNameAraDesign)
        {
            MainEdit.Edit.CanvasProperties.ClassNameAraDesign = ClassNameAraDesign;
        }
        #endregion
    }

    #region Sinc
    public interface IEventSinc
    {
        void Executa();
    }

    public class EventSinc : IEventSinc
    {
        public Action Event;
        public bool Executou = false;
        public object Return;
        public Exception Error = null;

        public void Executa()
        {
            try
            {
                ((dynamic)Event)();
            }
            catch (Exception err)
            {
                Error = err;
            }
            finally
            {
                Executou = true;
            }


        }
    }
    #endregion
}