// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
Ara.AraClass.Add('AraGridOfIcons', function (vAppId, vId, ConteinerFather) {
   
    // Eventos  ---------------------------------------
    this.Events = {};

    var TmpThis = this;
    this.Events.Click =
    {
        Enabled: false,
        SetEnabled: function (vValue) {
            var TmpThis2 = this;
            if (vValue && this.PrimeiraAtivacaoEvento != true) {
                $(TmpThis.Obj).bind('contextmenu click', function (e) {
                    if (TmpThis2) TmpThis2.Function(e);
                    return false;
                });
                this.PrimeiraAtivacaoEvento = true;
            }

            this.Enabled = vValue;
        },
        ThreadType: 2, // Single_thread
        Function: function (event) {
            if (this.Enabled) {
                var vParans = {
                    Mouse_which: event.which,
                    Mouse_layerX: event.layerX,
                    Mouse_layerY: event.layerY,
                    Mouse_clientX: event.clientX,
                    Mouse_clientY: event.clientY,
                    Mouse_offsetX: event.offsetX,
                    Mouse_offsetY: event.offsetY,
                    Mouse_pageX: event.pageX,
                    Mouse_pageY: event.pageY,
                    Mouse_screenX: event.screenX,
                    Mouse_screenY: event.screenY,
                    Mouse_x: event.x,
                    Mouse_y: event.y,
                    Mouse_altKey: event.altKey,
                    Mouse_ctrlKey: event.ctrlKey,
                    Mouse_shiftKey: event.shiftKey
                };

                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "Click", vParans);
            }
        }
    };
    
    this.Events.WidthChangeAfter =
    {
        Enabled: false,
        ThreadType: 1, // Multiple_thread
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "WidthChangeAfter", null);
            }
        }
    };

    this.Events.HeightChangeAfter =
    {
        Enabled: false,
        ThreadType: 1, // Multiple_thread
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "HeightChangeAfter", null);
            }
        }
    };

    this.Events.ClickIco =
    {
        Enabled: false,
        ThreadType: 2,
        Function: function (vKey, event) {
            if (this.Enabled) {

                if (TmpThis.Icons[vKey].ClickCancel) {
                    TmpThis.Icons[vKey].ClickCancel = false;
                    return;
                }

                var vParans = {
                    Mouse_which: event.which,
                    Mouse_layerX: event.layerX,
                    Mouse_layerY: event.layerY,
                    Mouse_clientX: event.clientX,
                    Mouse_clientY: event.clientY,
                    Mouse_offsetX: event.offsetX,
                    Mouse_offsetY: event.offsetY,
                    Mouse_pageX: event.pageX,
                    Mouse_pageY: event.pageY,
                    Mouse_screenX: event.screenX,
                    Mouse_screenY: event.screenY,
                    Mouse_x: event.x,
                    Mouse_y: event.y,
                    Mouse_altKey: event.altKey,
                    Mouse_ctrlKey: event.ctrlKey,
                    Mouse_shiftKey: event.shiftKey,
                    key: vKey
                };

                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "ClickIco", vParans);
            }
        }
    };

    this.Events.ChangePositionIco =
    {
        Enabled: false,
        ThreadType: 1,
        Function: function (vKey) {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "ChangePositionIco", { key: vKey });
            }
        }
    };

    

    this.Events.IsVisible =
    {
        Enabled: false,
        ThreadType: 1,
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "IsVisible", null);
            }
        }
    };

   

    this.SetVisible = function (vTmp) {
        if (vTmp)
            this.Obj.style.display = "block";
        else
            this.Obj.style.display = "none";
    }


    this.Left = null;
    this.SetLeft = function (vTmp) {
        if (this.Left != vTmp) {
            this.Left = vTmp;
            this.Obj.style.left = vTmp;
        }
    }

    this.Top = null;
    this.SetTop = function (vTmp) {
        if (this.Top != vTmp) {
            this.Top = vTmp;
            this.Obj.style.top = vTmp;
        }
    }

    this._MinWidth = null;
    this.SetMinWidth = function (vTmp) {
        this._MinWidth = vTmp;
        if (this._MinWidth != null && this.Width != null && parseInt(this._MinWidth, 10) > parseInt(this.Width, 10))
            this.SetWidth(this._MinWidth, false);
    }

    this.Width = null;
    this.SetWidth = function (vTmp, vServer) {
        if (vTmp != null && this._MinWidth != null && parseInt(this._MinWidth, 10) > parseInt(vTmp, 10))
            vTmp = this._MinWidth;

        if (this.Width != vTmp) {
            this.Width = vTmp;
            if (vServer)
                this.ControlVar.SetValueUtm('Width', this.Width);

            this.Obj.style.width = vTmp;

            if (!vServer)
                this.Events.WidthChangeAfter.Function();

            if (this.Anchor != null)
                this.Anchor.RenderChildren();
        }
    }

    this._MinHeight = null;
    this.SetMinHeight = function (vTmp) {
        this._MinHeight = vTmp;
        if (this._MinHeight != null && this.Height != null && parseInt(this._MinHeight,10) > parseInt(this.Height, 10))
            this.SetHeight(this._MinHeight, false);
    }

    this.Height = null;
    this.SetHeight = function (vTmp, vServer) {
        if (vTmp != null && this._MinHeight != null && parseInt(this._MinHeight,10) > parseInt(vTmp,10))
            vTmp = this._MinHeight;

        if (this.Height != vTmp) {
            this.Height = vTmp;
            if (vServer)
                this.ControlVar.SetValueUtm('Height', this.Height);

            this.Obj.style.height = vTmp;

            if (!vServer)
                this.Events.HeightChangeAfter.Function();

            if (this.Anchor != null)
                this.Anchor.RenderChildren();
        }
    }


    this.SetOverFlow = function (vTmp) {
        this.Obj.style.overflow = vTmp;
    }

    

    this.AddClass = function (vTmp) {
        $(this.Obj).addClass(vTmp);
    }

    this.DelClass = function (vTmp) {
        $(this.Obj).removeClass(vTmp);
    }

    this.destruct = function () {
        $(this.Obj).remove();
    }

    this.IsDestroyed = function () {
        if (!document.getElementById(this.id))
            return true;
        else
            return false;
    }

    this.FormResize = function () {
        if (this.Anchor)
            this.Anchor.FormResize();
    }

    this.IcoWidth = 64;
    this.SetIcoWidth = function (vValor) {
        this.IcoWidth = vValor;
    }

    this.IcoHeight = 64;
    this.SetIcoHeight = function (vValor) {
        this.IcoHeight = vValor;
    }

    this.Icons = Array();


    this.ClassIcons = function (vObjAraGridOfIcons, vObj, vObjjquery, vKey, vCaption) {
        return {
            ObjAraGridOfIcons : vObjAraGridOfIcons,
            obj : vObj,
            objjquery : vObjjquery,
            key : vKey,
            caption : vCaption,
            left : 0,
            top : 0,
            ClickCancel : false,
            stop : function () {
                this.ObjAraGridOfIcons.FallsOnTheGrid(this.obj);
                this.top = parseInt(this.obj.style.top, 10);
                this.left = parseInt(this.obj.style.left, 10);
                this.ObjAraGridOfIcons.ChecaColicao(this.key);
                this.ClickCancel = true;

                if (this.start_top != this.top || this.start_left != this.left) {
                    TmpThis.Events.ChangePositionIco.Function(this.key);
                }
            },

            start : function () {
                this.start_top = parseInt(this.obj.style.top, 10);
                this.start_left = parseInt(this.obj.style.left, 10);
            },
            SetLeft : function (vvalue) {
                this.obj.style.left = vvalue + "px";
                this.ObjAraGridOfIcons.FallsOnTheGrid(this.obj);
                this.left = parseInt(this.obj.style.left, 10);
            },
            SetTop: function (vvalue) {
                this.obj.style.top = vvalue + "px";
                this.ObjAraGridOfIcons.FallsOnTheGrid(this.obj);
                this.top = parseInt(this.obj.style.top, 10);
            }
        };
    }

    this.FallsOnTheGrid = function (vObj2) {
        var vtop = parseInt(vObj2.style.top, 10);
        var vleft = parseInt(vObj2.style.left, 10);

        var topR = Math.round(vtop / (this.IcoHeight + 32));
        var leftR = Math.round(vleft / (this.IcoWidth + 5));

        topR = topR * (this.IcoHeight + 32);
        leftR = leftR * (this.IcoWidth + 5);

        vObj2.style.top = topR + "px";
        vObj2.style.left = leftR + "px";
    }


    this.Event_ChangePosition = false;


    this.TextBackgroundColor = "";
    this.SetTextBackgroundColor = function (vColor) {
        this.TextBackgroundColor = vColor;
    }

    this.TextColor = "";
    this.SetTextColor = function (vColor) {
        this.TextColor = vColor;
    }

    this.TextFont = "Arial";
    this.SetTextFont = function (vFont) {
        this.TextFont = vFont;
    }

    this.AddIco = function (vKey, vImg, vCaption) {

        var vHtml = "";

        var Id = this.id + "_" + vKey;
        var IdIco = Id + "_ico";
        var IdCaption = Id + "_caption";

        vHtml += "<div id='" + IdIco + "' style=\"cursor:pointer;background-image: url('" + vImg + "'); background-repeat: no-repeat;background-position: center center;width:" + this.IcoWidth + "px;height:" + this.IcoHeight + "px\" ></div>";
        //vHtml += "<div style=\"width:100%;height:32px;overflow:\">" + vCaption + "</div>";


        var vTmpTextBackgroundColor = "";
        if (this.TextBackgroundColor != "")
            vTmpTextBackgroundColor = "background-color: " + this.TextBackgroundColor + ";border-radius: 4px 4px 4px 4px;";

        var vTmpTextColor = "";
        if (this.TextColor != "")
            vTmpTextColor = "color: " + this.TextColor + ";";

        var vTmpTextFont = "";
        if (this.TextFont != "")
            vTmpTextFont = "font-family: " + this.TextFont + ";";

        vHtml += "<div id='" + IdCaption + "' style=\"text-align:center;width:100%;" + vTmpTextBackgroundColor + vTmpTextColor + vTmpTextFont + "\" title='" + vCaption + "' class='ui-widget ui-widget-content ui-corner-all' >"; //overflow:hidden
        vHtml += vCaption;
        vHtml += "</div>";

        var vObj = document.createElement('Div');
        vObj.id = Id;
        vObj.style.top = "0px";
        vObj.style.left = "0px";
        vObj.style.width = this.IcoWidth + "px";
        vObj.style.position = "absolute";
        vObj.title = vCaption;
        vObj.innerHTML = vHtml;


        var TmpThis = this;
        this.Icons[vKey] = new this.ClassIcons(this, vObj, $(vObj), vKey, vCaption);

        $(vObj).bind('contextmenu click', function (e) {
            TmpThis.Events.ClickIco.Function(vKey, e);
            return false;
        });


        var tmpico = this.Icons[vKey];
        $(vObj).draggable({
            //grid: [this.IcoWidth, this.IcoHeight + 32],
            containment: "parent",
            stop: function () { tmpico.stop() },
            start: function () { tmpico.start() }
        });

        this.Obj.appendChild(this.Icons[vKey].obj);
        this.ChecaColicao(vKey);

        this.AdaptaCaption(IdCaption, vCaption);
    }

    this.AdaptaCaption = function (vIdCaption, vValor) {


        var ObjTesteW = null;
        if (!document.getElementById("AraGridOfIconsAutoAdjustCaption")) {

            ObjTesteW = document.createElement('div');
            ObjTesteW.id = "AraGridOfIconsAutoAdjustCaption";
            ObjTesteW.style.top = "-5000px";
            ObjTesteW.style.left = "-5000px";
            ObjTesteW.style.position = "absolute";
            this.Obj.appendChild(ObjTesteW);
        }

        ObjTesteW = document.getElementById("AraGridOfIconsAutoAdjustCaption");

        //ObjTesteW.style.width = this.IcoWidth + "px";
        ObjTesteW.innerHTML = "AAA";
        var ObjTesteWJQ = $(ObjTesteW);

        if (ObjTesteWJQ.width() == 0 || ObjTesteWJQ.height() == 0) {
            var TmpThis = this;
            setTimeout(function () { TmpThis.AdaptaCaption(vIdCaption, vValor); }, 100);
            return;
        }

        var ObjCaption = document.getElementById(vIdCaption);
        var Linhas = 1;

        ObjTesteW.innerHTML = "";
        var Palavras = vValor.split(" ");

        for (var palavraN in vValor.split(" ")) {
            ObjTesteW.innerHTML += Palavras[palavraN] + " ";

            if (ObjTesteWJQ.width() >= this.IcoWidth || ObjTesteWJQ.height() > 32) {
                if (Linhas == 1) {
                    ObjTesteW.innerHTML = ObjTesteW.innerHTML.substring(0, ObjTesteW.innerHTML.length - Palavras[palavraN].length - 1);
                    if (ObjTesteW.innerHTML != "") {
                        ObjTesteW.innerHTML += "<br>";
                        ObjTesteW.innerHTML += Palavras[palavraN] + " ";
                    }
                    else {
                        ObjTesteW.innerHTML = Palavras[palavraN];
                        if (ObjTesteWJQ.width() >= this.IcoWidth || ObjTesteWJQ.height() > 32) {
                            ObjTesteW.innerHTML = ObjTesteW.innerHTML + "...";
                            while (ObjTesteWJQ.width() >= this.IcoWidth || ObjTesteWJQ.height() >= 32) {
                                ObjTesteW.innerHTML = ObjTesteW.innerHTML.substring(0, ObjTesteW.innerHTML.length - 4) + "...";
                            }
                        }
                        ObjTesteW.innerHTML += "<br>";
                    }
                    Linhas++;
                }
                else if (Linhas > 1) {
                    ObjTesteW.innerHTML = ObjTesteW.innerHTML + "...";
                    while (ObjTesteWJQ.width() >= this.IcoWidth || ObjTesteWJQ.height() > 32) {
                        ObjTesteW.innerHTML = ObjTesteW.innerHTML.substring(0, ObjTesteW.innerHTML.length - 4) + "...";
                    }
                }

            }
        }

        ObjCaption.innerHTML = ObjTesteW.innerHTML;
    }

    this.ClearIcos = function () {
        this.Icons = Array();
        this.Obj.innerHTML = "";
    }

    this.DelIco = function (vKey) {
        var Id = this.id + "_" + vKey;
        this.Obj.removeChild(document.getElementById(Id));
        delete this.Icons[vKey];
    }


    this.SetIcoLeft = function (vKey, vValor) {
        this.Icons[vKey].SetLeft(vValor);
    }

    this.SetIcoTop = function (vKey, vValor) {
        this.Icons[vKey].SetTop(vValor);
    }

    this.ChecaColicao = function (vKey) {
        for (n in this.Icons) {
            if (this.Icons[n].top == this.Icons[vKey].top && this.Icons[n].left == this.Icons[vKey].left && vKey != n) {
                this.Icons[vKey].SetLeft(this.Icons[vKey].left + this.IcoWidth);
                this.ChecaColicao(vKey);
                return;
            }
        }
    }



    this.GetPosIcons = function () {
        var vParam = Array();
        for (n in this.Icons) {
            vParam.push({ key: n, top: this.Icons[n].top, left: this.Icons[n].left });
        }
        return JSON.stringify(vParam);
    }


    this.AppId = vAppId;
    this.id = vId;
    this.ConteinerFather = ConteinerFather;

    this.Obj = document.getElementById(this.id);
    if (!this.Obj) {
        alert("Object '" + this.id + "' Not Found");
        return;
    }

    var TmpThis = this;
    $(this.Obj).css({ position: "absolute", top: "0px", left: "0px" });
    this.Left = 0;
    this.Top = 0;

    this.Obj.innerHTML = '';
//    this.Obj.style.overflow = "auto";

    

    


    this.ControlVar = new ClassAraGenVarSend(this);
    this.ControlVar.AddPrototype("Top");
    this.ControlVar.AddPrototype("Left");
    this.ControlVar.AddPrototype("Width");
    this.ControlVar.AddPrototype("Height");
    this.ControlVar.AddPrototype("GetPosIcons()");

    this.Anchor = new ClassAraAnchor(this);

});