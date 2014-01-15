//Formata valores de moeda

function MaskDecimal(m, scriptOnKeyDown)
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

MaskDecimal.prototype.attach = function (o)
{
	// check to see if an invalid mask operation has been entered
	if( !this.regValidMask.test(this.mask) )
		return false;

	if ((o.readonly == null) || (o.readonly == false))
	{
		var concat = this.scriptOnKeyDown == null? "": this.scriptOnKeyDown;
		
		o.onclick		= new Function("return " + this.ref + ".storeCaret(this, true, event)");
		o.onkeydown		= new Function(concat + "return " + this.ref + ".storeCaret(this, false, event)");
		o.onkeypress	= new Function("return " + this.ref + ".OnKeyPress(event, this)");
		o.onmouseup		= new Function("return " + this.ref + ".storeCaret(this, true, event)");
		o.onkeyup		= new Function("return " + this.ref + ".OnKeyUp(this, event)");
		o.onblur		= new Function("return " + this.ref + ".CheckMaskValue_n(this, event);");

		var x = o.currentStyle.fontFamily.split(",");
		if ( x.length > 0 )
			this.fontname = x[0].toLowerCase();
			
		this.fontsize = o.currentStyle.fontSize;
		
		//troco a fonte do input caso não exista na lista
		changeFont( o, this );
	}
}

MaskDecimal.prototype.OnKeyPress= function (e, o) {
	this.CheckMaskValue_n(o, e); 
	this.ChangePositionCursor(o, e);
	return true;
}

