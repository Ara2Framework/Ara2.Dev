// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.IO;
using Ara2.Components;
using Ara2;
using Ara2.Dev;
using System.ComponentModel;

namespace Ara2.Components.Layout
{
    [Serializable]
    public class FrmLayout : AraDesign.FrmLayoutAraDesign, IWindowAraDevPropertyCustomWindow, IFormAraCustomEditor
    {
        IAraContainerClient ObjectConteinerCanvas;
        public object ObjectCanvas
        {
            get
            {
                return ObjectConteinerCanvas;
            }
            set
            {
                ObjectConteinerCanvas = (IAraContainerClient)value;
                CarregaSelect();
            }
        }

        IAraContainerClient ObjectConteinerCanvasReal;
        public object ObjectCanvasReal
        {
            get
            {
                return ObjectConteinerCanvasReal;
            }
            set
            {
                ObjectConteinerCanvasReal = (IAraContainerClient)value;
                CarregaSelect();
            }
        }

        public PropertyInfo PropertyInfo { get; set; }
        public System.Action onRefreshScreen { get; set; }

        public bool IsOpenCustomEditor(object vObj)
        {
            return (string)vObj == LayoutCurrentConverter.NovoEditar;
        }

        public string Value
        {
            get;
            set;
        }

        

        public FrmLayout(IAraContainerClient ConteinerFather)
            : base(ConteinerFather)
        {
            GridOnlyDeviceType.Cols.Add(new AraGridCol(GridOnlyDeviceType, " ", "nome"));
            GridOnlyDeviceType.Commit();

            foreach (string vName in Enum.GetNames(typeof(EDeviceType)))
            {
                GridOnlyDeviceType.Rows.Add(vName, new Dictionary<string, string>
                {
                    {"nome",vName}
                });
            }

            GridOnlyDeviceType.AutoAdjustColumnWidth();
            this.Unload += FrmLayout_Unload;
        }

        private void FrmLayout_Unload(object vObjReturn)
        {
            onRefreshScreen();
        }

        public override void bAdd_Click(object sender, EventArgs e)
        {
            AraTools.AlertGetString("Nome do layout", Add);
        }

        public void Add(string vName)
        {
            if (vName != null || vName.Trim() != "")
            {
                if (ObjectConteinerCanvas.Layouts == null)
                {

                    ObjectConteinerCanvas.Layouts = new AraLayouts(ObjectConteinerCanvas);
                    if (ObjectConteinerCanvas.InstanceID!=ObjectConteinerCanvasReal.InstanceID)
                        ObjectConteinerCanvasReal.LayoutsString = ObjectConteinerCanvas.LayoutsString;
                }

                ObjectConteinerCanvas.Layouts.Add(vName);
                if (ObjectConteinerCanvas.InstanceID != ObjectConteinerCanvasReal.InstanceID)
                    ObjectConteinerCanvasReal.LayoutsString = ObjectConteinerCanvas.LayoutsString;

                CarregaSelect();
                sLayouts.Text = vName;
                ObjectConteinerCanvas.LayoutCurrent = vName;

                onRefreshScreen();
            }
        }

        public void CarregaSelect()
        {
            sLayouts.Clear();
            bool AddDefault = false;
            if (ObjectConteinerCanvas.Layouts != null)
            {
                foreach (AraLayout L in ObjectConteinerCanvas.Layouts)
                {
                    string vName = L.Name;
                    if (vName == null)
                    {
                        vName = "Default";
                        AddDefault = true;
                    }

                    sLayouts.List.Add(vName, vName);
                }
            }

            try
            {
                if (!AddDefault)
                    sLayouts.List.Add("Default", "Default");

                sLayouts.Text = (ObjectConteinerCanvas.LayoutCurrent == null ? "Default" : ObjectConteinerCanvas.LayoutCurrent);

                CarregaLayout();
            }
            catch { }
        }

        public override void bExcluir_Click(object sender, EventArgs e)
        {
            if (sLayouts.Text == "Default")
            {
                AraTools.Alert("Não pode ser excluido");
                return;
            }

            if (ObjectConteinerCanvas.Layouts == null)
                return;

            ObjectConteinerCanvas.Layouts.LayoutCurrent = null;

            ObjectConteinerCanvas.Layouts.Remove(sLayouts.Text);
            if (ObjectConteinerCanvas.InstanceID != ObjectConteinerCanvasReal.InstanceID)
                ObjectConteinerCanvasReal.LayoutsString = ObjectConteinerCanvas.LayoutsString;
            CarregaSelect();

            onRefreshScreen();
        }

