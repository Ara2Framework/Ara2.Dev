using System;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using tom;
using Ara2.Dev.AraDesign.Edit.Service;
using System.IO;
using Ara2.Dev.AraDesign;
using System.Reflection;
using System.Text;

namespace Tecnomips.Ara2_Dev_VS
{
    public partial class MyEditor : UserControl
    {
        //private VSMacroRecorder m_Recorder;
        public ClienteServerChannel<IPakServerAraDevEdit, PakVisualStudio, IPakVisualStudio> ServiceHost;
        public EditorPane EditorPane;
        public AraDesignProjectReferences ProjectReferences;

        public MyEditor(EditorPane vEditorPane)
        {
            EditorPane = vEditorPane;

            InitializeComponent();
            Web.PreviewKeyDown += Web_PreviewKeyDown;
            
        }

        DateTime? UtmWeb_PreviewKeyDown = null;

        private bool DuploKeyDown()
        {
            if (UtmWeb_PreviewKeyDown == null || (DateTime.Now - (DateTime)UtmWeb_PreviewKeyDown).TotalMilliseconds >= 500)
            {
                UtmWeb_PreviewKeyDown = DateTime.Now;
                return false;
            }
            else
                return true;
                
        }

        void Web_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            lock(this)
            {
                if (e.Control)
                {
                    if (e.KeyValue == 'c' || e.KeyValue == 'C')
                    {
                        if (!DuploKeyDown())
                            EditorPane.onCopy();
                    }
                    else if (e.KeyValue == 'v' || e.KeyValue == 'V')
                    {
                        if (!DuploKeyDown())
                            EditorPane.onPaste();
                    }
                    else if (e.KeyValue == 'x' || e.KeyValue == 'X')
                    {
                        if (!DuploKeyDown())
                            EditorPane.onCut();
                    }
                    else if (e.KeyValue == 'a' || e.KeyValue == 'A')
                    {
                        if (!DuploKeyDown())
                            EditorPane.onSelectAll();
                    }
                }
                else if (e.KeyData == Keys.Delete)
                {
                    if (!DuploKeyDown())
                        EditorPane.onDelete();
                }
            }
        }
        

        private string _FileProject = string.Empty;

        public string FileProject
        {
            get { return _FileProject; }
            set
            {
                _FileProject = value;
                ProjectReferences = new AraDesignProjectReferences(new FileInfo(_FileProject));
                EditorPane.InitializeToolBox();
            }
        }

        private string _File = string.Empty;
        public string File
        {
            get { return _File; }
            set
            {
                if (value.Length < ".AraDesign".Length)
                    throw new Exception("File not '.AraDesign'");
                _File = value;
                _FileCs = File.Substring(0, File.Length - ".AraDesign".Length) + ".cs";
                _FileAraDesignCs = File + ".cs";


                if (ServiceHost == null)
                    ServiceHost = new ClienteServerChannel<IPakServerAraDevEdit, PakVisualStudio, IPakVisualStudio>(new PakVisualStudio(this), NewRandowPort(), NewRandowPort());

                //#if (!DEBUG)
                var vUrl = StartIISExpress.Star(Path.GetDirectoryName(FileProject));
                //#else
                //var vUrl = "http://localhost:45374/";
                //#endif


                string vUrlTmp = vUrl + "?FileProject=" + FileProject + "&File=" + _File + "&AraDevEditPort=" + ServiceHost.Cliente.Porta + "&VSPort=" + ServiceHost.Server.Porta;
                Web.Url = new Uri(vUrlTmp);
            }
        }

        private string _FileAraDesignCs;
        public string FileAraDesignCs
        {
            get
            {
                return _FileAraDesignCs;
            }
        }

        private string _FileCs;
        public string FileCs
        {
            get
            {
                return _FileCs;
            }
        }