MaskDecimal.prototype.OnKeyUp= function ( o, e ) {
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

MaskDecimal.prototype.storeCaret=function (maskControl , recalcstart, evt) 
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

MaskDecimal.prototype.CheckMaskValue_n=function(maskControl, evt)
{
    if ( evt.type == "keypress" )
    {    
		//verifica se foi digitado o char decimal 
		var aceitatecla = false;
		aceitatecla = (String.fromCharCode(evt.keyCode) == this.decimalSymbol );
		
        if (!aceitatecla && (evt.keyCode < 48) || (evt.keyCode > 57))
		{}
		else {		
			var key = String.fromCharCode(evt.keyCode);
			var textfmt = "";
			var possel = 0;
			maskControl.keyCode = evt.keyCode;
			
			var atualizavalor = true;
			
			if ( maskControl.selstart == 0 && key == "0" )
				atualizavalor = false;

			if ( atualizavalor ) {
				if ( (!aceitatecla || (aceitatecla && maskControl.value.indexOf(key) == -1 ) ) &&
					maskControl.createTextRange && maskControl.caretPos){
					maskControl.caretPos.text = key;
					maskControl.caretPos.select();
				}
				else if ( !aceitatecla && !replacedtext )
					maskControl.value = key;
			}
			
			textfmt = maskControl.value;
			maskControl.oldvalue = textfmt.length;
			maskControl.value = this.formatCurrency( textfmt, false );
		}
	}	
	else if ( evt.type == "blur" ) {
		maskControl.value = this.formatCurrency( maskControl.value, true );
		maskControl.selstart = 0;
	}
	
	evt.keyCode = 0;
}

MaskDecimal.prototype.ChangePositionCursor=function(maskControl, evt)
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
					if (maskControl.selstart >= this.mask.length) break;
					find_next_valid_digit = 
						!(this.regValidNumberOrAlpha.test( maskControl.value.charAt(maskControl.selstart) ) )
				}
			}
			else
				maskControl.selstart = maskControl.value.length;
				
		} else {
			//procura simbolo decimal			
			if ( String.fromCharCode(maskControl.keyCode) == this.decimalSymbol 
				&& maskControl.value.indexOf(this.decimalSymbol) > -1)
				maskControl.selstart = maskControl.value.indexOf(this.decimalSymbol)+1;
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

MaskDecimal.prototype.formatCurrency=function( value, update ) 
{
	var v = value, m = this.mask;

	//retiro os valores inválios
	v = v.replace(this.regexpInvalidValues, "");
	
	if ( v.length == 0 ) return "";
	
	//tiro o char de agrupamento
	v = v.replace(this.regexpReplaceGroupingSymbol, "");
	
	//deixo só um char decimal
	v = v.replace(this.regexpReplaceDecimalSymbol, "d");
	v = v.replace(this.regexpReplaceAllDecimalSymbol, "").replace(/d/, this.decimalSymbol);
	
	var vleft, vrigth;
	
	vleft = v.indexOf(this.decimalSymbol) > -1 ? v.split(this.decimalSymbol)[0] : v;
	vrigth = v.indexOf(this.decimalSymbol) > -1 ? v.split(this.decimalSymbol)[1] : null;
	
	//verifica numero de grupos no máximo
	var maxgroups = null;
	maxgroups = m.replace(/([^[\d]])*([0#,.])/g, "");
	maxgroups = maxgroups.length > 0 ? maxgroups = maxgroups.replace(/[^\d]/g , "") : 10; //seto o max group máximo com 10 == ###. 10 vezes
	
	// replace all non-place holders from the mask
	m = m.replace(this.regexpReplaceNonPlaceHolders, "");

	/*
		make sure there are the correct number of decimal places
	*/
	
	var fmt = vrigth == null ? "" : String(vrigth);
	
	/*
		pad the int with any necessary zeros
	*/	
	
	// get number of digits before decimal point in mask
	var im = (m.indexOf(this.decimalSymbol) > -1 ) ? m.split(this.decimalSymbol)[0] : m;
	var im = im.replace(this.regexpPadZeros, "");
	// find the first zero, which indicates the minimum length
	// that the value must be padded w/zeros
	var mv = im.indexOf("0")+1;
	// if there is a zero found, make sure it's padded
	if( mv > 0 ){
		mv = im.length - mv + 1;
		while( vleft.length < mv ) vleft = "0" + vleft;
	}
	
    /*
		check to see if we need commas in the thousands place holder
	*/
	var outchars = "";
	
	if( this.regexpRemovePlaceHolders.test(m) ){
		
		//verifica o tamanho dos grupos
		outchars = (parseInt(maxgroups,10) * 3) < vleft.length ? vleft.substr( parseInt(maxgroups) * 3 ) : "";
		vleft = vleft.substr( 0, vleft.length - outchars.length );
		vrigth = vrigth == null && outchars != "" ? vrigth = parseInt( outchars, 10 ) : vrigth;
		
		// add the commas as the place holder		
		var x = [], i=0, n=Number(vleft);
		while( n > 999 ){
			if ( i < maxgroups ) {
				x[i] = "00" + String(n%1000);
				x[i] = x[i].substring(x[i].length - 3);
			} else
				outchars += String(n%1000);

			n = Math.floor(n/1000);
			i++;
		}
		
		x[i] = String(n%1000);
		vleft = x.reverse().join(this.groupingSymbol);
	}

	if ( outchars.length > 0 ) //verifica os caracteres foras e coloca nas casas decimais
		fmt = outchars + fmt;
	
	// get number of digits after decimal point in mask
	var dm = (m.indexOf(this.decimalSymbol) > -1 ) ? m.split(this.decimalSymbol)[1] : "";
	if( dm.length == 0 ){
		vi = String(Math.round(Number(vleft)));
		vd = "";
	} else if ( fmt != "" || update ) {
		
		fmt = fmt.substr(0, dm.length );
		
		if ( update ) {
			// find the last zero, which indicates the minimum number
			// of decimal places to show
			var md = dm.lastIndexOf("0")+1;
			// if the number of decimal places is greater than the mask, then round off
			if( fmt.length > dm.length ) fmt = String(Math.round(Number(fmt.substring(0, dm.length + 1))/10));
			// otherwise, pad the string w/the required zeros
			else while( fmt.length < md ) fmt += "0";
		}
	}

	/*
		combine the new value together
	*/	
	v = vleft;
	if ( vrigth != null || update) v += this.decimalSymbol ;
	if ( vrigth > 0 || fmt.length > 0 || update ) v += String(fmt);
	
	return v;	
}