        public override void bSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public override void sLayouts_Change(object sender, EventArgs e)
        {
            if (sLayouts.Text != "")
            {
                if (ObjectConteinerCanvas.LayoutCurrent != (sLayouts.Text == "Default" ? null : sLayouts.Text))
                {
                    ObjectConteinerCanvas.LayoutCurrent = (sLayouts.Text == "Default" ? null : sLayouts.Text);
                    if (ObjectConteinerCanvas.InstanceID != ObjectConteinerCanvasReal.InstanceID)
                        ObjectConteinerCanvasReal.LayoutsString = ObjectConteinerCanvas.LayoutsString;

                    CarregaLayout();
                }
            }
        }

        private void CarregaLayout()
        {
            AraLayout LayoutCurrent = ObjectConteinerCanvas.Layouts.GetLayoutCurrent();

            txtWidth.Text = (LayoutCurrent.LayoutCurrentWidthLess == null ? "0" : LayoutCurrent.LayoutCurrentWidthLess.ToString());
            txtHeight.Text = (LayoutCurrent.LayoutCurrentHeightLess == null ? "0" : LayoutCurrent.LayoutCurrentHeightLess.ToString());

            foreach (AraGridRow Row in GridOnlyDeviceType.Rows.ToArray())
            {
                if (LayoutCurrent.DeviceTypes != null)
                    Row.Select =LayoutCurrent.DeviceTypes.Where(a=> Enum.GetName(typeof(EDeviceType),a) == Row.ID).Count() >0;
                else
                    Row.Select = false;
            }

            onRefreshScreen();
        }

        public override void sLayouts_Click(object sender, EventArgs e)
        {
            sLayouts_Change(sender, e);
        }

        public override void bRenomear_Click(object sender, EventArgs e)
        {
            if (sLayouts.Text == "Default")
            {
                AraTools.Alert("Não pode ser renomeado");
                return;
            }

            AraTools.AlertGetString("Digite o novo nome.", sLayouts.Text, Renomeia);
        }

        private void Renomeia(string vValor)
        {
            if (vValor  != null && vValor.Trim() != "")
            {
                ObjectConteinerCanvas.Layouts.RenameCurrent(vValor);
                if (ObjectConteinerCanvas.InstanceID != ObjectConteinerCanvasReal.InstanceID)
                    ObjectConteinerCanvasReal.LayoutsString = ObjectConteinerCanvas.LayoutsString;

                onRefreshScreen();
            }
        }



        public override void bSalvar_Click(object sender, EventArgs e)
        {
            if (ObjectConteinerCanvas.Layouts == null || ObjectConteinerCanvas.Layouts.GetLayoutCurrent() == null)
            {
                AraTools.Alert("Crie um layout primeiro.");
                return;
            }

            AraLayout LayoutCurrent = ObjectConteinerCanvas.Layouts.GetLayoutCurrent();


            int? H = (Convert.ToInt32(txtHeight.Text) == 0 ? (int?)null : (int?)Convert.ToInt32(txtHeight.Text));
            int? W = (Convert.ToInt32(txtWidth.Text) == 0 ? (int?)null : (int?)Convert.ToInt32(txtWidth.Text));


            if (LayoutCurrent.LayoutCurrentHeightLess != H)
                LayoutCurrent.LayoutCurrentHeightLess = H;

            if (LayoutCurrent.LayoutCurrentWidthLess != W)
                LayoutCurrent.LayoutCurrentWidthLess = W;

            List<EDeviceType> vTmpDT = new List<EDeviceType>();
            foreach (AraGridRow Row in GridOnlyDeviceType.Rows.ToArray())
            {
                if (Row.Select)
                    vTmpDT.Add((EDeviceType)Enum.Parse(typeof(EDeviceType), Row.ID));
            }

            if (vTmpDT.Count() > 0)
                LayoutCurrent.DeviceTypes = vTmpDT.ToArray();
            else
                LayoutCurrent.DeviceTypes = null;


            if (ObjectConteinerCanvas.InstanceID != ObjectConteinerCanvasReal.InstanceID)
                ObjectConteinerCanvasReal.LayoutsString = ObjectConteinerCanvas.LayoutsString;

            CarregaSelect();
            onRefreshScreen();
            AraTools.Alert("OK");
        }

        public override void bHeigthAtual_Click(object sender, EventArgs e)
        {
            txtHeight.Text = ((IAraComponentVisualDimensionsWidthHeight)ObjectConteinerCanvas).Height.Value.ToString();
        }

        public override void bWidthAtual_Click(object sender, EventArgs e)
        {
            txtWidth.Text = ((IAraComponentVisualDimensionsWidthHeight)ObjectConteinerCanvas).Width.Value.ToString();
        }
    }
}