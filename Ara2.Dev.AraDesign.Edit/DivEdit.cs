using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ara2.Components;
using System.IO;
using Ara2.Dev;
using Ara2.Dev.AraDesign.Edit.Service;

namespace Ara2.Dev.AraDesign.Edit
{
    public class DivEdit:AraDiv
    {        
        public DivEdit(IAraObject vContainer)
            :base(vContainer)
        {
            this.ScrollBar = new AraScrollBar(this);
            this.Selectable = new AraSelectable(this);
            this.Selectable.tolerance = AraSelectable.Etolerance.fit;
            this.Selectable.Stop += Selectable_Stop;

            Canvas = new AraDiv(this);
            Canvas.Left = 10;
            Canvas.Top = 10;
            Canvas.Width = 300;
            Canvas.Height = 300;
            Canvas.StyleContainer = true;

            Canvas.StartEditPropertys += this_StartEditPropertys;
            Canvas.ChangeProperty += this_ChangeProperty;

            Canvas.Resizable = new AraResizable(Canvas);
            Canvas.Resizable.OnResize += AlterouLaguraAlturaCanvas;
            this.Selectable.AddObjectSelectable(Canvas);

        }

        AraDesignProjectReferences References=null;
        FileInfo _FileProject;
        public FileInfo FileProject
        {
            get { return _FileProject; }
            set
            {
                _FileProject = value;
                References = new AraDesignProjectReferences(_FileProject);
            }
        }

        FileInfo _FileAraDesign;
        public FileInfo FileAraDesign
        {
            get { return _FileAraDesign; }
            set { 
                _FileAraDesign = value; 

                string vScript = File.ReadAllText(_FileAraDesign.FullName);

                if (string.IsNullOrEmpty(vScript))
                    JSon = new AraDesignJSon();
                else
                    JSon = new AraDesignJSon(vScript);

                var vFileName = Path.GetFileNameWithoutExtension(_FileAraDesign.FullName);
                if (this.CanvasReal.Name != vFileName)
                    this.CanvasReal.Name = vFileName;

                PendeteSalvar = false;
                SalvarSave();
            }
        }

        private AraObjectInstance<AraDiv> _Canvas = new AraObjectInstance<AraDiv>();
        public AraDiv Canvas
        {
            get { return _Canvas.Object; }
            set { _Canvas.Object = value; }
        }

        private AraObjectInstance<IAraDev> _CanvasReal = new AraObjectInstance<IAraDev>();
        public IAraDev CanvasReal
        {
            get { return (_CanvasReal==null? null : _CanvasReal.Object); }
            set {
                _CanvasReal.Object = value;

                //_CanvasRealComun = Merger.Merge(CanvasReal, CanvasComun);
            }
        }

        //public CanvasComun CanvasComun = new CanvasComun();
        //object _CanvasRealComun = null;
        //public object CanvasRealComun
        //{
        //    get
        //    {
        //        return _CanvasRealComun;
        //    }
        //}
        public CanvasProperties CanvasProperties;
        #region Acerta Zindex
        public void AcertaZindex()
        {
            foreach (IAraDev vObjDev in GetAllChilds(this.Canvas))
            {
                if (vObjDev.ZIndex == null)
                {
                    int? vMax = ((IAraDev)vObjDev.ConteinerFather).Childs.Max(a => ((IAraDev)a).ZIndex);
                    if (vMax == null) vMax = 0;
                    vMax += 1;
                    vObjDev.ZIndex = vMax;
                }
            }
            
        }
        public IAraDev[] GetAllChilds(IAraDev vObjPai)
        {
            List<IAraDev> TmpR = new List<IAraDev>();

            foreach (IAraDev TmpObj in vObjPai.Childs.Where(a=>a is IAraDev))
            {
                TmpR.Add(TmpObj);
            }
            TmpR = TmpR.OrderBy(a => (a is IAraComponentVisual ? ((IAraComponentVisual)a).ZIndex : 10000)).ToList();


            foreach (IAraDev TmpObj in vObjPai.Childs.Where(a => a is IAraDev))
            {
                if (AraDevComponent.Get(TmpObj).Conteiner)
                    TmpR.AddRange(GetAllChilds(TmpObj));
            }

            return TmpR.ToArray();
        }

