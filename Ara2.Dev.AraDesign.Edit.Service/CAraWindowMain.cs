using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2;
using Ara2.Components;
using Ara2.Keyboard;
using System.ComponentModel;

namespace Ara2.Dev.AraDesign.Edit.Service
{
    [Serializable]
    public class CWindowMain : AraObject, IAraWindowMain
    {
        public CWindowMain() :
        base(Tick.GetTick().Session.GetNewID(), Tick.GetTick().Session.WindowMain)
        {
            #region Set Eventos
            Click = new AraComponentEvent<EventHandler>(this, "Click");
            ResizeStop = new AraComponentEvent<Action>(this, "ResizeStop");

            //KeyDown = new AraComponentEventKey<DArakeyboard>(this, "KeyDown");
            //KeyUp = new AraComponentEventKey<DArakeyboard>(this, "KeyUp");
            //KeyPress = new AraComponentEventKey<DArakeyboard>(this, "KeyPress");

            PopState = new AraComponentEvent<DPopState>(this, "PopState");
            Menssage = new AraComponentEvent<DMenssage>(this, "Menssage");

            Unload = new AraComponentEvent<Action>(this, "unload");

            ChangelocationHash = new AraEvent<Action>();
            Active = new AraEvent<Action>();

            #endregion
        }

        public void Show()
        {

        }
        #region Div
        public void ShowDiv(IAraComponentVisualAnchor vForm)
        {

        }
        public void ShowDivModal(IDivModal vForm)
        {

        }
        public void CloseDivModal(IDivModal vForm)
        {

        }
        public void CloseDivModal(IDivModal vForm, object vObjReturn)
        {

        }
        public void HideDivModal(IDivModal vForm)
        {

        }
        public void HideDivModal(IDivModal vForm, object vObjReturn)
        {

        }

        [Browsable(false)]
        public IAraComponentVisualAnchor DivCanvas { get; set; }
        #endregion

        public void SetCookie(string vName, string vValue, int? ExpiresDays = null, string path = null)
        {

        }
        public void GetCookie(string vName, DGetCookie vEventReturn)
        {

        }
        public void DelCookie(string vName, string path = null)
        {

        }

        public void SetHistoryReplaceState(string vValue)
        {

        }

        public void SetHistoryPushState(string vValue)
        {

        }

        public void WaitLoading(string vMessage, Action vAction)
        {

        }
        
        public void TickScriptCall()
        {
            throw new NotImplementedException();
        }

        public void CssAddClass(string vNameClass)
        {
            throw new NotImplementedException();
        }

        public void CssRemoveClass(string vNameClass)
        {
            throw new NotImplementedException();
        }

        public void Style(string vInstanceID, string vValue)
        {
            throw new NotImplementedException();
        }

        public void LoadJS()
        {
            
        }

        [AraDevProperty("")]
        public string Name { get; set; }

        [AraDevEvent]
        public AraComponentEvent<Action> ResizeStop { get; set; }
        [AraDevEvent]
        public AraComponentEvent<Action> Unload { get; set; }
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Click { get; set; }
        [AraDevEvent]
        public AraComponentEventKey<DArakeyboard> KeyDown { get; set; }
        [AraDevEvent]
        public AraComponentEventKey<DArakeyboard> KeyUp { get; set; }
        [AraDevEvent]
        public AraComponentEventKey<DArakeyboard> KeyPress { get; set; }
        [AraDevEvent]
        public AraComponentEvent<DPopState> PopState { get; set; }
        [AraDevEvent]
        public AraEvent<Action> ChangelocationHash { get; set; }
        [AraDevEvent]
        public AraEvent<Action> Active { get; set; }
        [AraDevEvent]
        public AraComponentEvent<DMenssage> Menssage { get; set; }
        [AraDevEvent]
        public AraEvent<DStartEditPropertys> StartEditPropertys { get; set; }
        [AraDevEvent]
        public AraEvent<DStartEditPropertys> ChangeProperty { get; set; }

        #region  IAraWindowMain
        [Browsable(false)]
        public bool Visible {get;set;}

        [Browsable(false)]
        public AraComponentVisual.ETypePosition TypePosition {get;set;}

        [Browsable(false)]
        public AraResizable Resizable {get;set;}

        [Browsable(false)]
        public AraDraggable Draggable {get;set;}

        [Browsable(false)]
        public AraSelectable Selectable {get;set;}

        
        [Browsable(false)]
        public int? ZIndex {get;set;}

        
        [Browsable(false)]
        public AraEvent<DComponentEventInternal> EventInternal {get;set;}

        
        [Browsable(false)]
        public AraEvent<DComponentProperty> SetProperty {get;set;}

        
        [Browsable(false)]
        public AraDistance Left {get;set;}

        
        [Browsable(false)]
        public AraDistance Top {get;set;}

        
        [Browsable(false)]
        public AraDistance MinWidth {get;set;}

        
        [Browsable(false)]
        public AraDistance MinHeight {get;set;}

        
        [Browsable(false)]
        [AraDevProperty]
        public AraDistance Width {get;set;}

        
        [Browsable(false)]
        [AraDevProperty]
        public AraDistance Height {get;set;}

        
        [Browsable(false)]
        public AraEvent<AraDistance.DChangeDistance> WidthChangeBefore {get;set;}

        
        [Browsable(false)]
        public AraEvent<AraDistance.DChangeDistance> HeightChangeBefore {get;set;}

        
        [Browsable(false)]
        public AraComponentEvent<Action> WidthChangeAfter {get;set;}

        
        [Browsable(false)]
        public AraComponentEvent<Action> HeightChangeAfter {get;set;}
        #endregion
    }
}
