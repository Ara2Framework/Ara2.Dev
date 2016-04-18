using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.ComponentModel;
using Ara2.Dev.AraDesign.Edit.Service;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Tecnomips.Ara2_Dev_VS
{
    

    public static class AraObjectInstanceGetObjectInstanceCustom
    {
        #region AraObjectInstance GetObjectInstanceCustom
        public static object AraObjectInstance_GetObjectInstanceCustom(string IdInstance)
        {
            if (vEditorPaneByThead.ContainsKey(System.Threading.Thread.CurrentThread.ManagedThreadId))
            {
                EditorPane EditorPane;
                if (vEditorPaneByThead.TryGetValue(System.Threading.Thread.CurrentThread.ManagedThreadId, out EditorPane))
                {
                    var vBs = EditorPane.editorControl.ServiceHost.Cliente.Channel<byte[]>(a => a.GetObjectByIdInstance(IdInstance));

                    BinaryFormatter bf = new BinaryFormatter();
                    var vObj = bf.Deserialize(new MemoryStream(vBs));
                    return vObj;
                }
                else
                    throw new Exception("EventGetObjectInstanceCustom não chamado");
            }
            else if (OldGetObjectInstanceCustom != null)
                return OldGetObjectInstanceCustom(IdInstance);
            else
                throw new Exception("EventGetObjectInstanceCustom não chamado");
        }

        private static Dictionary<int, EditorPane> vEditorPaneByThead = new Dictionary<int, EditorPane>();
        public static void EventGetObjectInstanceCustom(ref EditorPane vEditorPane, Action vEvent)
        {
            EventGetObjectInstanceCustom<object>(ref vEditorPane, () => { vEvent(); return null; });
        }

        private static Func<string,object> OldGetObjectInstanceCustom =null;
        public static object EventGetObjectInstanceCustom<T>(ref EditorPane vEditorPane,Func<T> vEvent)
        {
            if (Ara2.Components.AraObjectInstanceStatic.GetObjectInstanceCustom == null)
            {
                if (Ara2.Components.AraObjectInstanceStatic.GetObjectInstanceCustom != AraObjectInstance_GetObjectInstanceCustom)
                    OldGetObjectInstanceCustom = Ara2.Components.AraObjectInstanceStatic.GetObjectInstanceCustom;
                else
                    OldGetObjectInstanceCustom = null;

                Ara2.Components.AraObjectInstanceStatic.GetObjectInstanceCustom += AraObjectInstance_GetObjectInstanceCustom;
            }

            EditorPane vEditorPaneTmp;
            if (!vEditorPaneByThead.TryGetValue(System.Threading.Thread.CurrentThread.ManagedThreadId, out vEditorPaneTmp))
            {

                lock (vEditorPaneByThead)
                {
                    vEditorPaneByThead.Add(System.Threading.Thread.CurrentThread.ManagedThreadId, vEditorPane);
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
                }
            }
            else
                return vEvent();
        }
        #endregion
    }

    public class DynamicProxy : DynamicObject, ICustomTypeDescriptor
    {

        #region protected methods
        protected PropertyInfo GetPropertyInfo(string propertyName)
        {
            return ProxiedObject.GetType().GetProperties().First
            (propertyInfo => propertyInfo.Name == propertyName);
        }

        protected virtual void SetMember(string propertyName, object value)
        {
            GetPropertyInfo(propertyName).SetValue(ProxiedObject, value, null);
        }

        protected virtual object GetMember(string propertyName)
        {
            return GetPropertyInfo(propertyName).GetValue(ProxiedObject, null);
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            try
            {
                result = this.GetProperties().Find(binder.Name, true).GetValue(this);
                //result = ProxiedObject.GetType().GetProperty(binder.Name).GetValue(ProxiedObject);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {

            try
            {
                //ProxiedObject.GetType().GetProperty(binder.Name).SetValue(ProxiedObject, value);

                this.GetProperties().Find(binder.Name, true).SetValue(this, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion


        #region constructor
        public PropertyDescriptorTeste Pai = null;
        public DynamicProxy PaiDPMaster = null;
        public string FullName;
        public DynamicProxy(object proxiedObject, EditorPane vEditorPane, PropertyDescriptorTeste vPai , DynamicProxy vPaiDPMaster)
        {
            EditorPane = vEditorPane;
            ProxiedObject = proxiedObject;
            Pai = vPai;
            PaiDPMaster = vPaiDPMaster;

            FullName = string.Join(".", GetNameFull());
        }

        public DynamicProxy(object proxiedObject, EditorPane vEditorPane)
        {
            
            EditorPane = vEditorPane;
            ProxiedObject = proxiedObject;

            FullName = string.Join(".", GetNameFull());
        }
        #endregion

        #region public properties
        public EditorPane EditorPane;
        public object _ProxiedObject = null;
        public object ProxiedObject
        {
            get
            {
                return AraObjectInstanceGetObjectInstanceCustom.EventGetObjectInstanceCustom<object>(ref EditorPane, () =>
                     {

                         if (EditorPane.Objects == null)
                             return _ProxiedObject;

                         if (!IsSubPorperty())
                         {
                             var vTmpName = (string)(((dynamic)_ProxiedObject).Name);
                             var vObjNewObjP = EditorPane.Objects.Where(a => ((dynamic)a._ProxiedObject).Name == vTmpName && a._ProxiedObject != _ProxiedObject).Select(a => a._ProxiedObject).FirstOrDefault();
                             if (vObjNewObjP != null)
                                 return vObjNewObjP;
                             else
                                 return _ProxiedObject;
                         }
                         else
                         {
                             var vTmpNamePai = PaiDPMaster.Name;
                             var vObjNew = EditorPane.Objects.Where(a => ((dynamic)a._ProxiedObject).Name == vTmpNamePai).FirstOrDefault();
                             if (vObjNew._ProxiedObject != PaiDPMaster._ProxiedObject)
                             {
                                 PropertyDescriptor vTmpP = null;
                                 dynamic vValueTmp = vObjNew;
                                 foreach (var vNameP in FullName.Split('.').Skip(1))
                                 {
                                     vTmpP = vValueTmp.GetProperties().Find(vNameP, false);
                                     vValueTmp = vTmpP.GetValue(vValueTmp);
                                 }

                                 return ((DynamicProxy)vValueTmp)._ProxiedObject;
                             }
                             else
                                 return _ProxiedObject;
                         }
                     });
            }
            set
            {
                _ProxiedObject = value;
            }
        }
        #endregion

        public string Name
        {
            get
            {
                try
                {
                    return (string)(((dynamic)ProxiedObject).Name);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                var vPName = GetProperties()["Name"];
                vPName.SetValue(this, value);
                //((dynamic)ProxiedObject).Name = value;
            }
        }

        #region Implementation of ICustomTypeDescriptor

        public PropertyDescriptorCollection GetProperties()
        {
            return this.GetProperties(null);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var vTmpP = TypeDescriptor.GetProperties(ProxiedObject, attributes);
            PropertyDescriptor[] pds = new PropertyDescriptor[vTmpP.Count];

            int i = 0;
            foreach (PropertyDescriptor vTmp in vTmpP)
            {
                pds[i] = new PropertyDescriptorTeste(this, vTmp, attributes);
                i++;
            }

            return new PropertyDescriptorCollection(pds);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            //return TypeDescriptor.GetProperties(ProxiedObject);
            return this.GetProperties(attributes);
        }

        

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public string GetClassName()
        {
            return ProxiedObject.GetType().Name;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return ((dynamic)ProxiedObject).Name;
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            //return AttributeCollection.Empty;
            return TypeDescriptor.GetAttributes(ProxiedObject);
        }
        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(ProxiedObject);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(ProxiedObject);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(ProxiedObject);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            //throw new NotSupportedException();
            //return null;
            return TypeDescriptor.GetEditor(ProxiedObject, editorBaseType);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            //return EventDescriptorCollection.Empty;
            return TypeDescriptor.GetEvents(ProxiedObject);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            //return EventDescriptorCollection.Empty;
            return TypeDescriptor.GetEvents(ProxiedObject, attributes);
        }
        #endregion

        public string ToString()
        {
            return ((dynamic)ProxiedObject).ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is DynamicProxy)
            {
                return ((DynamicProxy)obj).ProxiedObject == this.ProxiedObject;
                /*
                return ((DynamicProxy)obj).Name == this.Name 
                        && ((DynamicProxy)obj).Pai == this.Pai
                        && ((DynamicProxy)obj).GetClassName() == this.GetClassName();
                 */
            }
            else
                return base.Equals(obj);
        }

        private bool IsSubPorperty()
        {
            return this.Pai != null;
        }

        private IEnumerable<string> GetNameFull()
        {
            List<string> vPropertName = new List<string>();
            PropertyDescriptorTeste DPPai = this.Pai;
            while (DPPai != null)
            {
                vPropertName.Add(DPPai.Name);
                DPPai = DPPai.ObjectDP.Pai;
            }
            if (PaiDPMaster == null)
                vPropertName.Add(this.Name);
            else
                vPropertName.Add(PaiDPMaster.Name);
            vPropertName.Reverse();

            return vPropertName;
        }

    }

    public class PropertyDescriptorTeste : PropertyDescriptor
    {
        public DynamicProxy ObjectDP;
        public DynamicProxy ObjectDPMaster;
        private PropertyDescriptor property;
        private object Value = null;
        private bool SetValueInternal = false;
        private IEnumerable<string> FullNameProperty;

        public PropertyDescriptorTeste(DynamicProxy vObjectDP, PropertyDescriptor target, Attribute[] attrs)
            : base(target, attrs)
        {
            ObjectDP = vObjectDP;
            this.property = target;

            if (vObjectDP.Pai != null)
            {
                ObjectDPMaster = vObjectDP.Pai.ObjectDP;
                while (ObjectDPMaster.Pai != null)
                    ObjectDPMaster = vObjectDP.Pai.ObjectDP;
            }
            else
                ObjectDPMaster = ObjectDP;

            FullNameProperty = GetNameFull(ObjectDP);
        }

        public virtual TypeConverter Converter
        {
            get
            {
                return property.Converter;
            }
        }


        public override bool CanResetValue(object component)
        {
            return property.CanResetValue(component);
        }

        public override Type ComponentType
        {
            get { return property.ComponentType; }
        }



        public override bool IsReadOnly
        {
            get { return property.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return this.property.PropertyType; }
        }

        public override void ResetValue(object component)
        {
            property.ResetValue(component);
            // Not relevant.
        }

        public override bool ShouldSerializeValue(object component)
        {
            try
            {
                return this.property.ShouldSerializeValue(component);
            }
            catch
            {
                return true;
            }
        }

        //private string GetNameObject()
        //{
        //    if (IsSubPorperty())
        //    {
        //        PropertyDescriptorTeste DPPai = ObjectDP.Pai;
        //        while (DPPai.ObjectDP.Pai != null)
        //        {
        //            DPPai = DPPai.ObjectDP.Pai;
        //        }

        //        return DPPai.ObjectDP.Name;
        //    }
        //    else
        //        return ObjectDP.Name;
        //}

        private bool IsSubPorperty()
        {
            return ObjectDP.Pai!=null;
        }

        private IEnumerable<string> GetNameFull(object component)
        {
            var vObj = ((Tecnomips.Ara2_Dev_VS.DynamicProxy)(component));
            return GetNameFull(vObj);
        }

        private IEnumerable<string> GetNameFull(DynamicProxy vObj)
        {
            List<string> vPropertName = new List<string>();
            vPropertName.Add(property.Name);
            PropertyDescriptorTeste DPPai = vObj.Pai;
            while (DPPai != null)
            {
                vPropertName.Add(DPPai.Name);
                DPPai = DPPai.ObjectDP.Pai;
            }
            vPropertName.Reverse();

            return vPropertName;
        }

        public override object GetValue(object component)
        {
            var vObj = ((Tecnomips.Ara2_Dev_VS.DynamicProxy)(component));

            return AraObjectInstanceGetObjectInstanceCustom.EventGetObjectInstanceCustom<object>(ref vObj.EditorPane, () =>
            {
                var vObjNew = vObj.EditorPane.Objects.Where(a => a.Name == ObjectDPMaster.Name).FirstOrDefault();

                if (ObjectDPMaster != vObjNew)
                {
                    if (!IsSubPorperty())
                    {
                        var vProp = vObjNew.GetProperties().Find(this.property.Name, false);
                        if (vProp != null)
                            return vProp.GetValue(vObjNew);
                        else
                            throw new Exception("GetValue Falhou porque a propriedade " + this.Name + " não foi encontrada no objeto novo.");
                    }
                    else
                    {
                        PropertyDescriptor vTmpP = null;
                        dynamic vValueTmp = vObjNew;
                        foreach (var vNameP in FullNameProperty)
                        {
                            vTmpP = vValueTmp.GetProperties().Find(vNameP, false);
                            vValueTmp = vTmpP.GetValue(vValueTmp);
                        }

                        return vValueTmp;
                    }
                }
                else
                {
                    Tecnomips.Ara2_Dev_VS.DynamicProxy vComponent = ((Tecnomips.Ara2_Dev_VS.DynamicProxy)(component));

                    object vobj = property.GetValue(vComponent.ProxiedObject);
                    if (vobj != null && vobj.GetType().IsClass && vobj.GetType() != typeof(string))
                        return new Tecnomips.Ara2_Dev_VS.DynamicProxy(vobj, vComponent.EditorPane, this, ObjectDPMaster);
                    else
                        return vobj;
                }
            });

        }

        

        public override void SetValue(object component, object value)
        {
            byte[] vTmp2;
            bool IsNull;
            if (value != null)
            {
                IsNull = false;
                using (MemoryStream fs = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, (value is DynamicProxy ? ((DynamicProxy)value).ProxiedObject : value));
                    vTmp2 = fs.ToArray();
                }
            }
            else
            {
                IsNull = true;
                vTmp2 = new byte[] { 0 };
            }

            var vObj = ((Tecnomips.Ara2_Dev_VS.DynamicProxy)(component));

            string NomeObjeto = ObjectDPMaster.Name;
            string NomePropriedade = string.Join(".", GetNameFull(vObj));


            //EditorPane.WaitReturnSetValueAll();
            var vReturnSetValue = EditorPane.GetNewReturnSetValue();

            vObj.EditorPane.editorControl.ServiceHost.Cliente.Channel(a=>a.SetValue(vReturnSetValue.ID, NomeObjeto, NomePropriedade, vTmp2, IsNull));

            var Dateini = DateTime.Now;
            while (!vReturnSetValue.FinallySetValueEnd)
            {
                Application.DoEvents();
                if ((DateTime.Now - Dateini).TotalMinutes > 1)
                    throw new TimeoutException();
            }

            Exception Err = vReturnSetValue.FinallySetValueError;
            EditorPane.FinallySetValueClear(vReturnSetValue.ID);

            if (Err != null)
                throw Err;

            Application.DoEvents();
        }

        
    }

}
