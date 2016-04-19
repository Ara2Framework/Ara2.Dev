using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Net;
using Ara2;
using System.Reflection;
using System.Linq;
using Ara2.Dev.VS.Tools;
using System.Windows.Forms;

namespace Tecnomips.Ara2_Dev_VS
{
    static class StartIISExpress
    {
        public static string Star(string AppPath)
        {
            string vPathExtencion = GetPathExtencion();
            string vPathWebServer = Path.Combine(vPathExtencion, "Resources", "ServidorWeb");

            //#region Cria pasta vPathWebServer
            //try
            //{
            //    if (!Directory.Exists(Path.Combine(vPathWebServer)))
            //        Directory.CreateDirectory(Path.Combine(vPathWebServer));
            //}
            //catch
            //{
            //    throw new Exception("Erro ao criar pasta '" + vPathWebServer + "'");
            //}
            //#endregion


            //if (!Directory.Exists(Path.Combine(vPathWebServer, "bin")))
            //    Directory.CreateDirectory(Path.Combine(vPathWebServer, "bin"));

            //ManifestResourceStreamToPathFile(Path.Combine(vPathWebServer, "Default.aspx"), "Tecnomips.Ara2_Dev_VS/Resources/ServidorWeb/" + "Default.aspx");
            //ManifestResourceStreamToPathFile(Path.Combine(vPathWebServer, "Web.config"), "Tecnomips.Ara2_Dev_VS/Resources/ServidorWeb/" + "Web.config");
            //ManifestResourceStreamToPathFile(Path.Combine(vPathWebServer, "Config.config"), "Tecnomips.Ara2_Dev_VS/Resources/ServidorWeb/" + "Config.config");
            //ManifestResourceStreamToPathFile(Path.Combine(vPathWebServer, "bin/Ara2.Dev.AraDesign.dll"), "Tecnomips.Ara2_Dev_VS/Resources/ServidorWeb/" + "bin/Ara2.Dev.AraDesign.dll");
            //ManifestResourceStreamToPathFile(Path.Combine(vPathWebServer, "bin/Ara2.Dev.AraDesign.Edit.dll"), "Tecnomips.Ara2_Dev_VS/Resources/ServidorWeb/" + "bin/Ara2.Dev.AraDesign.Edit.dll");
            //ManifestResourceStreamToPathFile(Path.Combine(vPathWebServer, "bin/Ara2.Dev.AraDesign.Edit.Service.dll"), "Tecnomips.Ara2_Dev_VS/Resources/ServidorWeb/" + "bin/Ara2.Dev.AraDesign.Edit.Service.dll");
            //ManifestResourceStreamToPathFile(Path.Combine(vPathWebServer, "bin/Ara2.dll"), "Tecnomips.Ara2_Dev_VS/Resources/ServidorWeb/" + "bin/Ara2.dll");
            //ManifestResourceStreamToPathFile(Path.Combine(vPathWebServer, "bin/Ara2.Dev.VS.Tools.dll"), "Tecnomips.Ara2_Dev_VS/Resources/ServidorWeb/" + "bin/Ara2.Dev.VS.Tools.dll");



            if (Config.Get.PathWebConfig != Path.Combine(vPathWebServer, "Web.config"))
                Config.Get.PathWebConfig = Path.Combine(vPathWebServer, "Web.config");

            int port = 0;
            Process process = null;
            string Url = null;
            bool ServidorRespondeu = false;
            bool PrecisaSalvar = false;

            if (!string.IsNullOrWhiteSpace(Config.Get["ProcessID"]) && !string.IsNullOrWhiteSpace(Config.Get["UltimaPorta"]))
            {
                try
                {
                    process = Process.GetProcessById(Convert.ToInt32(Config.Get["ProcessID"]));
                }
                catch { process = null; }

                if (process != null)
                {
                    port = Convert.ToInt32(Config.Get["UltimaPorta"]);
                    Url = GetUrl(port);
                    ServidorRespondeu = AgardaServidor(process, Url);
                    if (!ServidorRespondeu)
                    {
                        process = null;
                        port = 0;
                    }
                }
            }

            if (process == null)
            {
                if (!string.IsNullOrWhiteSpace(Config.Get["port"]))
                {
                    try
                    {
                        port = Convert.ToInt32(Config.Get["port"]);
                    }
                    catch { throw new Exception("Port invalid!"); }
                }
                else
                    port = (new Random()).Next(6000, 8000);


                string IIS_EXPRESS;
                if (string.IsNullOrWhiteSpace(Config.Get["IISExpressExe"]))
                {
                    IIS_EXPRESS = Path.Combine(System.Environment.GetEnvironmentVariable("programfiles"), "IIS Express\\iisexpress.exe");

                    if (!File.Exists(IIS_EXPRESS))
                    {
                        IIS_EXPRESS = Path.Combine(System.Environment.GetEnvironmentVariable("programfiles(x86)"), "IIS Express\\iisexpress.exe");

                        if (!File.Exists(IIS_EXPRESS))
                        {
                            string vMsgErro = "Favor instalar o IIS Express em '" + Path.Combine(System.Environment.GetEnvironmentVariable("programfiles"), "IIS Express\\iisexpress.exe") + "'";
                            //MessageBox.Show(vMsgErro);
                            throw new Exception(vMsgErro);
                        }
                            
                    }
                }
                else
                {
                    IIS_EXPRESS = Config.Get["IISExpressExe"];
                    if (!File.Exists(IIS_EXPRESS))
                        throw new Exception("IISExpressExe not found \"" + IIS_EXPRESS + "\"");
                }

                process = new Process
                    {
                    StartInfo = new ProcessStartInfo
                        {
                            FileName = IIS_EXPRESS,
                            //WindowStyle = ProcessWindowStyle.Hidden,
                            ErrorDialog = false,
                            LoadUserProfile = true,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            Arguments = string.Format("/path:\"{0}\" /port:{1}", vPathWebServer, port),
                            Verb = "runas",
                            //RedirectStandardOutput = true
                        }
                    };
                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_ErrorDataReceived;
                process.Start();

                Url = GetUrl(port);
                AddLog("Url '" + Url + "'");
                ServidorRespondeu = AgardaServidor(process, Url);

                if (ServidorRespondeu)
                {
                    Config.Get["ProcessID"] = process.Id.ToString();
                    Config.Get["UltimaPorta"] = port.ToString();
                }
                else
                    throw new Exception("IIS Express não presponde. Vide Log:'" + FileLog + "'");
   
            }
            

            return GetUrl(port);
        }

        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            AddLog("IIS Error: " + e.Data);
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            AddLog("IIS: " + e.Data);
        }