        public IAraObject[] GetAllChilds(IAraObject vObjPai)
        {
            List<IAraObject> TmpR = new List<IAraObject>();

            foreach (IAraObject TmpObj in vObjPai.Childs)
            {
                TmpR.Add(TmpObj);
            }
            TmpR = TmpR.OrderBy(a => (a is IAraComponentVisual ? ((IAraComponentVisual)a).ZIndex : 10000)).ToList();


            foreach (IAraObject TmpObj in vObjPai.Childs)
            {
                TmpR.AddRange(GetAllChilds(TmpObj));
            }

            return TmpR.ToArray();
        }

        public IAraObject GetObjectByIDInstace(string IDInstance )
        {
            if (this.CanvasReal.InstanceID == IDInstance)
                return this.CanvasReal;
            else
            {
                var vObj = GetObjectByIDInstace(this.Canvas, IDInstance);

                if (vObj != null)
                    return vObj;
                else
                    return GetObjectByIDInstace(this.CanvasReal, IDInstance);               
            }
        }

        public IAraObject GetObjectByIDInstace(IAraObject vObjPai, string IDInstance)
        {
            IAraObject vObjR = vObjPai.Childs.Where(a => a.InstanceID == IDInstance).FirstOrDefault();
            if (vObjR != null)
                return vObjR;

            foreach (IAraObject TmpObj in vObjPai.Childs)
            {
                IAraObject vObjR2 = GetObjectByIDInstace(TmpObj, IDInstance);
                if (vObjR2 != null)
                    return vObjR2;
            }

            return null;
        }
        #endregion

        #region Json
        //AraDesignJSon _JSon;

        public AraDesignJSon JSon
        {
            get { return GetJson(); }
            set { 
                CarregaJson(value);
                
            }
        }


        #region Carrega 
        bool CarregandoJson = false;
        private void CarregaJson(AraDesignJSon vJSon)
        {
            CarregandoJson = true;
            try
            {
                //Salva variavel PendeteSalvar
                var vTmpPendeteSalvar = PendeteSalvar;

                #region descarrega CanvasReal
                if (CanvasReal != null)
                {
                    foreach (IAraObject vObj in Canvas.Childs.ToArray())
                    {
                        try
                        {
                            vObj.Dispose();
                        }
                        catch { }
                        try
                        {
                            Canvas.RemuveChild(vObj);
                        }
                        catch { }
                    }

                    CanvasReal.Dispose();
                    CanvasReal = null;
                }
                #endregion

                if (!string.IsNullOrEmpty(vJSon.Canvas.TypeAssembly))
                {
                    Type vTypeCanvasReal;
                    //if (vJSon.Canvas.TypeAssembly == "")
                    //    vTypeCanvasReal = GetTypeByName(vJSon.Canvas.TypeAssembly);
                    //else
                        vTypeCanvasReal = GetTypeByName(vJSon.Canvas.TypeAssembly);
                    if (vTypeCanvasReal == typeof(WindowMain))
                        CanvasReal = new CWindowMain();
                    else
                    {
                        try
                        {
                            CanvasReal = (IAraDev)Activator.CreateInstance(vTypeCanvasReal, (IAraObject)(dynamic)this);
                        }
                        catch(Exception err)
                        {
                            throw new Exception("Erro on load class '" + vJSon.Canvas.TypeAssembly + "'",err);
                        }
                    }
                    CanvasProperties = new CanvasProperties()
                    {
                        NameSpace = vJSon.Canvas.NameSpace,
                        ClassName = vJSon.Canvas.ClassName,
                        NameSpaceAraDesign = vJSon.Canvas.NameSpaceAraDesign,
                        ClassNameAraDesign = vJSon.Canvas.ClassNameAraDesign,
                        TypeCanas = null
                    };
                }
                else
                    CanvasReal = new AraWindow(this);

                if (CanvasReal is AraWindow)
                    ((AraWindow)CanvasReal).Show();

                CanvasReal.Width = 300;
                CanvasReal.Height = 300;
                CanvasReal.Visible = false;
                
                foreach (var vProperty in vJSon.Canvas.Propertys)
                {
                    try
                    {
                        SAraDevProperty.SetProperty(CanvasReal, vProperty.Name, vProperty.Value);

                        if (vProperty.Name == "Width" || vProperty.Name == "Height" ||
                            vProperty.Name == "LayoutsString" || vProperty.Name == "LayoutCurrent")
                            SAraDevProperty.SetProperty(Canvas, vProperty.Name, vProperty.Value);
                    }
                    catch (Exception err)
                    {
                        AraTools.Alert("Erro on load Property '" + vProperty.Name + "' value '" + vProperty.Value + "'.\n" + err.Message);
                    }

                }

                if (CanvasReal is IAraContainerClient && ((IAraContainerClient)CanvasReal).Layouts != null)
                    ((IAraContainerClient)CanvasReal).Layouts.Enable = false;

                CarregaJson_PartChildren(Canvas, vJSon.Canvas.Children);

                //Canvas.Resizable = new AraResizable(Canvas);
                PendeteSalvar = vTmpPendeteSalvar;
            }
            finally
            {
                CarregandoJson = false;
                CheckChangeObjectNames();
            }
        }

