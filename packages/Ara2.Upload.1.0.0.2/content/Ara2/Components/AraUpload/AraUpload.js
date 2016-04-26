Ara.AraClass.Add('AraUpload', function (vAppId, vId, ConteinerFather) {
    

    // Eventos  ---------------------------------------
    this.Events = {};

    var TmpThis = this;
    this.Events.Click =
    {
        Enabled: false,
        SetEnabled: function (vValue) {
            var TmpThis2 = this;
            if (vValue && this.PrimeiraAtivacaoEvento != true) {
                $(TmpThis.Obj).bind('contextmenu click', function (e) { TmpThis2.Function(e); return false; });
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



    this.Events.PermissionToSend =
    {
        Enabled: false,
        ThreadType: 1, // Mult_thread
        Function: function (vFileId, vFileName) {
            if (TmpThis.Events.Click.PermissionToSend) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "PermissionToSend", { FileId: vFileId, FileName: vFileName });
            }
        }
    };
    //---------------------------------------------------


    this.vSpan = "";
    this.vValorAntigo = "";
    this.vValorAntigo_class = "";

    this.ResponseUploadJson = function (vResponseJson) {
        this.ObjUploader._handler._options.AraReponse(vResponseJson);
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

    this.Width = null;
    this.SetWidth = function (vTmp,vServer) {
        if (this.Width != vTmp) {
            this.Width = vTmp;
            if (vServer) this.ControlVar.SetValueUtm('Width', this.Width);
            this.Obj.style.width = vTmp;
            if (this.Anchor != null)
                this.Anchor.RenderChildren();
        }
    }

    this.Height = null;
    this.SetHeight = function (vTmp, vServer) {
        if (this.Height != vTmp) {
            this.Height = vTmp;
            if (vServer)  this.ControlVar.SetValueUtm('Height', this.Height);
            this.Obj.style.height = vTmp;
            if (this.Anchor != null)
                this.Anchor.RenderChildren();
        }
    }



    this.SetVisible = function (vTmp) {
        if (vTmp)
            this.Obj.style.display = "block";
        else
            this.Obj.style.display = "none";
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

    this.SetTypePosition = function (vTypePosition) {
        if (vTypePosition != "static")
            $(this.Obj).css({ position: vTypePosition });
        else {
            $(this.Obj).css({ position: "", left: "", top: "" });
        }
    }

    this._Text = "Enviar";
    this.SetText = function (vValue) {
        this._Text = vValue;
        this.ReCreate();
    };

    this._cancelButtonText = "Cancelar";
    this.SetcancelButtonText = function (vValue) {
        this._cancelButtonText = vValue;
        this.ReCreate();
    };

    this._failUploadText = "Envio Falhou";
    this.SetFailUploadText = function (vValue) {
        this._failUploadText = vValue;
        this.ReCreate();
    };

    this.ReCreateTimer = null;
    this.ReCreate = function () {
        if (this.ReCreateTimer!=null)
            clearTimeout(this.ReCreateTimer);
        var vTmpThis = this;
        this.ReCreateTimer = setTimeout(function () {

            vTmpThis.ReCreateAncy();
            vTmpThis.ReCreateTimer = null;
        }, 100);
    };

    this.ReCreateAncy = function () {
        this.ObjUploader = new qq.FileUploader({
            element: TmpThis.Obj,
            action: Url,
            uploadButtonText: this._Text,
            cancelButtonText: this._cancelButtonText,
            failUploadText: this._failUploadText
        });
    };


    this.AppId = vAppId;
    this.id = vId;
    this.ConteinerFather = ConteinerFather;

    this.Obj = document.getElementById(this.id);
    if (!this.Obj) {
        alert("Object '" + this.id + "' Not Found");
        return;
    }

    var Url = Ara.Tick.Url; 
    if (Url.indexOf("?") == -1)
        Url += "?";
    else
        Url += "&";

    Url += "vrndl=" + new Date().getTime();
    Url += "&Ara2=1";
    Url += "&AppId=" + this.AppId;
    Url += "&SessionID=" + Ara.SessionId;
    Url += "&ObjId=" + this.id;
    Url += "&Event=sendfile";
    //Url += "&returnisjavascript=0";


    var TmpThis = this;
    this.ReCreate();

    $(this.Obj).css({ position: "absolute", top: "0px", left: "0px" });

    this.ControlVar = new ClassAraGenVarSend(this);
    this.ControlVar.AddPrototype("Top");
    this.ControlVar.AddPrototype("Left");
    this.ControlVar.AddPrototype("Width");
    this.ControlVar.AddPrototype("Height");
    this.ControlVar.AddPrototype("IsDestroyed()");

    this.Anchor = new ClassAraAnchor(this);

});