        #region Aguarda aplicação ficar inativa ou servidor web fechar
        public static void AguardaAplicaçãoFicarInativa()
        {
            //AddLog("Aguarda aplicação ficar inativa ou servidor web fechar");
            //DateTime? SemRespotaOuSemusuarios = null;
            //do
            //{
            //    Thread.Sleep(5000);
            //    int? NUsuarios = GetAraMemoryAreaCount(Url);
            //    if (NUsuarios == null || NUsuarios == 0)
            //    {
            //        if (SemRespotaOuSemusuarios == null)
            //        {
            //            AddLog("NUsuarios " + NUsuarios);
            //            SemRespotaOuSemusuarios = DateTime.Now;
            //        }
            //    }
            //    else
            //    {
            //        if (SemRespotaOuSemusuarios != null)
            //        {
            //            AddLog("NUsuarios " + NUsuarios);
            //            SemRespotaOuSemusuarios = null;
            //        }
            //    }

            //} while (process.IsRunning() && (SemRespotaOuSemusuarios == null || ((DateTime.Now - (DateTime)SemRespotaOuSemusuarios).TotalMinutes <= Convert.ToInt32(Config.Get["MinutosFecharSemUsuarios"]))));

            //#region Tenta Matar o processo
            //if (process.IsRunning())
            //{
            //    AddLog("Fechado por timeout de " + (DateTime.Now - (DateTime)SemRespotaOuSemusuarios).TotalMinutes + " minutos");

            //    try
            //    {
            //        process.Kill();
            //    }
            //    catch { }
            //}
            //else
            //{
            //    AddLog("Fechado porque o processo parou");
            //}
            //#endregion
        }
        #endregion