        private void CarregaJson_PartChildren(IAraDev vObjCPai, IEnumerable<IAraDesignJSonCanvasChildren> vTmp)
        {

            foreach (var vTmpObj in vTmp)
            {
                Type vTmpType = GetTypeByName(vTmpObj.TypeNameInternal.ToString());

                IAraDev vTmpObjEstanciado = AddObjectInternal(vTmpType, vObjCPai);

                foreach (dynamic vProperty in vTmpObj.Propertys)
                {
                    try
                    {
                        SAraDevProperty.SetProperty(vTmpObjEstanciado, vProperty.Name, vProperty.Value);
                    }
                    catch (Exception err)
                    {
                        AraTools.Alert("Erro on load Property '" + vProperty.Name + "' value '" + vProperty.Value + "' Type:'" + GetTypeToString(vTmpObjEstanciado.GetType()) + "'.\n" + err.Message);
                    }
                }

                if (AraDevComponent.Get(vTmpObjEstanciado).Conteiner)
                    CarregaJson_PartChildren((IAraDev)vTmpObjEstanciado, vTmpObj.Children);
            }

        }
        #endregion

        #region Cria
        private AraDesignJSon GetJson()
        {
            AraDesignJSon vArquivoJson = new AraDesignJSon();

            vArquivoJson.Projeto = new AraDesignJSonProjeto(
                    SAraDevProperty.GetPropertys(CanvasReal).Where(a => a.Nome == "Name").Select(a => a.Value.ToString()).First(),
                    1);

            Type vCanvasRealType = CanvasReal.GetType();
            if (vCanvasRealType == typeof(Ara2.Dev.AraDesign.Edit.Service.CWindowMain))
                vCanvasRealType = typeof(WindowMain);

            vArquivoJson.Canvas = new AraDesignJSonCanvas(
                    vCanvasRealType.AssemblyQualifiedName, vCanvasRealType.FullName,
                    CanvasProperties.NameSpace,
                    CanvasProperties.ClassName,
                    CanvasProperties.NameSpaceAraDesign,
                    CanvasProperties.ClassNameAraDesign
                );

            if (CanvasReal is IAraContainerClient && ((IAraContainerClient)Canvas).Layouts != null)
            {
                ((IAraContainerClient)Canvas).Layouts.GetLayoutCurrent().Save();
                ((IAraContainerClient)CanvasReal).LayoutsString = ((IAraContainerClient)Canvas).LayoutsString;
            }

            List<IAraDesignJSonCanvasPropertys> JsonCanvasPropertys = new List<IAraDesignJSonCanvasPropertys>();
            foreach (SAraDevProperty vProperty in SAraDevProperty.GetPropertys(CanvasReal))
            {
                if (vProperty.Nome == "Name" || vProperty.IsDefault == false &&
                    (vProperty.Nome != "Width" && vProperty.Nome != "Height"))
                    JsonCanvasPropertys.Add(new AraDesignJSonCanvasPropertys()
                    {
                        Name = vProperty.Nome,
                        ValueType = vProperty.ValueType.AssemblyQualifiedName,
                        Value = vProperty.Value,
                        ValueDefault = Convert.ToString(vProperty.ValueDefault),
                        CustomEdition = false
                    });
                    
            }

            foreach (SAraDevProperty vProperty in SAraDevProperty.GetPropertys(Canvas).Where(a => a.Nome == "Width" || a.Nome == "Height"))
            {
                JsonCanvasPropertys.Add(new AraDesignJSonCanvasPropertys()
                {
                    Name = vProperty.Nome,
                    ValueType = vProperty.ValueType.AssemblyQualifiedName,
                    Value = vProperty.Value,
                    ValueDefault = Convert.ToString(vProperty.ValueDefault),
                    CustomEdition = false
                });
            }
            vArquivoJson.Canvas.Propertys=JsonCanvasPropertys;

            SalvaJson_PartChildren(Canvas, vArquivoJson.Canvas.Children);
            return vArquivoJson;
        }

