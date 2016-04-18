using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ara2.Dev.AraDesign
{
    [Serializable]
    public class AraDesignJSonBuidCanvasPropertys : AraDesignJSonCanvasPropertys, IAraDesignJSonBuidCanvasPropertys
    {
        public AraDesignJSonBuidCanvasPropertys(AraDesignProjectReferences ProjectReferences, IAraDesignJSonFather vFather, dynamic vProperty)
            : base(vFather, (object)vProperty)
        {
            Type vTypeObject = this.Father().GetTypeObject(ProjectReferences);

            CDadosProperty Dados=null;
            if (this.Name.IndexOf(".") == -1)
                Dados = GetDadosProperty(vTypeObject, this.Name, this.Value);
            else
            {  
                foreach (var vPerfName in this.Name.Split('.'))
                {
                    Dados = GetDadosProperty((Dados == null ? vTypeObject : Dados.ValueTypeObject), vPerfName, this.Value);                   
                }
            }

            Event = Dados.Event;
            ValueTypeObject = Dados.ValueTypeObject;
            IsDefault = Dados.IsDefault;
            EventParans = Dados.EventParans;
            EventReturnTypeName = Dados.EventReturnTypeName;

            //throw new Exception("AraDesignJSonBuidCanvasPropertys '" + (vTypeObject!=null ?vTypeObject.Name + ".":"") + this.Name + "' not found.");
        }

        private static CDadosProperty GetDadosProperty(Type vTypeObject, string vName, string vValue)
        {
            foreach (System.Reflection.PropertyInfo vP in vTypeObject.GetProperties().Where(a => a.GetCustomAttributes(typeof(AraDevProperty), true).Any()
                                                                  || a.GetCustomAttributes(typeof(AraDevEvent), true).Any()))
            {
                if (vName == vP.Name)
                {
                    if (vP.GetCustomAttributes(typeof(AraDevProperty), true).Any())
                    {
                        AraDevProperty At = (AraDevProperty)vP.GetCustomAttributes(typeof(AraDevProperty), true)[0];

                        return new CDadosProperty()
                        {
                            Event = false,
                            ValueTypeObject = vP.PropertyType,
                            IsDefault = (vValue == null ? At.ValueDefault == null : vValue.Equals((At.ValueDefault == null ? "" : At.ValueDefault).ToString()))
                        };
                    }
                    else if (vP.GetCustomAttributes(typeof(AraDevEvent), true).Any())
                    {
                        MethodInfo Met = (vP.PropertyType.GetProperty("InvokeEvent").PropertyType).GetMethod("Invoke");

                        List<AraDesignJSonBuidCanvasPropertysEventParan>  TmpEventParans = new List<AraDesignJSonBuidCanvasPropertysEventParan>();
                        foreach (ParameterInfo Paran in Met.GetParameters())
                        {
                            TmpEventParans.Add(new AraDesignJSonBuidCanvasPropertysEventParan() { ValueType = GetTypeToString(Paran.ParameterType), Name = Paran.Name });
                        }

                        return new CDadosProperty()
                        {
                            Event = true,
                            IsDefault = string.IsNullOrEmpty(vValue),
                            ValueTypeObject = vP.PropertyType,
                            EventReturnTypeName = GetTypeToString(Met.ReturnType),
                            EventParans = TmpEventParans
                        };
                    }
                    else
                        throw new Exception("PropertyInfo não previsto");

                }
            }

            foreach (System.Reflection.FieldInfo vField in vTypeObject.GetFields().Where(a => a.GetCustomAttributes(typeof(AraDevProperty), true).Any()
                                                                  || a.GetCustomAttributes(typeof(AraDevEvent), true).Any()))
            {
                if (vName == vField.Name)
                {
                    if (vField.GetCustomAttributes(typeof(AraDevProperty), true).Any())
                    {
                        AraDevProperty At = (AraDevProperty)vField.GetCustomAttributes(typeof(AraDevProperty), true)[0];

                        return new CDadosProperty()
                        {
                            Event = false,
                            ValueTypeObject = vField.FieldType,
                            IsDefault = (vValue == null ? At.ValueDefault == null : vValue.Equals((At.ValueDefault == null ? "" : At.ValueDefault).ToString()))
                        };
                    }
                    else if (vField.GetCustomAttributes(typeof(AraDevEvent), true).Any())
                    {
                        MethodInfo Met = (vField.FieldType.GetProperty("InvokeEvent").PropertyType).GetMethod("Invoke");

                        List<AraDesignJSonBuidCanvasPropertysEventParan>  TmpEventParans = new List<AraDesignJSonBuidCanvasPropertysEventParan>();

                        foreach (ParameterInfo Paran in Met.GetParameters())
                        {
                            TmpEventParans.Add(new AraDesignJSonBuidCanvasPropertysEventParan() { ValueType = GetTypeToString(Paran.ParameterType), Name = Paran.Name });
                        }


                        return new CDadosProperty()
                        {
                            Event = true,
                            IsDefault = string.IsNullOrEmpty(vValue),
                            ValueTypeObject = vField.FieldType,
                            EventReturnTypeName = GetTypeToString(Met.ReturnType),
                            EventParans = TmpEventParans
                        };

                        
                    }
                }
            }


            throw new Exception("GetDadosProperty '" + vTypeObject.Name + "' not found.");
        }

        private class CDadosProperty
        {
            public bool Event ;
            public Type ValueTypeObject ;
            public bool IsDefault ;
            public List<AraDesignJSonBuidCanvasPropertysEventParan> EventParans;
            public string EventReturnTypeName;
        }

        private static bool isClass(Type vType)
        {
            return vType.IsClass && vType != typeof(string);
        }

        #region GetTypeToString
        private static string GetTypeToString(Type vType)
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

        public Type ValueTypeObject { get; set; }
        public bool Event { get; set; }
        public string EventReturnTypeName { get; set; }
        public List<AraDesignJSonBuidCanvasPropertysEventParan> EventParans { get; set; }
        public bool IsDefault { get; set; }
        //public object ValueObject { get; set; }

        public string GetSriptAtrubuicaoPropriedade(AraDesignProjectReferences ProjectReferences)
        {
            if (this.Event)
                return " += " + this.Value;
            else
            {
                if (this.ValueTypeObject.IsEnum)
                    return " = " + (this.Value == null ? "null" : this.ValueTypeObject.DeclaringType.FullName + "." + this.ValueTypeObject.Name + "." + this.Value);
                else if (this.ValueTypeObject == typeof(bool) || this.ValueTypeObject == typeof(bool?))
                    return " = " + (this.Value == null ? "null" : (Convert.ToBoolean(this.Value) ? "true" : "false"));
                else if (this.ValueTypeObject == typeof(string))
                    return " = @\"" + this.Value.ToString().Replace("\"", "\"\"") + "\"";
                else if (
                    this.ValueTypeObject == typeof(int) || this.ValueTypeObject == typeof(Int16) || this.ValueTypeObject == typeof(Int64) ||
                    this.ValueTypeObject == typeof(int?) || this.ValueTypeObject == typeof(Int16?) || this.ValueTypeObject == typeof(Int64?) ||
                    this.ValueTypeObject == typeof(decimal) || this.ValueTypeObject == typeof(decimal?) ||
                    this.ValueTypeObject == typeof(float) || this.ValueTypeObject == typeof(float?) ||
                    this.ValueTypeObject == typeof(double) || this.ValueTypeObject == typeof(double?)
                    )
                    return " = " + (this.Value == null ? "null" : Convert.ToDecimal(this.Value).ToString().Replace(".", "").Replace(",", "."));
                else if (this.Value == null)
                    return " =  null ";
                else
                    return " =  new " + this.ValueTypeObject.FullName + "(@\"" + this.Value.ToString().Replace("\"", "\"\"") + "\")";
            }
        }
    }

    public interface IAraDesignJSonBuidCanvasPropertys : IAraDesignJSonCanvasPropertys
    {
        Type ValueTypeObject { get; set; }
        bool Event { get; set; }
        string EventReturnTypeName { get; set; }
        List<AraDesignJSonBuidCanvasPropertysEventParan> EventParans { get; set; }
        bool IsDefault { get; set; }
        //object ValueObject { get; set; }
        

        string GetSriptAtrubuicaoPropriedade(AraDesignProjectReferences ProjectReferences);
    }
}
