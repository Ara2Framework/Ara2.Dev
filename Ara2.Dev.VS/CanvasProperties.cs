using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Globalization;
using Ara2.Dev.AraDesign.Edit.Service;

namespace Tecnomips.Ara2_Dev_VS
{
    [Serializable]
    [DisplayName("Canvas")]
    public class CanvasProperties: ICanvasProperties, ICustomTypeDescriptor
    {
        private EditorPane editor;


        #region  ICustomTypeDescriptor
        public string GetClassName()
        {
            return Name;
        }
        
        public string GetComponentName()
        {
            return "CanvasProperties";
        }


        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(this,attributes,true);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(this,true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
        #endregion


        public readonly string Name = "Canvas";

        public override string ToString()
        {
            return this.Name;
        }

        public CanvasProperties(EditorPane Editor)
        {
            editor = Editor;
        }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        //public string FileName
        //{
        //    get { return editor.FileName; }
        //}

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        //public bool DataChanged
        //{
        //    get { return editor.DataChanged; }
        //}

        [Browsable(true)]
        public string NameSpace
        {
            get
            {
                return editor.editorControl.ServiceHost.Cliente.Channel(a => a.GetNameSpace());
            }
            set
            {
                editor.editorControl.ServiceHost.Cliente.Channel(a => a.SetNameSpace(value));
            }
        }

        [Browsable(true)]
        public string ClassName
        {
            get
            {
                return editor.editorControl.ServiceHost.Cliente.Channel(a => a.GetClassName());
            }
            set
            {
                editor.editorControl.ServiceHost.Cliente.Channel(a => a.SetClassName(value));
            }
        }

        [Browsable(true)]
        public string NameSpaceAraDesign
        {
            get
            {
                return editor.editorControl.ServiceHost.Cliente.Channel(a => a.GetNameSpaceAraDesign());
            }
            set
            {
                editor.editorControl.ServiceHost.Cliente.Channel(a => a.SetNameSpaceAraDesign(value));
            }
        }


        [Browsable(true)]
        public string ClassNameAraDesign
        {
            get
            {
                return editor.editorControl.ServiceHost.Cliente.Channel(a => a.GetClassNameAraDesign());
            }
            set
            {
                editor.editorControl.ServiceHost.Cliente.Channel(a => a.SetClassNameAraDesign(value));
            }
        }


        #region Collection will display as a ComboBox in PropertyGrid on the fly.
        public class TypeCanasPropertyConverter : TypeConverter
        {

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                //true means show a combobox
                return true;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                //true will limit to list. false will show the list, but allow free-form entry
                return true;
            }

            public override
                System.ComponentModel.TypeConverter.StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                var vEditor = ((CanvasProperties)context.Instance).editor;
                 

                List<Type> strCollection = new List<Type>();

                foreach (var vC in vEditor.editorControl.ProjectReferences.Components.Where(a => a.GetCustomAttributes(false).Where(b => b is Ara2.Dev.AraDevComponent && ((Ara2.Dev.AraDevComponent)b).Base == true).Any()))
                {
                    strCollection.Add(vC);
                }

                return new StandardValuesCollection(strCollection);
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                    return sourceType == typeof(string);
                else
                    return false;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value.GetType() == typeof(string))
                {
                    if ((string)value != string.Empty)
                    {
                        var vEditor = ((CanvasProperties)context.Instance).editor;
                        return vEditor.editorControl.ProjectReferences.Components.Where(a => a.ToString() == (string)value).FirstOrDefault();
                    }
                    else
                        return string.Empty;
                }
                else
                    return value;
            }

            public override object ConvertTo(ITypeDescriptorContext context,CultureInfo culture,object value,Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    if (value != null)
                        if (((dynamic)value).ToString() == "Ara2.Dev.AraDesign.Edit.Service.CWindowMain" )
                            return "Ara2.Components.WindowMain";
                        else
                            return value.ToString();
                    else
                        return string.Empty;
                }
                return 
                    base.ConvertTo(context, culture, value, destinationType);
            }

        }
        #endregion 

        [Browsable(true)]
        [TypeConverter(typeof(TypeCanasPropertyConverter))]
        public Type TypeCanas
        {
            get
            {
                var vNameTypeCananvas = editor.editorControl.ServiceHost.Cliente.Channel(a => a.GetTypeCananvas());
                if (vNameTypeCananvas != "Ara2.Dev.AraDesign.Edit.Service.CWindowMain")
                    return editor.editorControl.ProjectReferences.Components.Where(a => a.ToString() == vNameTypeCananvas).FirstOrDefault();
                else
                    return typeof(Ara2.Dev.AraDesign.Edit.Service.CWindowMain);
            }
            set
            {
                editor.editorControl.ServiceHost.Cliente.Channel(a => a.SetTypeCananvas(value.AssemblyQualifiedName));
            }
        }

    }
}