        private void SalvaJson_PartChildren(IAraDev vObjCPai, IEnumerable<IAraDesignJSonCanvasChildren> vTmp)
        {
            foreach (IAraDev vObjC in vObjCPai.Childs.Where(a=> a is IAraDev))
            {
                AraDesignJSonCanvasChildren vTmpObj = new AraDesignJSonCanvasChildren(vObjC.GetType().FullName, vObjC.GetType().AssemblyQualifiedName);

                if (vObjC is IAraContainerClient && ((IAraContainerClient)vObjC).Layouts != null && ((IAraContainerClient)vObjC).Layouts.GetLayoutCurrent()!=null)
                    ((IAraContainerClient)vObjC).Layouts.GetLayoutCurrent().Save();

                List<IAraDesignJSonCanvasPropertys> JsonCanvasPropertys = new List<IAraDesignJSonCanvasPropertys>();
                foreach (SAraDevProperty vProperty in SAraDevProperty.GetPropertys(vObjC))
                {
                    if (vProperty.Nome == "Name" || vProperty.IsDefault == false)
                        JsonCanvasPropertys.Add(new AraDesignJSonCanvasPropertys()
                        {
                            Name = vProperty.Nome,
                            ValueType = vProperty.ValueType.AssemblyQualifiedName,
                            Value = vProperty.Value,
                            ValueDefault = Convert.ToString(vProperty.ValueDefault),
                            CustomEdition = false
                        });
                }
                vTmpObj.Propertys = JsonCanvasPropertys;

                ((List<IAraDesignJSonCanvasChildren>)vTmp).Add(vTmpObj);

                if (AraDevComponent.Get(vObjC).Conteiner)
                    SalvaJson_PartChildren(vObjC, vTmpObj.Children);
            }
        }
        #endregion


        public void SaveJson()
        {
            SaveJson(_FileAraDesign.FullName);
        }

        public void SaveJson(string vFile)
        {
            File.WriteAllText(vFile, JSon.GetScript());
        }

        public IAraDev AddObjectInternal(Type vType, IAraObject vObjPai)
        {
            IAraDev vTmpObj=null;
            try
            {
                vTmpObj = (IAraDev)Activator.CreateInstance(vType, vObjPai);
            }
            catch (Exception err)
            {
                throw new Exception("Erro on load class '" + vType.AssemblyQualifiedName + "'", err);
            }

            vTmpObj.Name = vTmpObj.InstanceID;
            vTmpObj.CssAddClass("Ara2DevBordaTracejada");
            vTmpObj.StartEditPropertys += this_StartEditPropertys;
            vTmpObj.ChangeProperty += this_ChangeProperty;
            this.Selectable.AddObjectSelectable(vTmpObj);
            AtivaResizableDraggableObjeto(vTmpObj);

            return vTmpObj;
        }

        public IAraDev AddObject(string vTypeName)
        {
            return AddObject(GetTypeByName(vTypeName));
        }

