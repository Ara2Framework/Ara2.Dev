
/*
    NÂO ALTERAR ESTE ARQUIVO SEM O EDITOR ARA.DEV !
    DO NOT CHANGE THIS FILE WITHOUT THE EDITOR ARA.DEV!

 _   _          ____             _   _______ ______ _____            _____    ______  _____ _______ ______            _____   ____  _    _ _______      ______  
| \ | |   /\   / __ \      /\   | | |__   __|  ____|  __ \     /\   |  __ \  |  ____|/ ____|__   __|  ____|     /\   |  __ \ / __ \| |  | |_   _\ \    / / __ \ 
|  \| |  /  \ | |  | |    /  \  | |    | |  | |__  | |__) |   /  \  | |__) | | |__  | (___    | |  | |__       /  \  | |__) | |  | | |  | | | |  \ \  / / |  | |
| . ` | / /\ \| |  | |   / /\ \ | |    | |  |  __| |  _  /   / /\ \ |  _  /  |  __|  \___ \   | |  |  __|     / /\ \ |  _  /| |  | | |  | | | |   \ \/ /| |  | |
| |\  |/ ____ \ |__| |  / ____ \| |____| |  | |____| | \ \  / ____ \| | \ \  | |____ ____) |  | |  | |____   / ____ \| | \ \| |__| | |__| |_| |_   \  / | |__| |
|_| \_/_/    \_\____/  /_/    \_\______|_|  |______|_|  \_\/_/    \_\_|  \_\ |______|_____/   |_|  |______| /_/    \_\_|  \_\\___\_\\____/|_____|   \/   \____/ 
                                                                                                                                                                

Ara2.Dev 1.0

*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ara2;
using Ara2.Components;


namespace Ara2.Dev.Teste.NewFolder1.AraDesign
{
    [Serializable]
    public abstract class DefaultAraDesign : Ara2.Components.WindowMain
    {
    
       #region Objects
       private AraObjectInstance<Ara2.Components.AraButton> _A0O12 = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton A0O12
       {
          get { return _A0O12.Object; }
          set { _A0O12.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _A0O22 = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel A0O22
       {
          get { return _A0O22.Object; }
          set { _A0O22.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraTextBox> _A0O31 = new AraObjectInstance<Ara2.Components.AraTextBox>();
       public Ara2.Components.AraTextBox A0O31
       {
          get { return _A0O31.Object; }
          set { _A0O31.Object = value; }
       }
       #endregion 
       #region Events
       #endregion 
        public DefaultAraDesign(Ara2.Session vSession)
            : base(vSession)
        {
            #region Instances
            #region Propertys Main
            this.Name  = @"Default";
            this.Width  =  new Ara2.Components.AraDistance(@"290px");
            this.Height  =  new Ara2.Components.AraDistance(@"248px");
            #endregion
            
            
            #region A0O12
            this.A0O12 = new Ara2.Components.AraButton(this);

            this.A0O12.Name = "A0O12";
            this.A0O12.Text  = @" OK";
            this.A0O12.Name  = @"A0O12";
            this.A0O12.Left  =  new Ara2.Components.AraDistance(@"183px");
            this.A0O12.Top  =  new Ara2.Components.AraDistance(@"211px");
            this.A0O12.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.A0O12.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
            this.A0O12.Width  =  new Ara2.Components.AraDistance(@"90px");
            this.A0O12.Height  =  new Ara2.Components.AraDistance(@"25px");
            #endregion
            #region A0O22
            this.A0O22 = new Ara2.Components.AraLabel(this);

            this.A0O22.Name = "A0O22";
            this.A0O22.Text  = @"Nome:";
            this.A0O22.Name  = @"A0O22";
            this.A0O22.Left  =  new Ara2.Components.AraDistance(@"6px");
            this.A0O22.Top  =  new Ara2.Components.AraDistance(@"14px");
            this.A0O22.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.A0O22.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
            this.A0O22.Width  =  new Ara2.Components.AraDistance(@"55px");
            this.A0O22.Height  =  new Ara2.Components.AraDistance(@"17px");
            #endregion
            #region A0O31
            this.A0O31 = new Ara2.Components.AraTextBox(this);

            this.A0O31.Name = "A0O31";
            this.A0O31.Name  = @"A0O31";
            this.A0O31.Left  =  new Ara2.Components.AraDistance(@"61px");
            this.A0O31.Top  =  new Ara2.Components.AraDistance(@"11px");
            this.A0O31.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.A0O31.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
            this.A0O31.Width  =  new Ara2.Components.AraDistance(@"150px");
            this.A0O31.Height  =  new Ara2.Components.AraDistance(@"17px");
            #endregion
            #endregion
        } 
    } 
} 
