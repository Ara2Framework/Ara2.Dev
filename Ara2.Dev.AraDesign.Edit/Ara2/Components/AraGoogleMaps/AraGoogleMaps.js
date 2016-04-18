Ara.AraClass.Add('AraGoogleMaps', function (vAppId, vId, ConteinerFather) {
    // Eventos  ---------------------------------------
    this.Events = {};

    var TmpThis = this;
    this.Events.Click =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function (overlay, latlng, overlaylatlng) {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "Click", {
                    overlay: overlay,
                    latlng: latlng,
                    overlaylatlng: overlaylatlng
                });
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
    //---------------------------------------------------

  



    this.Active = function () {

        if (!this.map) {
            this.Obj.innerHTML = "Carregando Google Maps...";

            var ClassesGoogleCarregada = true;

            try {
                if (!google) {
                    ClassesGoogleCarregada = false;
                }
                else if (!google.maps) {
                    var TmpThis = this;
                    google.load("maps", "3",
	            	{
	            	    "callback": function () { TmpThis.Active(); },
	            	    other_params: "sensor=false"
	            	});
                    return;
                }
            }
            catch (err) {
                ClassesGoogleCarregada = false;
            }

            if (!ClassesGoogleCarregada) {
                var TmpThis = this;
                setTimeout(function () { TmpThis.Active(); }, 500);
                return;
            }



            var myOptions = {
                zoom: 2,
                center: new google.maps.LatLng(20, 0),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            try {
                this.map = new google.maps.Map(this.Obj, myOptions);
            }
            catch (err) {
                
                this.map = null;
                var TmpThis = this;
                setTimeout(function () { TmpThis.Active(); }, 500);
                return;
            }


            //this.map.setCenter(, 2);
            //this.map.setUIToDefault();

            
            var TmpThis = this;
            google.maps.event.addListener(this.map, 'click', function (overlay, latlng, overlaylatlng) { TmpThis.Events.Click.Function(overlay, latlng, overlaylatlng); });

            this.bounds = new google.maps.LatLngBounds();

            Ara.Tick.Send(1, this.AppId, this.id, "GoogleMapsActive", null);
        }
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


    this.SetVisible = function (vTmp) {
        if (vTmp)
            this.Obj.style.display = "block";
        else
            this.Obj.style.display = "none";
    }

    this.GetChange = function (vParans) {

    }


    this.FormResize = function () {
        if (this.Anchor)
            this.Anchor.FormResize();
    }

    this.getLng = function () {
        if (this.map != "") {
            var center = this.map.getCenter();
            return center.lng();
        }
        return 0;
    }

    this.getLat = function () {
        if (this.map != "") {
            var center = this.map.getCenter();
            return center.lat();
        }
        return 0;
    }

    this.getZoom = function () {
        if (this.map != "") {
            return this.map.getZoom();
        }
        return 0;
    }

    this.SearchPath = function (vSearch) {
        var vTmpThis = this;

        var GeoC = new google.maps.Geocoder(
        {
            key: this.Key
        });

        GeoC.geocode({ 'address': vSearch },
		function (result, status) {

		    for (var TmpN in result) {
		        result[TmpN].geometry.location.latitude = result[TmpN].geometry.location.lat();
		        result[TmpN].geometry.location.longitude = result[TmpN].geometry.location.lng();
		    }

		    var vParans = new vTmpThis.Janela.Ara.ClassAraComunicacao_Param();
		    vParans.Add("result", vTmpThis.Janela.JSON.stringify(result));
		    vTmpThis.Janela.Send(vTmpThis.id, "SearchPathResult", vParans, 1);
		});
    }

    this.SearchPoint = function (vX, vY) {
        var vTmpThis = this;

        var GeoC = new google.maps.Geocoder(
        {
            key: this.Key
        });

        var point = new google.maps.LatLng(parseFloat(vX), parseFloat(vY), true);
        GeoC.geocode({ 'latLng': point },
		    function (result, status) {
		        var vParans = new vTmpThis.Janela.Ara.ClassAraComunicacao_Param();
		        vParans.Add("result", vTmpThis.Janela.JSON.stringify(result));
		        vTmpThis.Janela.Send(vTmpThis.id, "SearchPointResult", vParans, 1);
		    }
	    );

    }

    this.Markers = new Array();

    this.MarkerCreate = function (vId, vX, vY, vimg, vimgshadow, vTexto, vImgW, vImgH) {

        var point = new google.maps.LatLng(parseFloat(vX), parseFloat(vY));
        this.bounds.extend(point);

        /*
        var baseIcon = new google.maps.Icon(this.Janela.DocumentJs.G_DEFAULT_ICON);
        baseIcon.shadow = vimgshadow; //"http://www.google.com/mapfiles/shadow50.png";
        baseIcon.iconSize = new GSize(vImgW, vImgH);  //(20, 34);
        baseIcon.shadowSize = new GSize(37, 34);
        baseIcon.iconAnchor = new google.maps.Point(9, 34);
        baseIcon.infoWindowAnchor = new google.maps.Point(9, 2);
        baseIcon.imageMap = [0, 0, 34, 0, 34, 34, 0, 34];

        var letteredIcon = new google.maps.Icon(baseIcon);
        letteredIcon.image = vimg;

        var marker = new GMarker(point, { icon: letteredIcon });
        var TmpAra = this.Janela.Ara;
        if (vTexto != "") {
        eval("GEvent.addListener(marker, 'click', function() { marker.openInfoWindowHtml('" + vTexto + "'); });");
        }
        this.map.addOverlay(marker);
        */

        var beachMarker = new google.maps.Marker({
            position: point,
            map: this.map,
            icon: vimg,
            shadow: vimgshadow
        });

        this.Markers[vId] = beachMarker;

        if (vTexto != "") {
            google.maps.event.addListener(beachMarker, 'click', function () {
                var infoWindow = new google.maps.InfoWindow();
                infoWindow.setContent(vTexto);
                infoWindow.open(this.map, beachMarker);
            });
        }
    }

    this.MarkerClearAll = function () {
        for (var vId in this.Markers) {
            this.Markers[vId].setMap(null);
        }
        this.Markers = new Array();

        this.bounds = new google.maps.LatLngBounds();
    }

    this.SetXY = function (vLat, vLng) {
        this.map.setCenter(new google.maps.LatLng(vLat, vLng));
    }
    this.SetXYZOOM = function (vLat, vLng, vZoom) {
        this.map.setCenter(new google.maps.LatLng(vLat, vLng));
        this.map.setZoom(vZoom);
    }

    this.Polygons = new Array();

    this.PolygonCreate =
	    function (
            vId,
		    vArrayP,
		    vTexto,
		    vBColor, vBOpacity, vLColor, vLSize, vLOpacity
	    ) {
	        var vP = new Array();

	        for (var Index in vArrayP) {
	            vP.push(new google.maps.LatLng(parseFloat(vArrayP[Index].X), parseFloat(vArrayP[Index].Y)));
	        }

	        var ObjMaps = this.map;
	        var TmpPolygon = new google.maps.Polygon(
            {
                paths: vP,
                strokeColor: vLColor,
                strokeOpacity: vLSize,
                strokeWeight: vLOpacity,
                fillColor: vBColor,
                fillOpacity: vBOpacity
            });

	        var TmpAra = this.Janela.Ara;
	        if (vTexto != '') {
	            google.maps.event.addListener(TmpPolygon, 'click', function () {
	                var infoWindow = new google.maps.InfoWindow();
	                infoWindow.setContent(vTexto);
	                infoWindow.open(this.map);
	            });
	            //eval("GEvent.addListener(TmpPolygon, 'click', function() { ObjMaps.openInfoWindowHtml(TmpPolygon.getBounds().getCenter(), '" + vTexto + "'); });");
	        }
	        //this.map.addOverlay(TmpPolygon);
	        TmpPolygon.setMap(this.map);
	        this.Polygons[vId] = TmpPolygon;
	    }

    this.PolygonClear = function () {
        for (var vId in this.Polygons) {
            this.Polygons[vId].setMap(null);
        }
        this.Polygons = new Array();
    }

    this.Circles = new Array();

    this.CircleCreate =
	    function (
            vId,
		    vRaio, vX, vY,
		    vTexto,
		    vBColor, vBOpacity, vLColor, vLSize, vLOpacity
	    ) {
	        /*
                var polyPoints = Array();
    
                var polyNumSides = 20;
                var polySideLength = 18;
    
                for (var a = 0; a < 21; a++) {
                    var aRad = polySideLength * a * (3.141516 / 180);
                    var pixelX = vX + vRaio * Math.cos(aRad);
                    var pixelY = vY + vRaio * Math.sin(aRad);
                    var polyPixel = new google.maps.Point(pixelX, pixelY);
                    var polyPoint = G_NORMAL_MAP.getProjection().fromPixelToLatLng(polyPixel, 8);
                    polyPoints.push(polyPoint);
                }
    
                var ObjMaps = this.map;
                var TmpPolygon = new google.maps.Polygon(polyPoints, vLColor, vLSize, vLOpacity, vBColor, vBOpacity);
    
                var TmpAra = this.Janela.Ara;
                if (vTexto != '')
                    eval("GEvent.addListener(TmpPolygon, 'click', function() { ObjMaps.openInfoWindowHtml(TmpPolygon.getBounds().getCenter(), '" + vTexto + "'); });");
                TmpPolygon.setMap(this.map);
                //this.map.addOverlay(TmpPolygon);
                */

	        var circle = new google.maps.Circle({
	            map: this.map,
	            radius: vRaio * 1000, // 3000 km
	            center: new google.maps.LatLng(vX, vY),
	            strokeColor: vLColor,
	            strokeOpacity: vLSize,
	            strokeWeight: vLOpacity,
	            fillColor: vBColor,
	            fillOpacity: vBOpacity
	        });

	        this.Circles[vId] = circle;

	        //circle.setCenter(new google.maps.Point(vX, vY));

	    }

    this.CircleClear = function () {
        for (var vId in this.Circles) {
            this.Circles[vId].setMap(null);
        }
        this.Circles = new Array();
    }

    this.MarkerCenter = function () {
        this.map.fitBounds(this.bounds);
        //this.map.setCenter(this.bounds.getCenter(), this.map.getBoundsZoomLevel(this.bounds));

        var vParans = new this.Janela.Ara.ClassAraComunicacao_Param();
        this.Janela.Send(this.id, "MarkerCenter", vParans, 1);
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

    $(this.Obj).click(function () { TmpThis.Events.Click.Function(); });

    this.ControlVar = new ClassAraGenVarSend(this);
    this.ControlVar.AddPrototype("Top");
    this.ControlVar.AddPrototype("Left");
    this.ControlVar.AddPrototype("Width");
    this.ControlVar.AddPrototype("Height");

    this.ControlVar.AddPrototype("getLng()");
    this.ControlVar.AddPrototype("getLat()");
    this.ControlVar.AddPrototype("getZoom()");

    this.Anchor = new ClassAraAnchor(this);

    this.Active();
});