        public IAraDev AddObject(Type vType)
        {
            if (this.ObjetoSendoEditado != null && this.ObjetoSendoEditado.Count() == 1)
                return AddObject(vType, this.ObjetoSendoEditado[0]);
            else
                throw new Exception("Selecione o objeto pai onde você deseja adcionar o novo obejto");
        }

        public IAraDev AddObject(Type vType, IAraObject vObjPai)
        {
            var vTmpObj = AddObjectInternal(vType, vObjPai);
            CheckChangeObjectNames();
            PendeteSalvar = true;
            return vTmpObj;
        }

        public void AtivaResizableDraggableObjeto(IAraComponentVisual vObj)
        {
            if (vObj.TypePosition != ETypePosition.Static)
            {
                if (AraDevComponent.Get((IAraDev)vObj).Resizable)
                {
                    vObj.Resizable = new AraResizable(vObj);
                    vObj.Resizable.OnResize += AlterouLaguraAlturaCanvas;
                }

                if (AraDevComponent.Get((IAraDev)vObj).Draggable)
                {
                    if (Canvas != vObj)
                    {
                        vObj.Draggable = new AraDraggable(vObj);
                        vObj.Draggable.OnDraggable += AlterouLeftTopCanvas;
                    }
                    else
                    {
                        //vObj.Selectable = new AraSelectable(vObj);
                        //vObj.Selectable.Selected += this_Selected;
                        //vObj.Selectable.UnSelected += this_UnSelected;
                    }
                }
            }
        }

        #region Type
        public Type GetTypeByName(string vNameFull)
        {
            
            try
            {
                return References.Components.Where(a => a.AssemblyQualifiedName == vNameFull).First();
            }
            catch
            {
                try
                {
                    string STypeAssembly2 = vNameFull.Substring(0, vNameFull.IndexOf(","));

                    Type vTmpType = References.Components.Where(a => a.FullName == STypeAssembly2).First();
                    return vTmpType;
                }
                catch
                {
                    throw new Exception("Assembly '" + vNameFull + "' no load!");
                }
            }
        }

        private string GetTypeToString(Type vType)
        {
            if (vType.Equals(typeof(void)))
                return "void";
            else
                return GetCSharpRepresentation(vType, true);

        }

        static string GetCSharpRepresentation(Type t, bool trimArgCount)
        {
            if (t.IsGenericType)
            {
                var genericArgs = t.GetGenericArguments().ToList();

                return GetCSharpRepresentation(t, trimArgCount, genericArgs);
            }

            return t.FullName;
        }

        static string GetCSharpRepresentation(Type t, bool trimArgCount, List<Type> availableArguments)
        {
            if (t.IsGenericType)
            {
                string value = t.Name;
                if (trimArgCount && value.IndexOf("`") > -1)
                {
                    value = value.Substring(0, value.IndexOf("`"));
                }

                if (t.DeclaringType != null)
                {
                    // This is a nested type, build the nesting type first
                    value = GetCSharpRepresentation(t.DeclaringType, trimArgCount, availableArguments) + "+" + value;
                }

                // Build the type arguments (if any)
                string argString = "";
                var thisTypeArgs = t.GetGenericArguments();
                for (int i = 0; i < thisTypeArgs.Length && availableArguments.Count > 0; i++)
                {
                    if (i != 0) argString += ", ";

                    argString += GetCSharpRepresentation(availableArguments[0], trimArgCount);
                    availableArguments.RemoveAt(0);
                }

                // If there are type arguments, add them with < >
                if (argString.Length > 0)
                {
                    value += "<" + argString + ">";
                }

                return value;
            }

            return t.FullName;
        }

        #endregion

        #region Controle Versão Edição
        private bool _PendeteSalvar = false;

        Dictionary<int, string> Saves = new Dictionary<int, string>();
        int NSave = 1;

        public AraEvent<Action> ChangePendeteSalvar = new AraEvent<Action>();
        public bool PendeteSalvar
        {
            get { return _PendeteSalvar; }
            set
            {
                bool Mudou = _PendeteSalvar != value;
                _PendeteSalvar = value;

                //if (_PendeteSalvar)
                //    Aba.Caption = Objeto.Name + "(*)";
                //else
                //    Aba.Caption = Objeto.Name;

                //new System.Threading.Thread(SalvarSave).Start();
                if (_PendeteSalvar)
                    SalvarSave();
                else
                {
                    Saves.Clear();
                    _VisulizandoSave = null;
                }

                if (Mudou && ChangePendeteSalvar.InvokeEvent != null)
                    ChangePendeteSalvar.InvokeEvent();
            }
        }

