// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14


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


namespace AraDesign
{
  [Serializable]
  public abstract class FrmLayoutAraDesign : Ara2.Components.AraWindow
  {
  
       #region Objects
       private AraObjectInstance<Ara2.Components.AraButton> _bExcluir = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bExcluir
       {
          get { return _bExcluir.Object; }
          set { _bExcluir.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bAdd = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bAdd
       {
          get { return _bAdd.Object; }
          set { _bAdd.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bRenomear = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bRenomear
       {
          get { return _bRenomear.Object; }
          set { _bRenomear.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bSair = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bSair
       {
          get { return _bSair.Object; }
          set { _bSair.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _lData = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel lData
       {
          get { return _lData.Object; }
          set { _lData.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _A0O4780 = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel A0O4780
       {
          get { return _A0O4780.Object; }
          set { _A0O4780.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraSelect> _sLayouts = new AraObjectInstance<Ara2.Components.AraSelect>();
       public Ara2.Components.AraSelect sLayouts
       {
          get { return _sLayouts.Object; }
          set { _sLayouts.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraDiv> _A0O4771 = new AraObjectInstance<Ara2.Components.AraDiv>();
       public Ara2.Components.AraDiv A0O4771
       {
          get { return _A0O4771.Object; }
          set { _A0O4771.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _A0O4825 = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel A0O4825
       {
          get { return _A0O4825.Object; }
          set { _A0O4825.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _A0O17 = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel A0O17
       {
          get { return _A0O17.Object; }
          set { _A0O17.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraTextBox> _txtWidth = new AraObjectInstance<Ara2.Components.AraTextBox>();
       public Ara2.Components.AraTextBox txtWidth
       {
          get { return _txtWidth.Object; }
          set { _txtWidth.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _A0O37 = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel A0O37
       {
          get { return _A0O37.Object; }
          set { _A0O37.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraTextBox> _txtHeight = new AraObjectInstance<Ara2.Components.AraTextBox>();
       public Ara2.Components.AraTextBox txtHeight
       {
          get { return _txtHeight.Object; }
          set { _txtHeight.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _A0O56 = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel A0O56
       {
          get { return _A0O56.Object; }
          set { _A0O56.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _A0O95 = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel A0O95
       {
          get { return _A0O95.Object; }
          set { _A0O95.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraGrid> _GridOnlyDeviceType = new AraObjectInstance<Ara2.Components.AraGrid>();
       public Ara2.Components.AraGrid GridOnlyDeviceType
       {
          get { return _GridOnlyDeviceType.Object; }
          set { _GridOnlyDeviceType.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bSalvar = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bSalvar
       {
          get { return _bSalvar.Object; }
          set { _bSalvar.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bWidthAtual = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bWidthAtual
       {
          get { return _bWidthAtual.Object; }
          set { _bWidthAtual.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bHeigthAtual = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bHeigthAtual
       {
          get { return _bHeigthAtual.Object; }
          set { _bHeigthAtual.Object = value; }
       }
       #endregion 
       #region Events
       public abstract void bExcluir_Click(System.Object sender,System.EventArgs e);
       public abstract void bAdd_Click(System.Object sender,System.EventArgs e);
       public abstract void bRenomear_Click(System.Object sender,System.EventArgs e);
       public abstract void bSair_Click(System.Object sender,System.EventArgs e);
       public abstract void sLayouts_Click(System.Object sender,System.EventArgs e);
       public abstract void sLayouts_Change(System.Object sender,System.EventArgs e);
       public abstract void bSalvar_Click(System.Object sender,System.EventArgs e);
       public abstract void bWidthAtual_Click(System.Object sender,System.EventArgs e);
       public abstract void bHeigthAtual_Click(System.Object sender,System.EventArgs e);
       #endregion 
       public FrmLayoutAraDesign(IAraContainerClient vConteiner) : base(vConteiner)
       {
           #region Instances
           #region Propertys Main
           this.Title  = @"Layout";
           this.ZIndexWindow  = 1000004;
           this.Visible  = false;
           this.MinWidth  =  new Ara2.Components.AraDistance(@"0px");
           this.MinHeight  =  new Ara2.Components.AraDistance(@"0px");
           this.Width  =  new Ara2.Components.AraDistance(@"590px");
           this.Height  =  new Ara2.Components.AraDistance(@"280px");
           #endregion


           #region bExcluir
           this.bExcluir = new Ara2.Components.AraButton(this);

           this.bExcluir.Text  = @"Del. Layout";
           this.bExcluir.Ico  = Ara2.Components.AraButton.ButtonIco.trash;
           this.bExcluir.Left  =  new Ara2.Components.AraDistance(@"135px");
           this.bExcluir.Top  =  new Ara2.Components.AraDistance(@"9px");
           this.bExcluir.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.bExcluir.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
           this.bExcluir.Width  =  new Ara2.Components.AraDistance(@"120px");
           this.bExcluir.Height  =  new Ara2.Components.AraDistance(@"25px");
           this.bExcluir.ZIndex  = 27;
           this.bExcluir.Click  += bExcluir_Click;
           #endregion
           #region bAdd
           this.bAdd = new Ara2.Components.AraButton(this);

           this.bAdd.Text  = @"Add Layout";
           this.bAdd.Ico  = Ara2.Components.AraButton.ButtonIco.plus;
           this.bAdd.Left  =  new Ara2.Components.AraDistance(@"5px");
           this.bAdd.Top  =  new Ara2.Components.AraDistance(@"10px");
           this.bAdd.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.bAdd.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
           this.bAdd.Width  =  new Ara2.Components.AraDistance(@"125px");
           this.bAdd.Height  =  new Ara2.Components.AraDistance(@"25px");
           this.bAdd.ZIndex  = 27;
           this.bAdd.Click  += bAdd_Click;
           #endregion
           #region bRenomear
           this.bRenomear = new Ara2.Components.AraButton(this);

           this.bRenomear.Text  = @"Rename";
           this.bRenomear.Ico  = Ara2.Components.AraButton.ButtonIco.pencil;
           this.bRenomear.Left  =  new Ara2.Components.AraDistance(@"262px");
           this.bRenomear.Top  =  new Ara2.Components.AraDistance(@"9px");
           this.bRenomear.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.bRenomear.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
           this.bRenomear.Width  =  new Ara2.Components.AraDistance(@"95px");
           this.bRenomear.Height  =  new Ara2.Components.AraDistance(@"25px");
           this.bRenomear.ZIndex  = 27;
           this.bRenomear.Click  += bRenomear_Click;
           #endregion
           #region bSair
           this.bSair = new Ara2.Components.AraButton(this);

           this.bSair.Text  = @" Sair";
           this.bSair.Ico  = Ara2.Components.AraButton.ButtonIco.close;
           this.bSair.Anchor.Top  = 10;
           this.bSair.Anchor.Right  = 10;
           this.bSair.Left  =  new Ara2.Components.AraDistance(@"465px");
           this.bSair.Top  =  new Ara2.Components.AraDistance(@"10px");
           this.bSair.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.bSair.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
           this.bSair.Width  =  new Ara2.Components.AraDistance(@"115px");
           this.bSair.Height  =  new Ara2.Components.AraDistance(@"25px");
           this.bSair.ZIndex  = 28;
           this.bSair.Click  += bSair_Click;
           #endregion
           #region lData
           this.lData = new Ara2.Components.AraLabel(this);

           this.lData.Text  = @"Layout:";
           this.lData.TextAlign  = Ara2.Components.AraLabel.ETextAlign.right;
           this.lData.Left  =  new Ara2.Components.AraDistance(@"0px");
           this.lData.Top  =  new Ara2.Components.AraDistance(@"50px");
           this.lData.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.lData.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
           this.lData.Width  =  new Ara2.Components.AraDistance(@"145px");
           this.lData.Height  =  new Ara2.Components.AraDistance(@"17px");
           this.lData.ZIndex  = 29;
           #endregion
           #region A0O4780
           this.A0O4780 = new Ara2.Components.AraLabel(this);

           this.A0O4780.Text  = @"Triggers:";
           this.A0O4780.TextAlign  = Ara2.Components.AraLabel.ETextAlign.right;
           this.A0O4780.TextVAlignValue  =  new Ara2.Components.AraDistance(@"0px");
           this.A0O4780.Left  =  new Ara2.Components.AraDistance(@"0px");
           this.A0O4780.Top  =  new Ara2.Components.AraDistance(@"81px");
           this.A0O4780.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.A0O4780.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O4780.Width  =  new Ara2.Components.AraDistance(@"145px");
           this.A0O4780.Height  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O4780.ZIndex  = 29;
           #endregion
           #region sLayouts
           this.sLayouts = new Ara2.Components.AraSelect(this);

           this.sLayouts.Anchor.Left  = 155;
           this.sLayouts.Anchor.Right  = 10;
           this.sLayouts.Left  =  new Ara2.Components.AraDistance(@"155px");
           this.sLayouts.Top  =  new Ara2.Components.AraDistance(@"50px");
           this.sLayouts.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.sLayouts.MinHeight  =  new Ara2.Components.AraDistance(@"20px");
           this.sLayouts.Width  =  new Ara2.Components.AraDistance(@"425px");
           this.sLayouts.Height  =  new Ara2.Components.AraDistance(@"20px");
           this.sLayouts.ZIndex  = 41;
           this.sLayouts.Click  += sLayouts_Click;
           this.sLayouts.Change  += sLayouts_Change;
           #endregion
           #region A0O4771
           this.A0O4771 = new Ara2.Components.AraDiv(this);

           this.A0O4771.StyleContainer  = true;
           this.A0O4771.Anchor.Left  = 156;
           this.A0O4771.Anchor.Top  = 77;
           this.A0O4771.Anchor.Right  = 10;
           this.A0O4771.Anchor.Bottom  = 10;
           this.A0O4771.Left  =  new Ara2.Components.AraDistance(@"156px");
           this.A0O4771.Top  =  new Ara2.Components.AraDistance(@"77px");
           this.A0O4771.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.A0O4771.MinHeight  =  new Ara2.Components.AraDistance(@"10px");
           this.A0O4771.Width  =  new Ara2.Components.AraDistance(@"410px");
           this.A0O4771.Height  =  new Ara2.Components.AraDistance(@"195px");
           this.A0O4771.ZIndex  = 42;
           #endregion
           #region A0O4825
           this.A0O4825 = new Ara2.Components.AraLabel(this.A0O4771);

           this.A0O4825.Text  = @"the lower width as";
           this.A0O4825.TextAlign  = Ara2.Components.AraLabel.ETextAlign.right;
           this.A0O4825.Left  =  new Ara2.Components.AraDistance(@"32px");
           this.A0O4825.Top  =  new Ara2.Components.AraDistance(@"7px");
           this.A0O4825.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.A0O4825.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O4825.Width  =  new Ara2.Components.AraDistance(@"120px");
           this.A0O4825.Height  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O4825.ZIndex  = 1;
           #endregion
           #region A0O17
           this.A0O17 = new Ara2.Components.AraLabel(this.A0O4771);

           this.A0O17.Text  = @"the lower height as";
           this.A0O17.TextAlign  = Ara2.Components.AraLabel.ETextAlign.right;
           this.A0O17.Left  =  new Ara2.Components.AraDistance(@"27px");
           this.A0O17.Top  =  new Ara2.Components.AraDistance(@"38px");
           this.A0O17.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.A0O17.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O17.Width  =  new Ara2.Components.AraDistance(@"125px");
           this.A0O17.Height  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O17.ZIndex  = 2;
           #endregion
           #region txtWidth
           this.txtWidth = new Ara2.Components.AraTextBox(this.A0O4771);

           this.txtWidth.Text  = @"0";
           this.txtWidth.Mask  = Ara2.Components.AraTextBox.AraTextBoxMaskTypes.Integer;
           this.txtWidth.MaskText  = @"999.999.999.999";
           this.txtWidth.MaskType  = @"reverse";
           this.txtWidth.MaskDefaultValue  = @"0";
           this.txtWidth.SelectionStart  = 1;
           this.txtWidth.SelectionEnd  = 1;
           this.txtWidth.Left  =  new Ara2.Components.AraDistance(@"157px");
           this.txtWidth.Top  =  new Ara2.Components.AraDistance(@"6px");
           this.txtWidth.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.txtWidth.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
           this.txtWidth.Width  =  new Ara2.Components.AraDistance(@"70px");
           this.txtWidth.Height  =  new Ara2.Components.AraDistance(@"17px");
           this.txtWidth.ZIndex  = 3;
           #endregion
           #region A0O37
           this.A0O37 = new Ara2.Components.AraLabel(this.A0O4771);

           this.A0O37.Text  = @"px (0 to ignore)";
           this.A0O37.Left  =  new Ara2.Components.AraDistance(@"233px");
           this.A0O37.Top  =  new Ara2.Components.AraDistance(@"7px");
           this.A0O37.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.A0O37.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O37.Width  =  new Ara2.Components.AraDistance(@"100px");
           this.A0O37.Height  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O37.ZIndex  = 4;
           #endregion
           #region txtHeight
           this.txtHeight = new Ara2.Components.AraTextBox(this.A0O4771);

           this.txtHeight.Text  = @"0";
           this.txtHeight.Mask  = Ara2.Components.AraTextBox.AraTextBoxMaskTypes.Integer;
           this.txtHeight.MaskText  = @"999.999.999.999";
           this.txtHeight.MaskType  = @"reverse";
           this.txtHeight.MaskDefaultValue  = @"0";
           this.txtHeight.SelectionStart  = 1;
           this.txtHeight.SelectionEnd  = 1;
           this.txtHeight.Left  =  new Ara2.Components.AraDistance(@"158px");
           this.txtHeight.Top  =  new Ara2.Components.AraDistance(@"35px");
           this.txtHeight.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.txtHeight.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
           this.txtHeight.Width  =  new Ara2.Components.AraDistance(@"70px");
           this.txtHeight.Height  =  new Ara2.Components.AraDistance(@"17px");
           this.txtHeight.ZIndex  = 5;
           #endregion
           #region A0O56
           this.A0O56 = new Ara2.Components.AraLabel(this.A0O4771);

           this.A0O56.Text  = @"px (0 to ignore)";
           this.A0O56.TextVAlignValue  =  new Ara2.Components.AraDistance(@"0px");
           this.A0O56.Left  =  new Ara2.Components.AraDistance(@"234px");
           this.A0O56.Top  =  new Ara2.Components.AraDistance(@"36px");
           this.A0O56.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.A0O56.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O56.Width  =  new Ara2.Components.AraDistance(@"100px");
           this.A0O56.Height  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O56.ZIndex  = 6;
           #endregion
           #region A0O95
           this.A0O95 = new Ara2.Components.AraLabel(this.A0O4771);

           this.A0O95.Text  = @"only device type";
           this.A0O95.TextAlign  = Ara2.Components.AraLabel.ETextAlign.right;
           this.A0O95.TextVAlignValue  =  new Ara2.Components.AraDistance(@"0px");
           this.A0O95.Left  =  new Ara2.Components.AraDistance(@"27px");
           this.A0O95.Top  =  new Ara2.Components.AraDistance(@"66px");
           this.A0O95.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.A0O95.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O95.Width  =  new Ara2.Components.AraDistance(@"125px");
           this.A0O95.Height  =  new Ara2.Components.AraDistance(@"17px");
           this.A0O95.ZIndex  = 7;
           #endregion
           #region GridOnlyDeviceType
           this.GridOnlyDeviceType = new Ara2.Components.AraGrid(this.A0O4771);

           this.GridOnlyDeviceType.Anchor.Left  = 158;
           this.GridOnlyDeviceType.Anchor.Top  = 67;
           this.GridOnlyDeviceType.Anchor.Right  = 10;
           this.GridOnlyDeviceType.Left  =  new Ara2.Components.AraDistance(@"158px");
           this.GridOnlyDeviceType.Top  =  new Ara2.Components.AraDistance(@"67px");
           this.GridOnlyDeviceType.MinWidth  =  new Ara2.Components.AraDistance(@"0px");
           this.GridOnlyDeviceType.MinHeight  =  new Ara2.Components.AraDistance(@"0px");
           this.GridOnlyDeviceType.Width  =  new Ara2.Components.AraDistance(@"242px");
           this.GridOnlyDeviceType.Height  =  new Ara2.Components.AraDistance(@"85px");
           this.GridOnlyDeviceType.ZIndex  = 8;
           this.GridOnlyDeviceType.MultiSelect  = true;
           #endregion
           #region bSalvar
           this.bSalvar = new Ara2.Components.AraButton(this.A0O4771);

           this.bSalvar.Text  = @"Save";
           this.bSalvar.Ico  = Ara2.Components.AraButton.ButtonIco.disk;
           this.bSalvar.Anchor.Bottom  = 10;
           this.bSalvar.Anchor.CenterH  = true;
           this.bSalvar.Left  =  new Ara2.Components.AraDistance(@"160px");
           this.bSalvar.Top  =  new Ara2.Components.AraDistance(@"160px");
           this.bSalvar.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.bSalvar.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
           this.bSalvar.Width  =  new Ara2.Components.AraDistance(@"90px");
           this.bSalvar.Height  =  new Ara2.Components.AraDistance(@"25px");
           this.bSalvar.ZIndex  = 9;
           this.bSalvar.Click  += bSalvar_Click;
           #endregion
           #region bWidthAtual
           this.bWidthAtual = new Ara2.Components.AraButton(this.A0O4771);

           this.bWidthAtual.Ico  = Ara2.Components.AraButton.ButtonIco.flag;
           this.bWidthAtual.Left  =  new Ara2.Components.AraDistance(@"338px");
           this.bWidthAtual.Top  =  new Ara2.Components.AraDistance(@"3px");
           this.bWidthAtual.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.bWidthAtual.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
           this.bWidthAtual.Width  =  new Ara2.Components.AraDistance(@"35px");
           this.bWidthAtual.Height  =  new Ara2.Components.AraDistance(@"25px");
           this.bWidthAtual.ZIndex  = 10;
           this.bWidthAtual.Click  += bWidthAtual_Click;
           #endregion
           #region bHeigthAtual
           this.bHeigthAtual = new Ara2.Components.AraButton(this.A0O4771);

           this.bHeigthAtual.Ico  = Ara2.Components.AraButton.ButtonIco.flag;
           this.bHeigthAtual.Left  =  new Ara2.Components.AraDistance(@"337px");
           this.bHeigthAtual.Top  =  new Ara2.Components.AraDistance(@"32px");
           this.bHeigthAtual.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.bHeigthAtual.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
           this.bHeigthAtual.Width  =  new Ara2.Components.AraDistance(@"35px");
           this.bHeigthAtual.Height  =  new Ara2.Components.AraDistance(@"25px");
           this.bHeigthAtual.ZIndex  = 11;
           this.bHeigthAtual.Click  += bHeigthAtual_Click;
           #endregion
           #endregion
       } 
   } 
} 
