using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Ara2;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;

namespace Ara2.Dev.VS.Tools
{
    public class Config
    {
        public string this[string vParam]
        {
            get
            {
                string vTmp = "";
                lock (this)
                {

                    try
                    {
                        System.Configuration.Configuration config = GetWebConfiguration();
                        if (config.AppSettings.Settings.AllKeys.Contains(vParam))
                            vTmp = config.AppSettings.Settings[vParam].Value;
                        config = null;
                    }
                    catch { }
                }

                return vTmp;
            }

            set
            {
                lock (this)
                {
                    System.Configuration.Configuration config = GetWebConfiguration();
                    if (config.AppSettings.Settings.AllKeys.Contains(vParam))
                        config.AppSettings.Settings[vParam].Value = value;
                    else
                        config.AppSettings.Settings.Add(vParam, value);
                    //config.AppSettings.SectionInformation.ForceSave = true;
                    config.Save();
                    config = null;
                }
            }
        }


        public string PathWebConfig = null;

        private System.Configuration.Configuration GetWebConfiguration()
        {
            System.Configuration.Configuration _Config = null;
            if (_Config == null)
            {
                if (PathWebConfig == null)
                {
                    try
                    {
                        _Config = WebConfigurationManager.OpenWebConfiguration("~");
                    }
                    catch
                    {
                        _Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                    }
                }
                else
                {
                    
                    VirtualDirectoryMapping vdm = new VirtualDirectoryMapping(PathWebConfig, true);
                    
                    WebConfigurationFileMap wcfm = CreateFileMap();
                    //wcfm.VirtualDirectories.Add("/", vdm);

                    _Config = WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/");
                }

            }
            return _Config;
        }

        // Utility to map virtual directories to physical ones.
        // In the current physical directory maps 
        // a physical sub-directory with its virtual directory.
        // A web.config file is created for the
        // default and the virtual directory at the appropriate level.
        // You must create a physical directory called config at the
        // level where your app is running.
        private WebConfigurationFileMap CreateFileMap()
        {

            WebConfigurationFileMap fileMap =
                   new WebConfigurationFileMap();

            // Get he physical directory where this app runs.
            // We'll use it to map the virtual directories
            // defined next. 
            string physDir = Path.GetDirectoryName(PathWebConfig);

            // Create a VirtualDirectoryMapping object to use
            // as the root directory for the virtual directory
            // named config. 
            // Note: you must assure that you have a physical subdirectory
            // named config in the curremt physical directory where this
            // application runs.
            VirtualDirectoryMapping vDirMap =
                new VirtualDirectoryMapping(physDir + "\\config", true);

            // Add vDirMap to the VirtualDirectories collection 
            // assigning to it the virtual directory name.
            fileMap.VirtualDirectories.Add("/config", vDirMap);

            // Create a VirtualDirectoryMapping object to use
            // as the default directory for all the virtual 
            // directories.
            VirtualDirectoryMapping vDirMapBase =
                new VirtualDirectoryMapping(physDir, true, "web.config");

            // Add it to the virtual directory mapping collection.
            fileMap.VirtualDirectories.Add("/", vDirMapBase);

            # if DEBUG  
            // Test at debug time.
            foreach (string key in fileMap.VirtualDirectories.AllKeys)
            {
                Console.WriteLine("Virtual directory: {0} Physical path: {1}",
                fileMap.VirtualDirectories[key].VirtualDirectory,
                fileMap.VirtualDirectories[key].PhysicalDirectory);
            }
            # endif 

            // Return the mapping.
            return fileMap;

        }

        public string ParamPath(string vParam)
        {
            string vTmp = this[vParam];
            if (vTmp.IndexOf("$P$") != -1)
                return vTmp.Replace("$P$", AraTools.GetPath().Substring(0, AraTools.GetPath().Length - 1));
            else
                return vTmp;
        }

        public object GetObject(string vParan,Type TypeObjectBase)
        {
            if (this[vParan] != null && this[vParan] != "")
            {
                XmlSerializer ser = new XmlSerializer(TypeObjectBase);
                return ser.Deserialize( new StringReader(this[vParan]));
            }
            else
                return null;
        }

        public void SetObject(string vParan, Type TypeObjectBase,object vObject)
        {
            XmlSerializer ser = new XmlSerializer(TypeObjectBase);
            StringWriter TW = new StringWriter();
            ser.Serialize(TW,vObject);
            this[vParan] = TW.ToString();
        }


        private static Config _Get = null;
        public static Config Get
        {
            get
            {
                if (_Get == null)
                    _Get = new Config();

                return _Get;
            }
        }

    }
}