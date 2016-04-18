
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


namespace Ara2.Dev.Teste.FrmTeste.AraDesign
{
    [Serializable]
    public abstract class TesteAraDesign : Ara2.Components.WindowMain
    {
    
       #region Objects
       private AraObjectInstance<Ara2.Components.AraButton> _A0O13 = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton A0O13
       {
          get { return _A0O13.Object; }
          set { _A0O13.Object = value; }
       }
       #endregion 
       #region Events
       #endregion 
        public TesteAraDesign()
            : base()
        {
            #region Instances
            #region Propertys Main
            this.Name  = @"teste";
            this.Visible  = false;
            this.Width  =  new Ara2.Components.AraDistance(@"300px");
            this.Height  =  new Ara2.Components.AraDistance(@"300px");
            #endregion
            
            
            #region A0O13
            this.A0O13 = new Ara2.Components.AraButton(this);

            this.A0O13.Name = "A0O13";
            this.A0O13.Name  = @"A0O13";
            this.A0O13.Left  =  new Ara2.Components.AraDistance(@"84px");
            this.A0O13.Top  =  new Ara2.Components.AraDistance(@"120px");
            this.A0O13.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.A0O13.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
            this.A0O13.Width  =  new Ara2.Components.AraDistance(@"90px");
            this.A0O13.Height  =  new Ara2.Components.AraDistance(@"25px");
            #endregion
            #endregion
        } 
    } 
} 