        const int nSavesMax = 50;

        private void SalvarSave()
        {
            if (CarregandoJson)
                return;

            lock (Saves)
            {
                if (_VisulizandoSave != null)
                {
                    foreach (int vKey in Saves.Keys.Where(a => a > (int)_VisulizandoSave).ToArray())
                    {
                        Saves.Remove(vKey);
                    }
                }

                if (Saves.Count() > nSavesMax)
                {
                    foreach (int vKey in Saves.Keys.OrderByDescending(a => a).Skip(nSavesMax).ToArray())
                    {
                        Saves.Remove(vKey);
                    }
                }


                int vNewKey;
                if (Saves.Keys.Count() > 0)
                    vNewKey = Saves.Keys.Max() + 1;
                else
                    vNewKey = 1;
                Saves.Add(vNewKey, JSon.GetScript());
                _VisulizandoSave = null;
            }
        }

        int? _VisulizandoSave = null;

        #region Undo
        public AraEvent<Action> ChangeCatUndo = new AraEvent<Action>();
        public void Undo()
        {
            if (_VisulizandoSave == null)
                _VisulizandoSave = Saves.Keys.Max();

            if (_VisulizandoSave > Saves.Keys.Min())
            {
                _VisulizandoSave = (int)_VisulizandoSave - 1;
                JSon = new AraDesignJSon(Saves[(int)_VisulizandoSave]);

                if (ChangeCatUndo.InvokeEvent != null)
                    ChangeCatUndo.InvokeEvent();
            }
        }

        public bool CatUndo()
        {
            if (Saves.Count <= 1)
                return false;
            else if (_VisulizandoSave == null)
                return true;
            else if (_VisulizandoSave > Saves.Keys.Min())
                return true;
            else
                return false;
        }
        #endregion

        #region Redo
        public AraEvent<Action> ChangeCatRedo = new AraEvent<Action>();

        public void Redo()
        {
            if (_VisulizandoSave != null)
            {
                if (_VisulizandoSave < Saves.Keys.Max())
                    _VisulizandoSave = (int)_VisulizandoSave + 1;

                JSon = new  AraDesignJSon(Saves[(int)_VisulizandoSave]);
                if (_VisulizandoSave == Saves.Keys.Max())
                    _VisulizandoSave = null;

                if (ChangeCatRedo.InvokeEvent != null)
                    ChangeCatRedo.InvokeEvent();
            }
        }
        public bool CatRedo()
        {
            if (_VisulizandoSave != null && _VisulizandoSave < Saves.Keys.Max())
                return true;
            else
                return false;
        }
        #endregion


        #endregion

        #endregion

        #region Propertys
        DateTime UltSelected = DateTime.Now;
        public void this_StartEditPropertys(IAraDev vObj)
        {
            bool EventoSimultaneo = (DateTime.Now - UltSelected).TotalSeconds <= 1;

            if (EventoSimultaneo)
            {
                if (this.ObjetoSendoEditado.Length == 1)
                {
                    if (this.ObjetoSendoEditado[0].ConteinerFather.InstanceID == vObj.InstanceID)
                        return;
                }
            }


            AraEventMouse Mouse = new AraEventMouse();
            if (Mouse.shiftKey == true)
            {

                List<IAraDev> vTmpObj = new List<IAraDev>(this.ObjetoSendoEditado);
                vTmpObj.RemoveAll(a => a.InstanceID == vObj.InstanceID);

                if (Mouse.ctrlKey != true)
                {
                    if (vTmpObj.Count() == 0 || (vTmpObj[0].ConteinerFather.InstanceID == vObj.ConteinerFather.InstanceID))
                    {
                        if (!vTmpObj.Exists(a => a.InstanceID == vObj.InstanceID))
                            vTmpObj.Add(vObj);
                    }
                }
                this.ObjetoSendoEditado = vTmpObj.ToArray();

            }
            else
            {
                if (this.ObjetoSendoEditado.FirstOrDefault(a => a !=null && a.InstanceID == vObj.InstanceID) == null)
                    this.ObjetoSendoEditado = new IAraDev[] { vObj };
            }
            UltSelected = DateTime.Now;
        }