        private static Random _random = new Random(Environment.TickCount);
        private static int NewRandowPort()
        {
            for (int n = 0; n < 10; n++)
            {
                var vPort = _random.Next(6000, 6999);

                try
                {
                    System.Net.Sockets.TcpListener vTmpSerevr = new System.Net.Sockets.TcpListener(vPort);
                    vTmpSerevr.Start();
                    vTmpSerevr.Stop();
                    return vPort;
                }
                catch
                {
                    System.Threading.Thread.Sleep(100);
                }
            }

            throw new Exception("Tcp Randow Port 6000 to 6999 not Listener");
        }

        public void Save()
        {
            ServiceHost.Cliente.Channel(a => a.Save());
            var vFileJson = (new AraDesignJSonBuid(new FileInfo(FileProject), new FileInfo(File)));
            System.IO.File.WriteAllText(FileAraDesignCs, vFileJson.Compile());

            this.EditorPane.FileCsAddReferenceProject(FileAraDesignCs);
            if (!System.IO.File.Exists(FileCs))
                System.IO.File.WriteAllText(FileCs, GetFrmTemplate1(vFileJson));
            this.EditorPane.FileCsAddReferenceProject(FileCs);
            
        }

        private string GetFrmTemplate1(IAraDesignJSon vJson)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            bool ContrutorTemIAraContainerClient = vJson.Canvas.GetTypeObject(ProjectReferences).GetConstructor(new Type[] { typeof(Ara2.Components.IAraContainerClient) }) != null;
            bool ContrutorTemSession = vJson.Canvas.GetTypeObject(ProjectReferences).GetConstructor(new Type[] { typeof(Ara2.Session) }) != null;

            string ConstructParan;
            if (ContrutorTemIAraContainerClient) ConstructParan = "IAraContainerClient ConteinerFather";
            else if (ContrutorTemSession) ConstructParan = "Ara2.Session Session";
            else ConstructParan = "";

            string ConstructBaseParan;
            if (ContrutorTemIAraContainerClient) ConstructBaseParan = "ConteinerFather";
            else if (ContrutorTemSession) ConstructBaseParan = "Session";
            else ConstructBaseParan = "";


            return (new StreamReader(asm.GetManifestResourceStream("Tecnomips.Ara2_Dev_VS.Templates.FrmTemplate1.Tcs"))).ReadToEnd()
                    .Replace("{NameSpace}", EditorPane.CanvasProperties.NameSpace)
                    .Replace("{ClassName}", EditorPane.CanvasProperties.ClassName)
                    .Replace("{NameSpaceAraDesign}", EditorPane.CanvasProperties.NameSpaceAraDesign)
                    .Replace("{ClassNameAraDesign}", EditorPane.CanvasProperties.ClassNameAraDesign)
                    .Replace("{ConstructParan}", ConstructParan)
                    .Replace("{ConstructBaseParan}", ConstructBaseParan)
                    ;
        }

        public void Save(string vFile)
        {
            ServiceHost.Cliente.Channel(a => a.SaveAs(vFile));
        }

        public void Close()
        {
            ProjectReferences.Dispose();
            ProjectReferences = null;
            ServiceHost.Cliente.Channel(a => a.Close());
        }

        bool _ReadOnly = false;
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set { _ReadOnly = value; }
        }

        public event Action ChangePendingToSave;
        public void ChangePendingToSaveInvoke()
        {
            ChangePendingToSave();
        }

        /// <summary>
        /// This value is used internally so that we know what to display on the status bar.
        /// NOTE: Setting this value will not actually change the insert/overwrite behavior
        /// of the editor, it is just used so that we can keep track of the state internally.
        /// </summary>
        //private bool overstrike;
        public bool PendingToSave
        {
            get
            {
                try
                {
                    return ServiceHost.Cliente.Channel(a => a.GetPendingToSave());
                }
                catch
                {
                    return true;
                }
            }
        }

        public void SelectEditObject(string[] vNames)
        {
            ServiceHost.Cliente.Channel(a => a.SelectEditObject(vNames));
        }

        public new void Dispose()
        {
            try
            {
                EditorPane.Dispose();
            }
            catch { }
            EditorPane = null;
            try
            {
                ProjectReferences.Dispose();
            }
            catch { }
            ProjectReferences = null;
            base.Dispose();
        }

        ~MyEditor()
        {
            this.Dispose();
        }
    }
}
