using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ara2.Dev.AraDesign
{
    public class AraDesignJSonBuid :AraDesignJSon, IAraDesignJSon 
    {
        FileInfo FileProject;
        IAraDesignJSonBuidCanvas Canvas { get; set; }

        public AraDesignJSonBuid(FileInfo vFileProject, FileInfo JsonScript)
            : base(File.ReadAllText(JsonScript.FullName))
        {
            FileProject = vFileProject;

            _ProjectReferences = new AraDesignProjectReferences(FileProject);

            try
            {

                Canvas = new AraDesignJSonBuidCanvas(this,JSon.Canvas, Projeto);
            }
            catch (Exception err)
            {
                throw new Exception("Error load json.\n" + err.Message);
            }

            _ProjectReferences.Dispose();
            _ProjectReferences = null;
        }

        public IEnumerable<IAraDesignJSonBuidCanvasPropertys> GetListPropertys(IAraDesignJSonFather vFather, dynamic vTmp)
        {
            List<IAraDesignJSonBuidCanvasPropertys> vTmpP = new List<IAraDesignJSonBuidCanvasPropertys>();
            foreach (dynamic vProperty in vTmp)
            {

                vTmpP.Add(new AraDesignJSonBuidCanvasPropertys(ProjectReferences, vFather, vProperty));
            }

            return vTmpP;
        }

        public IEnumerable<IAraDesignJSonBuidCanvasChildren> GetListChildren(IAraDesignJSonFather vFather, dynamic vTmp)
        {
            List<IAraDesignJSonBuidCanvasChildren> vTmpC = new List<IAraDesignJSonBuidCanvasChildren>();
            foreach (dynamic Children in vTmp)
            {
                vTmpC.Add(new AraDesignJSonBuidCanvasChildren(this,vFather, Children));
            }

            return vTmpC;
        }

        public IEnumerable<IAraDesignJSonBuidCanvasChildren> GetChildsMany()
        {
            List<IAraDesignJSonBuidCanvasChildren> vTmp = new List<IAraDesignJSonBuidCanvasChildren>();
            vTmp.AddRange(Canvas.Children.OrderBy(a => Convert.ToInt32((a.Propertys.Where(b => b.Name == "Index").FirstOrDefault() != null && a.Propertys.Where(b => b.Name == "Index").FirstOrDefault().Value != null ? a.Propertys.Where(b => b.Name == "Index").FirstOrDefault().Value : "-1"))).ToArray());

            foreach (var vC in Canvas.Children)
                vTmp.AddRange(GetChildsMany(vC));

            return vTmp;
        }

        private static IEnumerable<IAraDesignJSonBuidCanvasChildren> GetChildsMany(IAraDesignJSonBuidCanvasChildren vFather)
        {
            
            List<IAraDesignJSonBuidCanvasChildren> vTmp = new List<IAraDesignJSonBuidCanvasChildren>();
            if (vFather.Children !=null && vFather.Children.Any())
            {
                vTmp.AddRange(vFather.Children.OrderBy(a => Convert.ToInt32((a.Propertys.Where(b => b.Name == "Index").FirstOrDefault() != null && a.Propertys.Where(b => b.Name == "Index").FirstOrDefault().Value != null ? a.Propertys.Where(b => b.Name == "Index").FirstOrDefault().Value : "-1"))).ToArray());
                foreach (var vC in vFather.Children)
                    vTmp.AddRange(GetChildsMany(vC));
            }
            return vTmp;
        }

        public bool Analysis = false;

        AraDesignProjectReferences _ProjectReferences=null;
        public AraDesignProjectReferences ProjectReferences
        {
            get
            {
                return _ProjectReferences;
            }
        }

        public string Compile()
        {
            StringBuilder vTmp = new StringBuilder();

            _ProjectReferences = new AraDesignProjectReferences(FileProject);


            List<string> ObjectLayoutsReander = new List<string>();

            vTmp.AppendLine(@"");
            vTmp.AppendLine(@"/*");
            vTmp.AppendLine(@"    NÂO ALTERAR ESTE ARQUIVO SEM O EDITOR ARA.DEV !");
            vTmp.AppendLine(@"    DO NOT CHANGE THIS FILE WITHOUT THE EDITOR ARA.DEV!");
            vTmp.AppendLine(@"");
            vTmp.AppendLine(@" _   _          ____             _   _______ ______ _____            _____    ______  _____ _______ ______            _____   ____  _    _ _______      ______  ");
            vTmp.AppendLine(@"| \ | |   /\   / __ \      /\   | | |__   __|  ____|  __ \     /\   |  __ \  |  ____|/ ____|__   __|  ____|     /\   |  __ \ / __ \| |  | |_   _\ \    / / __ \ ");
            vTmp.AppendLine(@"|  \| |  /  \ | |  | |    /  \  | |    | |  | |__  | |__) |   /  \  | |__) | | |__  | (___    | |  | |__       /  \  | |__) | |  | | |  | | | |  \ \  / / |  | |");
            vTmp.AppendLine(@"| . ` | / /\ \| |  | |   / /\ \ | |    | |  |  __| |  _  /   / /\ \ |  _  /  |  __|  \___ \   | |  |  __|     / /\ \ |  _  /| |  | | |  | | | |   \ \/ /| |  | |");
            vTmp.AppendLine(@"| |\  |/ ____ \ |__| |  / ____ \| |____| |  | |____| | \ \  / ____ \| | \ \  | |____ ____) |  | |  | |____   / ____ \| | \ \| |__| | |__| |_| |_   \  / | |__| |");
            vTmp.AppendLine(@"|_| \_/_/    \_\____/  /_/    \_\______|_|  |______|_|  \_\/_/    \_\_|  \_\ |______|_____/   |_|  |______| /_/    \_\_|  \_\\___\_\\____/|_____|   \/   \____/ ");
            vTmp.AppendLine(@"                                                                                                                                                                ");
            vTmp.AppendLine(@"");
            vTmp.AppendLine(@"Ara2.Dev 1.0");
            vTmp.AppendLine(@"");
            vTmp.AppendLine(@"*/");
            vTmp.AppendLine(@"");
            // Para Escrever com estas letras grandes
            // http://www.network-science.de/ascii/ 
            vTmp.AppendLine(@"");
            vTmp.AppendLine(@"");
            vTmp.AppendLine(@"using System;");
            vTmp.AppendLine(@"using System.Collections.Generic;");
            vTmp.AppendLine(@"using System.Linq;");
            vTmp.AppendLine(@"using System.Web;");
            vTmp.AppendLine(@"using Ara2;");
            vTmp.AppendLine(@"using Ara2.Components;");
            vTmp.AppendLine(@"");
            vTmp.AppendLine(@"");

            string vNameClass = this.Canvas.Propertys.Where(a => a.Name == "Name").First().Value;

            vTmp.AppendLine(@"namespace " + this.Canvas.NameSpaceAraDesign);
            vTmp.AppendLine(@"{");
            vTmp.AppendLine(@"    [Serializable]");
            vTmp.AppendLine(@"    public abstract class " + this.Canvas.ClassNameAraDesign + @" : " + this.Canvas.TypeName + @"");
            vTmp.AppendLine(@"    {");
            vTmp.AppendLine(@"    ");

            #region Objects
            vTmp.AppendLine("       #region Objects");
            foreach (var vObj in this.GetChildsMany())
            {
                string vName = vObj.Propertys.Where(a => a.Name == "Name").First().Value;
                vTmp.AppendLine(@"       private AraObjectInstance<" + vObj.TypeName + @"> _" + vName + @" = new AraObjectInstance<" + vObj.TypeName + @">();");
                vTmp.AppendLine(@"       public " + vObj.TypeName + @" " + vName + @"");
                vTmp.AppendLine(@"       {");
                vTmp.AppendLine(@"          get { return _" + vName + @".Object; }");
                vTmp.AppendLine(@"          set { _" + vName + @".Object = value; }");
                vTmp.AppendLine(@"       }");
            }
            vTmp.AppendLine("       #endregion ");
            #endregion

            #region Events
            vTmp.AppendLine("       #region Events");
            foreach (var vProperty in this.GetChildsMany().SelectMany(a => a.Propertys).Where(a => a.Event == true && a.Value != null && a.Value != ""))
            {
                List<string> vParans = new List<string>();

                foreach (var Paran in vProperty.EventParans)
                {
                    vParans.Add(Paran.ValueType + " " + Paran.Name);
                }

                vTmp.AppendLine("       public abstract " + vProperty.EventReturnTypeName + " " + vProperty.Value + "(" + string.Join(",", vParans) + ");");
            }
            vTmp.AppendLine("       #endregion ");
            #endregion

            bool ContrutorTemIAraContainerClient = this.Canvas.GetTypeObject(ProjectReferences).GetConstructor(new Type[] { typeof(Ara2.Components.IAraContainerClient) }) != null;
            bool ContrutorTemSession = this.Canvas.GetTypeObject(ProjectReferences).GetConstructor(new Type[] { typeof(Ara2.Session) }) != null;

            if (ContrutorTemIAraContainerClient)
            {
                vTmp.AppendLine("        public " + this.Canvas.ClassNameAraDesign + @"(IAraContainerClient vConteiner)");
                vTmp.AppendLine("            : base(vConteiner)");
            }
            else if (ContrutorTemSession)
            {
                vTmp.AppendLine("        public " + this.Canvas.ClassNameAraDesign + @"(Ara2.Session vSession)");
                vTmp.AppendLine("            : base(vSession)");
            }
            else
            {
                vTmp.AppendLine("        public " + this.Canvas.ClassNameAraDesign + @"()");
                vTmp.AppendLine("            : base()");
            }
            
            vTmp.AppendLine("        {");
            if (Analysis)
                vTmp.Append("           Tick.GetTick().Script.CustomerAnalysisBegin(\"" + vNameClass + "\"); ");

            vTmp.AppendLine("            #region Instances");


            vTmp.AppendLine("            #region Propertys Main");

            //if (Analysis)
            //    vTmp.AppendLine("            Tick.GetTick().Script.CustomerAnalysisAddArea(\"" + vNameClass + "\", \"Main\",\"" + GetTypeToString(CanvasReal.GetType()) + "\", () => {");

            foreach (var vProperty in this.Canvas.Propertys.Where(a => a.IsDefault == false))
            {
                vTmp.AppendLine("            this." + vProperty.Name + " " + vProperty.GetSriptAtrubuicaoPropriedade(ProjectReferences) + ";");
                if (vProperty.Name == "LayoutsString" && vProperty.Value != null)
                {
                    vTmp.AppendLine("            this.Layouts.NoSave = true;");
                    ObjectLayoutsReander.Add("this");
                }
            }

            if (Analysis)
                vTmp.AppendLine("            });");

            vTmp.AppendLine("            #endregion"); //Propertys Main
            vTmp.AppendLine("            ");
            vTmp.AppendLine("            ");


            foreach (var vObj in this.GetChildsMany())
            {
                vTmp.AppendLine("            #region " + vObj.Name + "");

                //if (Analysis)
                //    vTmp.AppendLine("            Tick.GetTick().Script.CustomerAnalysisAddArea(\"" + vNameClass + "\", \"" + vObj.Name + "\",\"" + GetTypeToString(vObj.GetType()) + "\", () => {");

                string vNameConteinerFather = (vObj.Father() == this.Canvas ? "this" : "this." + vObj.Father().Name);
                vTmp.AppendLine("            this." + vObj.Name + " = new " + vObj.GetTypeToString(ProjectReferences) + "(" + vNameConteinerFather + ");\n");
                vTmp.AppendLine("            this." + vObj.Name + ".Name = \"" + vObj.Name + "\";");

                foreach (var vProperty in vObj.Propertys.Where(a => a.IsDefault == false && ((a.Event == true && a.Value != null && a.Value != "") || a.Event == false)))
                {
                    vTmp.AppendLine("            this." + vObj.Name + "." + vProperty.Name + " " + vProperty.GetSriptAtrubuicaoPropriedade(ProjectReferences) + ";");
                    if (vProperty.Name == "LayoutsString" && vProperty.Value != null)
                    {
                        vTmp.AppendLine("           this." + vObj.Name + ".Layouts.NoSave = true;");
                        ObjectLayoutsReander.Add("this." + vObj.Name);
                    }
                }

                if (Analysis)
                    vTmp.AppendLine("            });");

                vTmp.AppendLine("            #endregion"); // Object
            }

            if (ObjectLayoutsReander.Count() > 0)
            {
                vTmp.AppendLine("            ");
                vTmp.AppendLine("            ");
                vTmp.AppendLine("            #region Layouts Reander");
                foreach (string vNameObj in ObjectLayoutsReander)
                {
                    vTmp.AppendLine("            " + vNameObj + ".Layouts.Render();");
                }
                vTmp.AppendLine("            #endregion"); // Layouts Reander
            }

            vTmp.AppendLine("            #endregion"); // Instances

            if (Analysis)
                vTmp.AppendLine("            Tick.GetTick().Script.CustomerAnalysisEnd(\"" + vNameClass + "\");");

            vTmp.AppendLine("        } "); // End Construct



            vTmp.AppendLine("    } ");// End Class
            vTmp.AppendLine("} ");// End NameSpace

            _ProjectReferences.Dispose();
            _ProjectReferences = null;

            return vTmp.ToString();
        }
    }
}