        public static void ManifestResourceStreamToPathFile(string vPath, string vPathManifestResourceName, Assembly asm = null)
        {
            if (!File.Exists(vPath))
            {
                if (asm == null)
                    asm = Assembly.GetExecutingAssembly();

                Stream stream = asm.GetManifestResourceStream(vPathManifestResourceName.Replace("/", "."));

                using (StreamWriter streamW = new StreamWriter(vPath))
                {
                    stream.CopyTo(streamW.BaseStream);
                }
            }
        }

        public static string GetPathExtencion()
        {
            return Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        }

        public static string FileLog = null;
        public static void AddLog(string vMsg)
        {
            if (FileLog == null)
                FileLog = Path.Combine(GetPathExtencion(), "StartIIS.log");

            lock(FileLog)
            {
                using (StreamWriter streamW = new StreamWriter(FileLog, true))
                {
                    streamW.WriteLine(DateTime.Now.ToString("yyyy-mm-dd hh-MM:ss") + " " + vMsg);
                }
            }
        }

        public static string GetUrl(int Port)
        {
            return "http://localhost:" + Port + @"/";
        }

        private static bool AgardaServidor(Process process, string vUrl)
        {
            int? vResposta = null;
            int Ntentativas = 0;
            do
            {
                vResposta = GetAraMemoryAreaCount(vUrl);

                if (vResposta != null)
                    return true;

                Thread.Sleep(500);
                Ntentativas++;

                if (process.IsRunning() == false && Ntentativas >= 5)
                    return false;
            } while (true);
        }

        public static int? GetAraMemoryAreaCount(string Url)
        {
            string vURL = URLCombine(Url, "?AraMemoryAreaCount=1");

            try
            {


                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(vURL);
                myRequest.Timeout = 6000;
                myRequest.Method = "GET";
                string result;
                try
                {
                    using (WebResponse myResponse = myRequest.GetResponse())
                    {
                        using (StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
                catch (WebException wex)
                {
                    result = new StreamReader(wex.Response.GetResponseStream())
                                          .ReadToEnd();
                }

                AddLog("GetAraMemoryAreaCount Resposta = " + IsIntOrNull(result) + " '" + result + "'");

                return IsIntOrNull(result);
            }
            catch (Exception err)
            {

                AddLog("GetAraMemoryAreaCount Erro '" + err.Message + "' URL '" + vURL + "'");

                return null;
            }
        }

        private static int? IsIntOrNull(string vtext)
        {
            try
            {
                int vTmp = Convert.ToInt32(vtext.Trim());
                return (int?)vTmp;
            }
            catch { return null; }
        }


        private static string URLCombine(string baseUri, params string[] relativeOrAbsoluteUri)
        {
            Uri vUri = new Uri(baseUri);
            foreach (string vTmp in relativeOrAbsoluteUri)
                vUri = new Uri(vUri, vTmp);

            return vUri.ToString();
        }

        public static void LoadMemoryToFirstCustomer(string vURL)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(vURL);
                myRequest.Timeout = 6000;
                myRequest.Method = "GET";
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();
            }
            catch { }
        }
    }

    static public class Processo
    {
        public static bool IsRunning(this Process process)
        {
            try { Process.GetProcessById(process.Id); }
            catch (InvalidOperationException) { return false; }
            catch (ArgumentException) { return false; }
            return true;
        }
    }
}
