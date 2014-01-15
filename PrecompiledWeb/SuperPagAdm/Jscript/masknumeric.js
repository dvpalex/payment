//Formata valores de moeda

function MaskNumeric( m , scriptOnKeyDown)
{
	var rx = {"x": "A-Za-z", "#": "0-9", "*": "A-Za-z0-9" };
	
	this.mask = m;
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
	
	//Regular expression para validacao
	this.regValidNumberOrAlpha = new RegExp("[" + rx["*"] + "]", "");
	this.regexpInvalidValues = /[^[0-9.,]*]*/gi;
	this.regexpReplaceGroupingSymbol = new RegExp("[\d"+this.groupingSymbol+"]*", "g");
	this.regexpReplaceDecimalSymbol = new RegExp("\\"+this.decimalSymbol+"]", "");
	this.regexpReplaceAllDecimalSymbol = new RegExp("\\"+this.decimalSymbol+"]", "g");
	// replace all non-place holders from the mask
	this.regexpReplaceNonPlaceHolders = /[^#0.,]*/gi;
	this.regexpPadZeros = /[^0#]+/gi;
	this.regexpRemovePlaceHolders = /[#0]+(_|,|.)[#0]{3}/;
	
	//versão 1.0 - !/^[\$€%£¥R$]?((\$?[\ +-]?([0#]{1,3}(.|\ |\ |_))?[0#]*([\.,][0#]*)?)|([\+-]?\([\+-]?([0#]{1,3}(.|\ |\ |_))?[0#]*([\.,][0#]*)?\)))[\$€%£¥]?$/
	this.regValidMask = /^[\$€%£¥R$]?((\$?[\ +-]?(\[\d\])?([0#]{1,3}(.|\ |\ |_))?[0#]*([\.,][0#]*)?)|([\+-]?\([\+-]?(\[\d\])?([0#]{1,3}(.|\ |\ |_))?[0#]*([\.,][0#]*)?\)))[\$€%£¥]?$/;
}

MaskNumeric.prototype.attach = function (o)
{
	// check to see if an invalid mask operation has been entered
	//if( !this.regValidMask.test(this.mask) )
	//	return false;

	if ((o.readonly == null) || (o.readonly == false))
	{
		var concat = this.scriptOnKeyDown == null? "": this.scriptOnKeyDown;
		
		o.onclick		= new Function("return " + this.ref + ".storeCaret(this, true, event)");
		o.onkeydown		= new Function(concat + "return " + this.ref + ".storeCaret(this, false, event)");
		o.onkeypress	= new Function("return " + this.ref + ".OnKeyPress(event, this)");
		o.onmouseup		= new Function("return " + this.ref + ".storeCaret(this, true, event)");
		o.onkeyup		= new Function("return " + this.ref + ".OnKeyUp(this, event)");

		var x = o.currentStyle.fontFamily.split(",");
		if ( x.length > 0 )
			this.fontname = x[0].toLowerCase();
			
		this.fontsize = o.currentStyle.fontSize;
		
		//troco a fonte do input caso não exista na lista
		changeFont( o, this );
	}
}

MaskNumeric.prototype.OnKeyPress= function (e, o) {
	this.CheckMaskValue_n(o, e); 
	this.ChangePositionCursor(o, e);
	return true;
}

MaskNumeric.prototype.OnKeyUp= function ( o, e ) {
	if (o.createTextRange) 
		o.caretPos = document.selection.createRange().duplicate();

	if ( o.shiftSelection == true || o.selstart > o.value.length ) 
	{
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

MaskNumeric.prototype.storeCaret=function (maskControl , recalcstart, evt) 
{	
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

MaskNumeric.prototype.CheckMaskValue_n=function(maskControl, evt)
{
    if ( evt.type == "keypress" ) {    
        if ((evt.keyCode < 48) || (evt.keyCode > 57)){}
		else 
		{
			var key = String.fromCharCode(evt.keyCode);
			maskControl.keyCode = evt.keyCode;

			if (maskControl.createTextRange && maskControl.caretPos ){
				maskControl.caretPos.text = key;
				maskControl.caretPos.select();
			}
			else
				maskControl.value = key;
		}
	}
	evt.keyCode = 0;
}

MaskNumeric.prototype.ChangePositionCursor=function(maskControl, evt)
{
	//home, end, shift, ctrl	
	var teclasinvalidas = "36;35;16;17;";
	
	if ( maskControl.keyCode == 0 && (teclasinvalidas.indexOf(evt.keyCode+";")>-1) )
	{}
	else {
		var moveText = true;
		
		//verifico se não está selecionado o texto
		if ( evt.keyCode == 39 && (evt.shiftKey || evt.ctrlKey ) ) moveText = false;
		if ( evt.keyCode == 37 && ( evt.shiftKey || evt.ctrlKey ) ) moveText = false;
				
		if ( moveText && evt.keyCode == 8 && (maskControl.selstart > 0)) 
			maskControl.selstart -= 1;
		else if ( moveText &&  (maskControl.keyCode > 47) && (maskControl.keyCode < 58) )  {
			if ( maskControl.value.length > maskControl.oldvalue )
					maskControl.selstart += maskControl.value.length - maskControl.oldvalue + 1;
			else if (  
				(maskControl.selstart + 1) <= maskControl.value.length ) 
			{
				maskControl.selstart += 1;
				
				var find_next_valid_digit =
					(maskControl.selstart) > maskControl.value.length ? 
						maskControl.value.length : 
						maskControl.value.charAt(maskControl.selstart) != this.decimalSymbol && 
							!this.regValidNumberOrAlpha.test( maskControl.value.charAt(maskControl.selstart) ) ;

				//faz loop ate posicionar o cursor em um caracter numerico
				while ( find_next_valid_digit ) {
					maskControl.selstart += 1; //soma um
					// se já chegou ao fim da mascara, sai fora
					if (maskControl.selstart >= maskControl.value.length) break;
					find_next_valid_digit = 
						!(this.regValidNumberOrAlpha.test( maskControl.value.charAt(maskControl.selstart) ) )
				}
			}
			else
				maskControl.selstart = maskControl.value.length;
				
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