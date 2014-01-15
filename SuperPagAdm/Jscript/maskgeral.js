//Formatos genéricos

function MaskGeneric( m , scriptOnKeyDown ) {

	var rx = {"x": "A-Za-z", "#": "0-9", "*": "A-Za-z0-9" };
	
	this.mask = m;
	this.type = "string";
	this.error = [];
	this.errorCodes = [];
	this.value = "";
	this.strippedValue = "";
	this.allowPartial = false;
	this.ref = this;
	this.groupingSymbol = ".";
	this.decimalSymbol = ",";
	this.id = MaskAPI.instances++;
	this.ref = "MaskAPI.objects['" + this.id + "']";
	this.scriptOnKeyDown = scriptOnKeyDown;
	MaskAPI.objects[this.id] = this;
	
	//---Regular expression
	this.regValidNumberOrAlpha = new RegExp("[" + rx["*"] + "]", "");
	
	//valida # e 0
	this.regValidMask = new RegExp("[#0 ]", "");

	//font usada
	this.fontname = "arial";
	this.fontsize = "10pt";

	//se sobreescreve os caracteres se a mascara for completada
	this.replaceChar = true;
}

MaskGeneric.prototype.attach = function (o)
{
	if ((o.readonly == null) || (o.readonly == false))
	{
		var concat = this.scriptOnKeyDown == null? "": this.scriptOnKeyDown;
		
		o.onclick			= new Function("return " + this.ref + ".storeCaret(this, true, event)");
		//caso o cara selecione sem clicar
		o.onmouseup			= new Function("return " + this.ref + ".storeCaret(this, true, event)");
		o.onfocus			= new Function("return " + this.ref + ".storeCaret(this, true, event)");
		o.onkeydown			= new Function(concat + "return " + this.ref + ".storeCaret(this, false, event)");
		o.onkeypress		= new Function("return " + this.ref + ".OnKeyPress(this, event)");
		//o.onblur			= new Function("return " + this.ref + ".CheckMaskValue(this, event);");
		o.onkeyup			= new Function("return " + this.ref + ".OnKeyUp(this, event)");
		
		o.selstart = 0;
		
		var x = o.currentStyle.fontFamily.split(",");
		if ( x.length > 0 )
			this.fontname = x[0].toLowerCase();
		this.fontsize = o.currentStyle.fontSize;

		//troco a fonte do input caso não exista na lista
		changeFont( o, this );	
	}
}

MaskGeneric.prototype.OnKeyPress= function ( o, e ) {
	this.CheckMaskValue(o, e); 
	this.ChangePositionCursor(o, e);
	return true;
}

MaskGeneric.prototype.OnKeyUp= function ( o, e ) {
	
	if (o.createTextRange) 
		o.caretPos = document.selection.createRange().duplicate();

	if ( o.shiftSelection == true || o.selstart > o.value.length ) {
		//mostra posição real	
		var pos = verificaPosicao( 
			o.value, 
			o.caretPos.offsetLeft, 
			o.scrollLeft,
			this.fontname,
			this.fontsize );

		o.selstart = pos;
	}
}

MaskGeneric.prototype.storeCaret= function(maskControl , recalcstart, evt) {
	
	if (maskControl.createTextRange) 
		maskControl.caretPos = document.selection.createRange().duplicate();

	if ( recalcstart ) {
		//mostra posição real	
		var pos = verificaPosicao( 
			maskControl.value, 
			maskControl.caretPos.offsetLeft, 
			maskControl.scrollLeft,
			this.fontname,
			this.fontsize );

		maskControl.selstart = pos;
	}
	else if ( evt != null && !evt.shiftKey && !evt.ctrlKey )
	{	
		if ( evt.keyCode == 36 || evt.keyCode == 33 ) //home, page up
			maskControl.selstart = 0;
		else if ( evt.keyCode == 35 || evt.keyCode == 34 ) //end page down
			maskControl.selstart = maskControl.value.length == 0 ? 0 : maskControl.value.length;
		else if ( evt.keyCode == 39 && maskControl.selstart < maskControl.value.length ) // ->
		{
			//foi selecionado todo o texto e é seta esq.
			if (maskControl.caretPos.text.length == maskControl.value.length)
				maskControl.selstart = maskControl.value.length;
			else
				maskControl.selstart += 1;
		}
		else if ( evt.keyCode == 37 && maskControl.selstart > 0 ) // <-
		{	
			//foi selecionado todo o texto e é seta esq.
			if (maskControl.caretPos.text.length == maskControl.value.length)
				maskControl.selstart = 0;
			else
				maskControl.selstart -= 1;
		}
		else if ( evt.keyCode == 8 && maskControl.selstart > 0 ) // backspace
			maskControl.selstart -= 1;
			
		maskControl.shiftSelection = false;
		
	} else if ( evt.shiftKey || evt.ctrlKey ) {		
		maskControl.shiftSelection = true;

	}
	
	if ( isNaN(maskControl.selstart) ) maskControl.selstart = 0;	
	if (evt != null ) evt.returnValue = true;
	
}