        public void this_ChangeProperty(IAraDev vObj)
        {
            CarregaGridPropriedades();
        }

        public AraEvent<Action<string[]>> OnObjetoSendoEditado = new AraEvent<Action<string[]>>(); 
        private List<AraObjectInstance<IAraDev>> _ObjetoSendoEditado = new List<AraObjectInstance<IAraDev>>();
        public IAraDev[] ObjetoSendoEditado
        {
            get { return _ObjetoSendoEditado.Select(a => a.Object).ToArray(); }
            set
            {
                if (_ObjetoSendoEditado.Count() > 0)
                {
                    foreach (IAraComponentVisual vTmp in _ObjetoSendoEditado.Where(a => a.Object != null).Select(a => (IAraComponentVisual)a.Object))
                    {
                        vTmp.CssRemoveClass("Ara2DevBordaTracejadaEditando");
                    }
                }

                _ObjetoSendoEditado = value.Select(a => new AraObjectInstance<IAraDev>(a)).ToList();
                _ObjetoSendoEditado.ForEach(a => ((IAraComponentVisual)a.Object).CssAddClass("Ara2DevBordaTracejadaEditando"));
                CarregaGridPropriedades();
                _ObjetoSendoEditado.ForEach(a => AtivaResizableDraggableObjeto((IAraComponentVisual)a.Object));

                if (OnObjetoSendoEditado.InvokeEvent != null)
                    OnObjetoSendoEditado.InvokeEvent(_ObjetoSendoEditado.Select(a =>(a.Object != this.Canvas?a.Object.Name: this.CanvasReal.Name)).ToArray());
            }
        }

        public void SelectAll()
        {
            List<IAraDev> vObjs = new List<IAraDev>();
            foreach(var vObj in this.Canvas.Childs.Where(a=>a is IAraDev).Select(a=>(IAraDev)a))
            {
                vObjs.Add(vObj);
            }

            ObjetoSendoEditado= vObjs.ToArray();
        }

        private void CarregaGridPropriedades()
        {

        }

        #endregion


        #region Edita Largura Altura
        private void AlterouLaguraAlturaCanvas()
        {
            this.CanvasReal.Width = this.Canvas.Width;
            this.CanvasReal.Height = this.Canvas.Height;
            if (this.ObjetoSendoEditado.Count() == 1)
            {
                ((IAraComponentVisual)this.ObjetoSendoEditado[0]).Width = Convert.ToInt32(((IAraComponentVisual)this.ObjetoSendoEditado[0]).Width.Value / 5) * 5;
                ((IAraComponentVisual)this.ObjetoSendoEditado[0]).Height = Convert.ToInt32(((IAraComponentVisual)this.ObjetoSendoEditado[0]).Height.Value / 5) * 5;

                if (this.ObjetoSendoEditado[0] == this.Canvas)
                    CarregaGridPropriedades();
            }

            PendeteSalvar = true;
            MainEdit.GetInstance().VS.Cliente.Channel(a => a.ChangeObjectsList());
        }

