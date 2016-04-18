using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Ara2.Dev.AraDesign.Tools;

namespace Ara2.Dev.AraDesign
{
    public class AraDesignProjectReferences:IDisposable
    {
        public List<Type> Components = new List<Type>();
        public List<Assembly> Assemblys = new List<Assembly>();

        public AraDesignProjectReferences(FileInfo FileProject)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            try
            {
                Assemblys = new List<Assembly>();

                string vFileProject = FileProject.FullName;

                var PastaProjeto = Path.GetDirectoryName(vFileProject);

                XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
                XDocument projDefinition = XDocument.Load(vFileProject);

                var ProjectProject = projDefinition.Element(msbuild + "Project");
                var ProjectOutputPaths = ProjectProject.Elements(msbuild + "OutputPath");


                foreach (System.Xml.Linq.XElement Tmp2 in ProjectProject.Elements(msbuild + "ItemGroup").Elements())
                {
                    string vAcao = Tmp2.Name.LocalName;
                    if (vAcao == "Reference")
                    {
                        if (Tmp2.Element(msbuild + "HintPath") != null)
                        {
                            //string vTmpPathFile = Tmp2.Attribute("Include").Value;
                            string vDll = Path.Combine(PastaProjeto, Tmp2.Element(msbuild + "HintPath").Value);
                            if (File.Exists(vDll))
                                AddDll(ref Assemblys, vDll);
                        }

                    }
                    else if (vAcao == "ProjectReference")
                    {
                        string vTmpPathFile = Tmp2.Attribute("Include").Value;
                        string vCsProj = (new FileInfo(Path.Combine(PastaProjeto, vTmpPathFile))).FullName;

                        XNamespace msbuildSubProject = "http://schemas.microsoft.com/developer/msbuild/2003";
                        XDocument projDefinitionSubProject = XDocument.Load(vCsProj);
                        System.Xml.Linq.XElement ProjectSubProject = projDefinitionSubProject.Element(msbuildSubProject + "Project");
                        System.Xml.Linq.XElement ProjectSubProjectPropertyGroup = ProjectSubProject.Element(msbuildSubProject + "PropertyGroup");
                        string OutputType = ProjectSubProjectPropertyGroup.Element(msbuildSubProject + "OutputType").Value;
                        string vAssemblyName = ProjectSubProjectPropertyGroup.Element(msbuildSubProject + "AssemblyName").Value;
                        var OutputPaths = ProjectSubProjectPropertyGroup.Elements(msbuildSubProject + "OutputPath");

                        if (OutputType == "Library")
                        {
                            string PastaSubProjeto = Path.GetDirectoryName(vCsProj);

                            //foreach(var OutputPath in OutputPaths)
                            //{
                            //    string vDll = Path.Combine(vCsProj, OutputPath.Value, vAssemblyName + ".dll");
                            //    if (File.Exists(vDll))
                            //    {

                            //    }
                            //}

                            ////string vNameProject = Tmp2.Element(msbuild + "Name").Value;
                            string vDll = Path.Combine(PastaSubProjeto, "bin", "Debug", vAssemblyName + ".dll");
                            if (!File.Exists(vDll))
                            {
                                if (!Directory.Exists(Path.Combine(PastaSubProjeto, "bin")))
                                    Directory.CreateDirectory(Path.Combine(PastaSubProjeto, "bin"));
                                if (!Directory.Exists(Path.Combine(PastaSubProjeto, "bin", "Debug")))
                                    Directory.CreateDirectory(Path.Combine(PastaSubProjeto, "bin", "Debug"));

                                try
                                {
                                    if (Config.Get["FW"] == null || Config.Get["FW"].Trim() == "")
                                        Config.Get["FW"] = "v4.0.30319";

                                    string vFWVercion = Config.Get["FW"];

                                    string vMsBuild = string.Format(@"{0}\Microsoft.NET\Framework\{1}\msbuild.exe", Environment.GetFolderPath(Environment.SpecialFolder.Windows), vFWVercion);

                                    if (File.Exists(vMsBuild))
                                    {
                                        Process p = new Process();
                                        p.StartInfo = new ProcessStartInfo(vMsBuild);
                                        p.StartInfo.Arguments = vCsProj + @" /p:OutputPath=bin\Debug "; ///noconsolelogger
                                        p.StartInfo.UseShellExecute = false;
                                        p.StartInfo.CreateNoWindow = true;
                                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                        p.Start();

                                        p.WaitForExit();

                                        if (File.Exists(vDll))
                                            AddDll(ref Assemblys, vDll);
                                    }
                                    else
                                        throw new Exception("msbuild not found '" + vMsBuild + "'.\n Config.xml FW= '" + vFWVercion + "'");
                                }
                                catch (Exception err)
                                {
                                    throw new Exception("Erro compile Project  '" + vAssemblyName + "'.\n" + err.Message);
                                }
                            }
                            else
                                AddDll(ref Assemblys, vDll);
                        }
                    }
                }

                foreach (var vAs in Assemblys)
                {
                    try
                    {
                        foreach (var vTmp in vAs.GetExportedTypes())
                        {
                            try
                            {
                                if (vTmp.GetCustomAttributes(typeof(AraDevComponent), true).Any())
                                    Components.Add(vTmp);
                            }
                            catch (Exception err)
                            {
                                Debug.Print("Erro 1 " + err.Message);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        Debug.Print("Erro 2 " + err.Message);
                    }
                }

            }
            finally
            {
                //Assemblys.Clear();
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            }
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var vTmp = Assemblys.Where(a => a.GetName().Name == (new AssemblyName(args.Name)).Name).FirstOrDefault();
            if (vTmp != null)
                return vTmp;
            else
                throw new Exception(args.Name + " not found");
        }

        private void AddDll(ref List<Assembly> vAssembly, string vDll)
        {
            try
            {
                var vAs = Assembly.LoadFile(vDll);
                if (vAs.GetName().Name != "Ara2")
                {
                    if (!vAssembly.Exists(a => a != null && a.FullName == vAs.FullName))
                        vAssembly.Add(vAs);
                }
                else
                    vAssembly.Add(Assembly.Load("Ara2"));
            }
            catch { }
        }

        public void  Dispose()
        {
            Components.Clear();
            Assemblys.Clear();
        }
    }
}