MaskGeneric.prototype.CheckMaskValue=function (maskControl, evt)
{
    if ( evt.type == "keypress" ) {
		//47 é /
        if ((evt.keyCode < 48) || (evt.keyCode > 57))
		{}
		else 
		{		
			var key = String.fromCharCode(evt.keyCode);
			var textfmt = "";
			var possel = 0;
			maskControl.keyCode = evt.keyCode;
			
			var replacedtext = false;

			//verifico se é menor ou igual ao tam da mascara e 
			//verifico se não escedeu o tam da mascara pela posicao  + 1
			if ( String(this.mask).length <= maskControl.value.length &&
				(maskControl.selstart+1) < String(this.mask).length && 
				maskControl.caretPos.text.length == 0)
			 {
				if ( this.replaceChar && 
					maskControl.caretPos.text.length < maskControl.value.length) 
				{
					if ( 
						!(this.regValidMask.test( this.mask.charAt(maskControl.selstart) ) )
					) maskControl.selstart += 1;
					
					maskControl.value = maskControl.value.substr( 0, maskControl.selstart ) +
						key +  maskControl.value.substr(  maskControl.selstart+1 );
					replacedtext = true;
				}
			}

			if (maskControl.createTextRange && maskControl.caretPos && !replacedtext)
			{
				maskControl.caretPos.text = key;
				maskControl.caretPos.select();
			}
			else if ( !replacedtext )
				maskControl.value = key;

			//TODO: verificar
			this.allowPartial = true;
			
			textfmt = maskControl.value;
			maskControl.value = this.setGeneric(textfmt, true);
		}
	}	
	else if ( evt.type == "blur" ) {
		if ( maskControl.value.length > 0 ){}
			maskControl.value = this.setGeneric(textfmt, true);
	}
	evt.keyCode = 0;
}

MaskGeneric.prototype.ChangePositionCursor=function (maskControl, evt)
{
	//home, end, shift, ctrl	
	var teclasinvalidas = "36;35;16;";
	
	if ( maskControl.keyCode == 0 && (teclasinvalidas.indexOf(evt.keyCode+";")>-1) )
	{}
	else {
		var moveText = true;
		
		//verifico se não está selecionado o texto
		if ( evt.keyCode == 39 && evt.shiftKey ) moveText = false;
		if ( evt.keyCode == 37 && evt.shiftKey ) moveText = false;
	

		if ( moveText &&  (maskControl.keyCode > 47) && (maskControl.keyCode < 58) )  {

			//flag se é um char diferente de 0-9,a-z
			var digit_sel_mask_special =
				!(this.regValidMask.test( this.mask.charAt(maskControl.selstart) ) )
			var digit_sel_special =
				!(this.regValidNumberOrAlpha.test( maskControl.value.charAt(maskControl.selstart) ) )

			if ( (digit_sel_mask_special && digit_sel_special)
				||
					(maskControl.value.length <= this.mask.length && 
						!(this.regValidMask.test( this.mask.charAt(maskControl.selstart+1) ) ) )
			)
				maskControl.selstart += 2; 
			else
				maskControl.selstart += 1;
		}

	  	if (moveText) {
    		var r = maskControl.createTextRange();
    		r.move('character', maskControl.selstart);
			r.select();
		}
	}
	
	maskControl.keyCode = 0;
	evt.keyCode = 0;
	evt.returnValue = false;
}

// ************************ GENERIC *********************** //

MaskGeneric.prototype.setGeneric = function (_v, _d){
	var v = _v, m = this.mask;
	var r = "x#*", rt = [], nv = "", t, x, a = [], j=0, rx = {"x": "A-Za-z", "#": "0-9", "*": "A-Za-z0-9" };

	// strip out invalid characters
	v = v.replace(new RegExp("[^" + rx["*"] + "]", "gi"), "");
	//if( (_d == true) && (v.length == this.strippedValue.length) ) v = v.substring(0, v.length-1);
	this.strippedValue = v;
	var b=[];
	for( var i=0; i < m.length; i++ ){
		// grab the current character
		x = m.charAt(i);
		// check to see if current character is a mask, escape commands are not a mask character
		t = (r.indexOf(x) > -1);
		// if the current character is an escape command, then grab the next character
		if( x == "!" ) x = m.charAt(i++);
		// build a regex to test against
		if( (t && !this.allowPartial) || (t && this.allowPartial && (rt.length < v.length)) ) rt[rt.length] = "[" + rx[x] + "]";
		// build mask definition table
		a[a.length] = { "char": x, "mask": t };
	}

	var hasOneValidChar = false;
	
	// if the regex fails, return an error	
	//if( !this.allowPartial && !(new RegExp(rt.join(""))).test(v) ) 
	// return this.throwError(1, "The value \"" + _v + "\" must be in the format " + this.mask + ".\n\nLa valeur \""+_v+"\" doit être dans le format "+this.mask+".", _v);
	
	// loop through the mask definition, and build the formatted string
	if ( !this.allowPartial && !/[a-zA-Z0-9]/.test(v) )
	{}
	else if( (this.allowPartial && (v.length > 0)) || !this.allowPartial ){
		for( i=0; i < a.length; i++ ){
			if( a[i].mask ){
				while( v.length > 0 && !(new RegExp(rt[j])).test(v.charAt(j)) ) v = (v.length == 1) ? "" : v.substring(1);
				if( v.length > 0 ){
					nv += v.charAt(j);
					hasOneValidChar = true;
				}
				j++;
			} else nv += a[i]["char"];
			if( this.allowPartial && (j > v.length) ) break;
		}
	}
	
	if( this.allowPartial && !hasOneValidChar ) nv = "";

	return nv;
}