        private void AlterouLeftTopCanvas(object vObj, decimal OldLeft, decimal OldTop)
        {

            if (this.ObjetoSendoEditado.Count() > 0 && this.ObjetoSendoEditado[0]!=null)
            {
                if (this.ObjetoSendoEditado.Count() == 1)
                {
                    IAraComponentVisual ObjetoUnicoSendoEditado = (IAraComponentVisual)this.ObjetoSendoEditado[0];

                    ObjetoUnicoSendoEditado.Left = Math.Round(ObjetoUnicoSendoEditado.Left.Value, 5);
                    ObjetoUnicoSendoEditado.Top = Math.Round(ObjetoUnicoSendoEditado.Top.Value, 5);

                    if (this.ObjetoSendoEditado[0] == this.Canvas)
                        CarregaGridPropriedades();

                    if (ObjetoUnicoSendoEditado is AraComponentVisualAnchor)
                    {
                        ((AraComponentVisualAnchor)ObjetoUnicoSendoEditado).Anchor.Reflesh();
                    }
                }
                else
                {
                    IAraObject vObjRef = (IAraObject)vObj;
                    decimal Div_Left = ((IAraComponentVisualPositionLeftTop)vObjRef).Left.Value - OldLeft;
                    decimal Div_Top = ((IAraComponentVisualPositionLeftTop)vObjRef).Top.Value - OldTop;
                    foreach (IAraDev ObjetoUnicoSendoEditado in this.ObjetoSendoEditado)
                    {
                        if (ObjetoUnicoSendoEditado.InstanceID != vObjRef.InstanceID)
                        {
                            ObjetoUnicoSendoEditado.Left += Div_Left;
                            ObjetoUnicoSendoEditado.Top += Div_Top;
                        }
                    }
                }
            }

            PendeteSalvar = true;
            MainEdit.GetInstance().VS.Cliente.Channel(a => a.ChangeObjectsList());
        }
        #endregion


        private void Selectable_Stop(IAraObject[] vObjects)
        {
            IAraObject vP = vObjects[0];
            IAraDev[] vTmp = vObjects.Where(a => a.ConteinerFather.InstanceID == vP.ConteinerFather.InstanceID).Select(a => (IAraDev)a).ToArray();
            if (vTmp.Length > 0)
                this.ObjetoSendoEditado = vTmp;
        }

        public AraEvent<Action> ChangeObjectsList = new AraEvent<Action>();

        private string[] _ObjectNamesOld = new string[] { };
        private string[] GetObjectNames()
        {
            List<string> Names = new List<string>();
            Names.Add(this.CanvasReal.Name);

            foreach (IAraDev vObj in GetAllChilds(this.Canvas).OrderBy(a => a.Name))
                Names.Add(vObj.Name); //(vObj.Visible == false ? " (Oculto) " : "")

            return Names.ToArray();
        }

        public void CheckChangeObjectNames()
        {
            AraTools.AsynchronousFunction(CheckChangeObjectNamesSincy);
        }

        public void CheckChangeObjectNamesSincy()
        {
            var NewObjNames = GetObjectNames();
            bool Change = false;
            if (NewObjNames.Length != _ObjectNamesOld.Length)                    Change = true;
            else if (NewObjNames.Where(a => !_ObjectNamesOld.Contains(a)).Any()) Change = true;
            else if (_ObjectNamesOld.Where(a => !NewObjNames.Contains(a)).Any()) Change = true;

            if (Change)
            {
                if (ChangeObjectsList.InvokeEvent != null)
                    ChangeObjectsList.InvokeEvent();

                _ObjectNamesOld = NewObjNames;
            }
        }

        public IAraDev[] GetOjectsDevAll()
        {
            List<IAraDev> vTmpObjsDev = new List<IAraDev>();
            vTmpObjsDev.Add(this.CanvasReal);

            foreach (IAraDev vObj in GetAllChilds(this.Canvas))
                vTmpObjsDev.Add(vObj); //(vObj.Visible == false ? " (Oculto) " : "")

            return vTmpObjsDev.ToArray();
        }

        public IAraDev GetOjectsDev(string vName)
        {
            if (this.CanvasReal.Name == vName)
                return (IAraDev)this.CanvasReal;
            else
                return GetAllChilds(this.Canvas).Where(a => a.Name == vName).Single();
        }

        public IAraDev[] GetOjectsDev(string[] vName)
        {
            var CanvasRealName = this.CanvasReal.Name;
            var vArray = GetAllChilds(this.Canvas).Where(a => a.Name != CanvasRealName && vName.Contains(a.Name)).ToList();
            if (vName.Contains(CanvasRealName))
                vArray.Add((IAraDev)this.CanvasReal);
            return vArray.ToArray();
        }

        public new void Dispose()
        {
            References.Dispose();
            References = null;
        }

    }
}
