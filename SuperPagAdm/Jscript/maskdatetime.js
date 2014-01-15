//Formata data com mascara

function MaskDateTime( m , scriptOnKeyDown ) {

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

	//---Regular expression
	this.regValidNumberOrAlpha = new RegExp("[" + rx["*"] + "]", "");
	// split mask into array, to see position of each day, month & year
	this.arrayDayMonthYear = this.mask.split(/[^mdy]+/);
	// split mask into array, to get delimiters
	this.arrayDelimiters = this.mask.split(/[mdy]+/);
	this.Delimiter = this.arrayDelimiters.length > 0 ? this.arrayDelimiters[0] : "";
	
	//Flag para sobreescrever dias, mes, ano no input
	this.replaceChar = true;
	this.validInOnBlur = false; //nao valida data quando perde o foco
	this.autoCompleteDate = false;
	
	//font usada
	this.fontname = "arial";
	this.fontsize = "10pt";
}

MaskDateTime.prototype.attach = function (o)
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
		o.onblur			= new Function("return " + this.ref + ".CheckMaskValue(this, event);");
		o.onkeyup			= new Function("return " + this.ref + ".OnKeyUp(this, event)");
		
		o.selstart = 0;
		
		var x = o.currentStyle.fontFamily.split(",");
		if ( x.length > 0 )
			this.fontname = x[0].toLowerCase();
		this.fontsize = o.currentStyle.fontSize;
		
		//troco a fonte do input caso não exista na lista
		changeFont( o, this );		

/*
		alert(this.fontname);
		alert(this.fontsize);
*/		
	}
}

MaskDateTime.prototype.OnKeyPress= function ( o, e ) {
	this.CheckMaskValue(o, e); 
	this.ChangePositionCursor(o, e);
	return true;
}

MaskDateTime.prototype.OnKeyUp= function ( o, e ) {
	if (o.createTextRange) 
		o.caretPos = document.selection.createRange().duplicate();

	if ( o.shiftSelection == true || o.selstart > o.value.length)
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

MaskDateTime.prototype.storeCaret= function(maskControl , recalcstart, evt) {
	
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

	} else if ( evt.shiftKey || evt.ctrlKey )		
		maskControl.shiftSelection = true;
	
	if ( isNaN(maskControl.selstart) ) maskControl.selstart = 0;	
	if (evt != null ) evt.returnValue = true;
	
}

MaskDateTime.prototype.CheckMaskValue=function (maskControl, evt)
{
    if ( evt.type == "keypress" ) {
		//47 é /
        if ((evt.keyCode < 48) || (evt.keyCode > 57))
		{}
		else {		
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
						!(this.regValidNumberOrAlpha.test( this.mask.charAt(maskControl.selstart) ) )
					) maskControl.selstart += 1;
					
					maskControl.value = maskControl.value.substr( 0, maskControl.selstart ) +
						key +  maskControl.value.substr(  maskControl.selstart+1 );
					replacedtext = true;
				}
			}

			if (maskControl.createTextRange && maskControl.caretPos && !replacedtext){
				maskControl.caretPos.text = key;
				maskControl.caretPos.select();
			}
			else if ( !replacedtext )
				maskControl.value = key;
			
			textfmt = maskControl.value;
			maskControl.value = this.formatData( textfmt, false );
		}
	}	
	else if ( evt.type == "blur" ) 
	{
		if ( maskControl.value.length > 0 )
			maskControl.value = this.formatData( maskControl.value, this.validInOnBlur );
	}
	evt.keyCode = 0;
}

