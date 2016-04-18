using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ara2.Components;
using Ara2;
using System.IO;

namespace Ara2.Dev.AraDesign.Edit
{
    [Serializable]
    public class MainEditAraDesign: Ara2.Components.WindowMain
    {
        private AraObjectInstance<AraTimer> _Timer = new AraObjectInstance<AraTimer>();
        public AraTimer Timer
        {
            get { return _Timer.Object; }
            set { _Timer.Object = value; }
        }

        AraObjectInstance<DivEdit> _DivEdit = new AraObjectInstance<DivEdit>();
        public DivEdit Edit
        {
            get { return _DivEdit.Object; }
            set { _DivEdit.Object = value; }
        }

        public MainEditAraDesign(Ara2.Session Session)
            : base(Session)
        {
            Edit = new DivEdit(this);
            Edit.Anchor.Left = 0;
            Edit.Anchor.Top = 0;
            Edit.Anchor.Right = 0;
            Edit.Visible = true;
            Edit.Anchor.Bottom = 0;

            Timer = new AraTimer(this);
        }

        public override string GetBodyHtml()
        {
            return "";
        }
    }
}