MaskDateTime.prototype.ChangePositionCursor=function (maskControl, evt)
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
				!(this.regValidNumberOrAlpha.test( this.mask.charAt(maskControl.selstart) ) )
			var digit_sel_special =
				!(this.regValidNumberOrAlpha.test( maskControl.value.charAt(maskControl.selstart) ) )

			if ( (digit_sel_mask_special && digit_sel_special)
				||
					(maskControl.value.length <= this.mask.length && !(this.regValidNumberOrAlpha.test( this.mask.charAt(maskControl.selstart+1) ) ) )
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

MaskDateTime.prototype.formatData=function ( value, checkdate ) {
	var v = value, m = this.mask;
	var e, mm, dd, yy, x, s;

	// convert the string into an array in which digits are together	
	e = v.split(this.Delimiter);

	if( this.arrayDelimiters[0].length == 0 ) this.arrayDelimiters.splice(0, 1);

	//for para pegar ddmmyyyy - sem barras
	for( var i=0; i < this.arrayDayMonthYear.length; i++ ){
		x = this.arrayDayMonthYear[i].charAt(0).toLowerCase();
		if( x == "m" && e[i] != undefined)		mm = e[i] == "" ? null : e[i];
		else if( x == "d" && e[i] != undefined) dd = e[i] == "" ? null : e[i];
		else if( x == "y" && e[i] != undefined) yy = e[i] == "" ? null : e[i];
	}
	
	var outchar = null;
	
	var arrayL = this.arrayDayMonthYear.length
	// valida as posições dos campos	
	for( var i=0; i < arrayL; i++ ){
		x = this.arrayDayMonthYear[i].charAt(0).toLowerCase();
		
		var maskparam = null;
		var arrayCurrentL = this.arrayDayMonthYear[i].length;
		
		if( x == "m" && mm != undefined) maskparam = mm;
		else if( x == "d" && dd != undefined) maskparam = dd;
		else if( x == "y" && yy != undefined) maskparam = yy;
		
		//verifico se sobrou numeros
		if ( outchar != null ) {
			if ( this.replaceChar && maskparam == null)
				maskparam = outchar + (maskparam == null ? "" : maskparam);
			else if ( maskparam != null && String(maskparam).length < arrayCurrentL )
				maskparam = outchar + maskparam.toString();
			else if ( maskparam != null && i < (arrayL-1))
				maskparam = outchar + maskparam;
			else if ( maskparam != null && i == (arrayL-1) && maskparam.length == arrayCurrentL)
				maskparam = outchar + maskparam.substr(1, arrayCurrentL-1);
		}
		
		if ( maskparam != null && String(maskparam).length > arrayCurrentL ) {
			//alert("maior " + maskparam + " " + String(maskparam).length + " > " +  a[i].length);
			outchar = String(maskparam).substr( arrayCurrentL , String(maskparam).length-arrayCurrentL );
			if( x == "m" )
				mm = String(maskparam).substr( 0, arrayCurrentL );
			if( x == "d" )
				dd = String(maskparam).substr( 0, arrayCurrentL );
			if( x == "y" )
				yy = String(maskparam).substr( 0, arrayCurrentL );
		} else if ( maskparam != null )
		{
			if( x == "m" ) mm = maskparam;
			if( x == "d" ) dd = maskparam;
			if( x == "y" ) yy = maskparam;
			outchar = null;
		}		
	}

	// if year is abbreviated, guess at the year
	// if( yy != null && yy.length != 0 && yy.length < 3){
	//	yy = 2000 + parseInt(yy, 10);
	//	if( (new Date()).getFullYear()+20 < yy ) yy = yy - 100;
	// }

	var fmtret = new String();
	
	var find_day = false;
	var find_month = false;
	var find_year = false;
		
	var prevSepChar = "";
	var prevExistChar = true;
	
	//monta a string de retorno
	for( var i=0; i < this.arrayDayMonthYear.length; i++ ){
		x = this.arrayDayMonthYear[i].charAt(0).toLowerCase();
		
		if ( x == "d" ) {
			if ( checkdate && this.autoCompleteDate && dd == undefined )
				dd = (new Date()).getDate();
				
			if ( dd != undefined) {
				
				while( !find_day ) {
					find_day = this.arrayDayMonthYear[i].length == String(dd).length;					
					if ( !find_day && checkdate )
						dd = paddingLeft( makeFormat("0", this.arrayDayMonthYear[i].length), dd);
					else
						break;
				}
				
				if ( !prevExistChar ) fmtret += prevSepChar;
				
				fmtret += dd;
			
				if (find_day)
					//verifica se tem a mascara
					if ( i <= ( this.arrayDelimiters.length-1 ) ) fmtret += this.arrayDelimiters[i]; // coloco a barra;
				
				prevSepChar = this.arrayDelimiters[i]; //guardo o caracter de separação
				prevExistChar = find_day; //flag de existencia do char de separaão
			} else prevExistChar = true;
		}
		else if ( x == "m" ) {
			if ( checkdate && this.autoCompleteDate && mm == undefined )
				mm = (new Date()).getMonth();
				
			if ( mm != undefined ) {
			
				while( !find_month ) {
					find_month = this.arrayDayMonthYear[i].length == String(mm).length;
					if ( !find_month && checkdate )
						mm = paddingLeft( makeFormat("0", this.arrayDayMonthYear[i].length), mm);
					else
						break;
				}

				if ( !prevExistChar ) fmtret += prevSepChar;

				fmtret += mm;
				
				if (find_month)
					//verifica se tem a mascara
					if ( i <= ( this.arrayDelimiters.length-1 ) ) fmtret += this.arrayDelimiters[i]; // coloco a barra;

				prevSepChar = this.arrayDelimiters[i]; //guardo o caracter de separação
				prevExistChar = find_month; //flag de existencia do char de separaão
			} else prevExistChar = true;
		}
		else if ( x == "y" ) {
			if ( checkdate && this.autoCompleteDate && yy == undefined )
				yy = (new Date()).getFullYear();
				
			if ( yy != undefined ) {

				while( !find_year ) {
					find_year = this.arrayDayMonthYear[i].length == String(yy).length;
					if ( !find_year && checkdate )
						yy = paddingLeft( makeFormat("0", this.arrayDayMonthYear[i].length), yy);
					else
						break;
				}
			
				if ( !prevExistChar ) fmtret += prevSepChar;

				fmtret += yy;
				
				if (find_year)
					if ( i <= ( this.arrayDelimiters.length-1 ) ) fmtret += this.arrayDelimiters[i]; // coloco a barra;

				prevSepChar = this.arrayDelimiters[i]; //guardo o caracter de separação
				prevExistChar = find_year; //flag de existencia do char de separaão
			} else prevExistChar = true;
		}		
	}
	
	if ( find_day && find_month && this.autoCompleteDate && yy == null )
		yy = (new Date()).getFullYear();
	else if ( find_day && find_month && find_year ) {
		var d = new Date(yy, mm, dd);
		if ( isNaN(d) || (checkdate &&
			(   (d.getDate() != parseInt(dd, 10) || parseInt(dd, 10)  < 1) ||
				(d.getMonth() != parseInt(mm, 10) || parseInt(mm, 10) < 1) ||
				d.getFullYear() != parseInt(yy, 10) || 
				( parseInt(mm, 10) == 2 && !checkFebMonth( dd, yy ) )
			) ) )
		{			
			//zera formatacao
			fmtret = "";
			if ( (parseInt(mm, 10) == 2) &&
				 (parseInt(dd, 10) > 28) ) //fevereiro
			{
				if ( checkFebMonth( 29, yy ) )
					dd = 29;
				else
					dd = 28;
			}
			
			if ( (parseInt(mm, 10) < 1) || (parseInt(mm, 10) > 12) )
				mm = (new Date()).getMonth();
			
			if ( (parseInt(dd, 10) < 1) || (parseInt(dd, 10) > 31) )
				dd = (new Date()).getDate();
			
			//refaz a formatacao
			for( var i=0; i < this.arrayDayMonthYear.length; i++ ){
				x = this.arrayDayMonthYear[i].charAt(0).toLowerCase();
				if ( x == "d" && dd != undefined) fmtret += paddingLeft( makeFormat("0", this.arrayDayMonthYear[i].length) , dd );
				else if ( x == "m" && mm != undefined) fmtret += paddingLeft( makeFormat("0", this.arrayDayMonthYear[i].length) , mm );
				else if ( x == "y" && yy != undefined) fmtret += paddingLeft( makeFormat("0", this.arrayDayMonthYear[i].length) , yy );
				
				if ( i <= ( this.arrayDelimiters.length-1 ) ) fmtret += this.arrayDelimiters[i];
			}
		}
	}
	
	return fmtret;
}

function paddingLeft( chars, text ) {
	if ( String(text).length >= String(chars).length ) return text;
	var ret = chars.substr(0, String(chars).length-String(text).length);
	for( var i=0; i<String(text).length; i++ ) {
		ret = ret + String(text).charAt(i);
	}
	return ret;
}

function makeFormat( s, n ) {
	var ret = "";
	for( var i=0; i< n; i++)
		ret = ret + s;
	return ret;
}

function checkFebMonth( day, year ) {
	var isleap=(year%4==0 && (year%100!=0 || year%400==0));
	if (parseInt(day, 10) > 29 || (parseInt(day,10) == 29 && !isleap)) {
		return false;
    }
    return true;
}
