// Rotinas para seleção multipla em ListBox

var sortitems = 0;

function move(fbox,tbox){
	for(var i=0; i<fbox.options.length; i++){
		if(fbox.options[i].selected && fbox.options[i].value != ""){
			var no = new Option();
			no.value = fbox.options[i].value;
			no.text = fbox.options[i].text;
			tbox.options[tbox.options.length] = no;
			fbox.options[i].value = "";
			fbox.options[i].text = "";
    	}
	}
	BumpUp(fbox);
	if (sortitems) SortD(tbox);
}

function BumpUp(box){
	for(var i=0; i<box.options.length; i++){
		if(box.options[i].value == ""){
			for(var j=i; j<box.options.length-1; j++){
				box.options[j].value = box.options[j+1].value;
				box.options[j].text = box.options[j+1].text;
			}
			var ln = i;
			break;
  		}
	}

	if(ln < box.options.length){
		box.options.length -= 1;
		BumpUp(box);
    }

}

function SortD(box){
	var temp_opts = new Array();
	var temp = new Object();

	for(var i=0; i<box.options.length; i++){
		temp_opts[i] = box.options[i];
	}

	for(var x=0; x<temp_opts.length-1; x++){
		for(var y=(x+1); y<temp_opts.length; y++){
			if(temp_opts[x].text > temp_opts[y].text){
				temp = temp_opts[x].text;
				temp_opts[x].text = temp_opts[y].text;
				temp_opts[y].text = temp;
       		}
	    }
	}

	for(var i=0; i<box.options.length; i++){
		box.options[i].value = temp_opts[i].value;
		box.options[i].text = temp_opts[i].text;
    }

}

// --- fim das rotinas


function openShowModalWindow(page,reload)
{
	//window.open("../frm_modal.asp?page="+page); //para teste pois o Modal Dialog não permite ver os fontes
	
	showModalDialog("../frm_modal.asp?page="+page,this.document,"dialogHeight:600px;dialogWidth:700px;scroll:no");
	
	if (reload)
	{
		document.forms[0].submit();
	}
}		   

function chamarPaginaDeSelecao(pagina,campoid,habilitaIncluir,validarCompliancer,param,msgparam)
{
	page = pagina + "?idcampo=" + campoid + "|incluir=" + habilitaIncluir + "|compliancer=" + validarCompliancer; 
	if ((param != null) && (param != ""))
	{
		var valorparam = document.getElementById(param + "_txtUcSelectId").value;
		if (valorparam == 0)
		{
			alert(msgparam);
			return 0;
		}
		page += "|valorcampopai=" + valorparam;
	}
	openShowModalWindow(page,false)
}


function MudaCorCell(idcor,cor) 
{
  document.getElementById(idcor).bgColor = cor;
}

function isValidDate(dateStr, format)
{
  if (format == null) {
    format = "MDY";
  }
  if ((format.indexOf("M") == -1) || (format.indexOf("D") == -1) || (format.indexOf("Y") == -1) ) {
    format = "MDY";
  }
  if (format.substring(0, 1) == "Y"){
    // If the year is first
    var reg1 = /^\d{2}(\-|\/|\.)\d{1,2}\1\d{1,2}$/
    var reg2 = /^\d{4}(\-|\/|\.)\d{1,2}\1\d{1,2}$/
  } else {
    if (format.substring(1, 2) == "Y"){
    // If the year is second
    var reg1 = /^\d{1,2}(\-|\/|\.)\d{2}\1\d{1,2}$/
    var reg2 = /^\d{1,2}(\-|\/|\.)\d{4}\1\d{1,2}$/
    } else {
      // The year must be third
      var reg1 = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{2}$/
      var reg2 = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{4}$/
    }
  }
  // If it doesn't conform to the right format (with either a 2 digit year or 4 digit year), fail
  if ((reg1.test(dateStr) == false) && (reg2.test(dateStr) == false) ){
    return false;
  }
  var parts = dateStr.split(RegExp.$1); // Split into 3 parts based on what the divider was
  // Check to see if the 3 parts end up making a valid date
  if (format.substring(0, 1) == "M"){
    var mm = parts[0];
  } else {
    if(format.substring(1, 2) == "M"){
    var mm = parts[1];
    } else {
      var mm = parts[2];
    }
  }
  if (format.substring(0, 1) == "D"){
    var dd = parts[0];
  } else {
    if (format.substring(1, 2) == "D"){
      var dd = parts[1];
    } else {
      var dd = parts[2];
    }
  }
  if (format.substring(0, 1) == "Y"){
    var yy = parts[0];
  } else {
    if (format.substring(1, 2) == "Y"){
      var yy = parts[1];
    } else {
      var yy = parts[2];
    }
  }
  if (parseFloat(yy) <= 50){
    yy = (parseFloat(yy) + 2000).toString();
  }
  if (parseFloat(yy) <= 99){
    yy = (parseFloat(yy) + 1900).toString();
  }
  var dt = new Date(parseFloat(yy), parseFloat(mm)-1, parseFloat(dd), 0, 0, 0, 0);
  if (parseFloat(dd) != dt.getDate()){
    return false;
  }
  if (parseFloat(mm)-1 != dt.getMonth()){
    return false;
  }
  return true;
}

function ConvertStrToDateTime(dateStr, format)
{
  if (format == null) {
    format = "MDY";
  }
  if ((format.indexOf("M") == -1) || (format.indexOf("D") == -1) || (format.indexOf("Y") == -1) ) {
    format = "MDY";
  }
  if (format.substring(0, 1) == "Y"){
    // If the year is first
    var reg1 = /^\d{2}(\-|\/|\.)\d{1,2}\1\d{1,2}$/
    var reg2 = /^\d{4}(\-|\/|\.)\d{1,2}\1\d{1,2}$/
  } else {
    if (format.substring(1, 2) == "Y"){
    // If the year is second
    var reg1 = /^\d{1,2}(\-|\/|\.)\d{2}\1\d{1,2}$/
    var reg2 = /^\d{1,2}(\-|\/|\.)\d{4}\1\d{1,2}$/
    } else {
      // The year must be third
      var reg1 = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{2}$/
      var reg2 = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{4}$/
    }
  }
  // If it doesn't conform to the right format (with either a 2 digit year or 4 digit year), fail
  if ((reg1.test(dateStr) == false) && (reg2.test(dateStr) == false) ){
    return false;
  }
  var parts = dateStr.split(RegExp.$1); // Split into 3 parts based on what the divider was
  // Check to see if the 3 parts end up making a valid date
  if (format.substring(0, 1) == "M"){
    var mm = parts[0];
  } else {
    if(format.substring(1, 2) == "M"){
    var mm = parts[1];
    } else {
      var mm = parts[2];
    }
  }
  if (format.substring(0, 1) == "D"){
    var dd = parts[0];
  } else {
    if (format.substring(1, 2) == "D"){
      var dd = parts[1];
    } else {
      var dd = parts[2];
    }
  }
  if (format.substring(0, 1) == "Y"){
    var yy = parts[0];
  } else {
    if (format.substring(1, 2) == "Y"){
      var yy = parts[1];
    } else {
      var yy = parts[2];
    }
  }
  if (parseFloat(yy) <= 50){
    yy = (parseFloat(yy) + 2000).toString();
  }
  if (parseFloat(yy) <= 99){
    yy = (parseFloat(yy) + 1900).toString();
  }
  var dt = new Date(parseFloat(yy), parseFloat(mm)-1, parseFloat(dd), 0, 0, 0, 0);
  if (parseFloat(dd) != dt.getDate()){
    return null;
  }
  if (parseFloat(mm)-1 != dt.getMonth()){
    return null;
  }
  return dt;
}

function ComparaDatas(StrData1,StrData2, format)
{
    var data1, data2;
    data1 = ConvertStrToDateTime(StrData1, format);
    data2 = ConvertStrToDateTime(StrData2, format);
	if ( data1.toString() == data2.toString() &&  data1 != null && data2 != null )
	   return true;
	else
       return false;
}


function FindObj(n, d) { 
				var p,i,x;
				if(!d) d=document; 
				if((p=n.indexOf("?"))>0&&parent.frames.length) {
				  d=parent.frames[n.substring(p+1)].document;
				  n=n.substring(0,p);
				}
				if (!(x=d[n])&&d.all) 
				  x=d.all[n]; 
				for (i=0;!x&&i<d.forms.length;i++) 
				  x=d.forms[i][n];
				for(i=0;!x&&d.layers&&i<d.layers.length;i++) 
				  x=MM_findObj(n,d.layers[i].document);
				if(!x && d.getElementById) 
				  x=d.getElementById(n); 
				return x;
		    }	

function ShowHideComponentes() 	{ 
			  var i,p,v,obj,args=ShowHideComponentes.arguments;
 			  for (i=0; i<(args.length-1); i+=2) {
				if ((obj=FindObj(args[i]))!=null) {
				   v=args[i+1];
			  	   if (obj.style) { 
			  		  obj=obj.style;
			  		  v=(v=='show')?'visible':(v=='hide')?'hidden':v; 
				      obj.visibility=v;
				   }
			  	}
			  }
			}
			
function InLineNoneComponentes() 	{ 
			  var i,p,v,obj,args=InLineNoneComponentes.arguments;
 			  for (i=0; i<(args.length-1); i+=2) {
				if ((obj=FindObj(args[i]))!=null) {
				   v=args[i+1];
			  	     if (obj.style) { 
			  	  	   obj=obj.style;
			  		   v=(v=='show')?'inline':(v=='hide')?'none':v; 
				       obj.display=v;
				     }
				}
			  }
			}

function EnableForm(acao, d) {
			  form = document.forms[0];
			  if(!d) d='ed_codigo'; 
			  switch (acao) {
			    case 'I': //Incluir
			      s=true;
			      l=true;
			      r=false;
			      break;
			    case 'A': //Alterar
			      s=true;
			      l=false;
			      r=false;
			      break;
			    case 'C':  //Cancelar
			      s=false;
			      l=false;
			      r=true;  
			      break;
			    default: 
			      s=false;
			      l=false;
			      r=false;
			  }
			  if (r) {  form.reset(); } 
		
			  var obj,p,i,elems=form.elements;
			  if (s) 
			  { 
			    a = 'hide'; b='show'; 
			  }
			  else
			  {
			    a='show'; b='hide'; 
			  }
			  ShowHideComponentes('navigator_bt_incluir',a);	
			  
			  if (((obj=FindObj(d))!=null) && (obj.value!='')) {
			    ShowHideComponentes('navigator_bt_alterar',a);	
			    ShowHideComponentes('navigator_bt_excluir',a);
			    if (l) { obj.value = ""; }			       	
			  }		  
			  else
			  {
			    ShowHideComponentes('navigator_bt_alterar','hide');	
			    ShowHideComponentes('navigator_bt_excluir','hide');
			  }
			  ShowHideComponentes('navigator_bt_cancelar',b);
			  ShowHideComponentes('navigator_bt_salvar',b);

 		      for (i=0; i<(elems.length); i++) {
			    var msg_erro = "";
			    var msg_erro2 = "";
			    var img_data = "";
				if ((p=elems[i].id.lastIndexOf('campoResult')) >= 0) {
			       if (l && (elems[i].id.substr(0,1) != 'N')) {
			   	      elems[i].value = ""; 
			   	   }
			    }
			   	if ((p=elems[i].id.lastIndexOf('txtUcDate')) >= 0) {
			       elems[i].readOnly=!s;
			       msg_erro = elems[i].id.substr(0,p-1) + '_valUcDate';
			       msg_erro2 = elems[i].id.substr(0,p-1) + '_valFormatoData';
			       if (l && (elems[i].id.substr(0,1) != 'N')) { elems[i].value = ""; }
			       img_data = elems[i].id.substr(0,p-1) + ':btAbreCal';
			       ShowHideComponentes(img_data,b);    
			       if (!s) { 
				     InLineNoneComponentes(msg_erro,'hide');    
			         InLineNoneComponentes(msg_erro2,'hide');    
			       }
			   	}
			   	if (((p=elems[i].id.lastIndexOf('txtUcSelect')) >= 0) && (elems[i].id.lastIndexOf('txtUcSelectId') < 0))
			   	{
			       elems[i].readOnly=!s;
			       msg_erro = elems[i].id.substr(0,p-1) + '_valUcSelect';
			       msg_erro2 = elems[i].id.substr(0,p-1) + '_txtUcSelectId';
			       if (FindObj(msg_erro)!=null) { document.getElementById(msg_erro).enabled=s; } 
			   	   if (l && (elems[i].id.substr(0,1) != 'N')) {
			   	      elems[i].value = ""; 
			   	      document.getElementById(msg_erro2).value = "";
			   	   }
			       img_data = elems[i].id.substr(0,p-1) + '_bt_selecionar';
			       ShowHideComponentes(img_data,b);    
			       if (!s) { 
				     InLineNoneComponentes(msg_erro,'hide');    
			       }
			   	}			   	
			   	if ((p=elems[i].id.lastIndexOf('txtUcCheckBox')) >= 0) {
			       elems[i].disabled=!s;			   	   
			   	   if (l && (elems[i].id.substr(0,1) != 'N')) { elems[i].checked = true; }
			    }
			    if ((p=elems[i].id.lastIndexOf('mostra')) >= 0) {
			       if (l) { elems[i].value = ""; }			   	   
			    }
			   	if ((p=elems[i].id.lastIndexOf('txtUcRadioButton')) >= 0) {
			       elems[i].disabled=!s;
			   	   msg_erro = elems[i].id.substr(0,p-1) + '_valUcRadioButton';
			   	   if (l && (elems[i].id.substr(0,1) != 'N')) { if (elems[i].name != elems[i-1].name) { elems[i].checked=true; } }
			       if (!s) { 
			         InLineNoneComponentes(msg_erro,'hide'); 
			       }
			    }
			   	if ((p=elems[i].id.lastIndexOf('txtUcComboBox')) >= 0) {
			       elems[i].disabled=!s;
			   	   msg_erro = elems[i].id.substr(0,p-1) + '_valUcComboBox';
			       if (FindObj(msg_erro)!=null) { document.getElementById(msg_erro).enabled=s; }
			   	   if (l && (elems[i].id.substr(0,1) != 'N')) { elems[i].selectedIndex = 0; }
			       if (!s) { 
			         InLineNoneComponentes(msg_erro,'hide'); 
			       }
			    }
			   	if ((p=elems[i].id.lastIndexOf('txtUcText')) >= 0) {
			       elems[i].readOnly=!s;
			       msg_erro = elems[i].id.substr(0,p-1) + '_valUcText';
			   	   if (FindObj(msg_erro)!=null) { document.getElementById(msg_erro).enabled=s; }
			   	   if (l && (elems[i].id.substr(0,1) != 'N')) { elems[i].value = ""; }
			       if (!s) { 
			          InLineNoneComponentes(msg_erro,'hide');         
			   	   }
			   	}
			   	if ((p=elems[i].id.lastIndexOf('txtUcString')) >= 0) {
			       elems[i].readOnly=!s;
			       msg_erro = elems[i].id.substr(0,p-1) + '_valUcString';
			       if (FindObj(msg_erro)!=null) { document.getElementById(msg_erro).enabled=s; }
			   	   if (l && (elems[i].id.substr(0,1) != 'N')) { elems[i].value = ""; }
			       if (!s) { 			          
			   	      InLineNoneComponentes(msg_erro,'hide');         
			       }
			   	}
			   	if ((p=elems[i].id.lastIndexOf('txtUcDecimal')) >= 0) {
			   	   elems[i].readOnly=!s;
			       msg_erro = elems[i].id.substr(0,p-1) + '_valUcDecimal';
			   	   if (FindObj(msg_erro)!=null) {  document.getElementById(msg_erro).enabled=s; }
			   	   if (l && (elems[i].id.substr(0,1) != 'N')) { elems[i].value = ""; }
			       if (!s) { 
			          InLineNoneComponentes(msg_erro,'hide');         
			   	   }
			   	}
			   	if ((p=elems[i].id.lastIndexOf('txtUcMoney')) >= 0) {
			       elems[i].readOnly=!s;
			       msg_erro = elems[i].id.substr(0,p-1) + '_valUcMoney';
			   	   if (FindObj(msg_erro)!=null) { document.getElementById(msg_erro).enabled=s; }
			   	   if (l && (elems[i].id.substr(0,1) != 'N')) { elems[i].value = ""; }
			       if (!s) { 
			          InLineNoneComponentes(msg_erro,'hide');         
			   	   }
			   	}				    

		        // ONLYALT é colocado na frente do campo quando ele é visivel somente na alteração
		        if ((elems[i].id.lastIndexOf('ONLYALT') >= 0) && (acao != 'A'))
			    {
				     if (FindObj(msg_erro)!=null) { document.getElementById(msg_erro).enabled=false; } 
			         if (FindObj(msg_erro2)!=null) { document.getElementById(msg_erro2).enabled=false; }
			         InLineNoneComponentes(msg_erro,'hide');    
			         InLineNoneComponentes(msg_erro2,'hide');    
			         ShowHideComponentes(img_data,'hide');    
			         InLineNoneComponentes(elems[i].id,'hide');    				   
			    } 
			    else if ((elems[i].id.lastIndexOf('ONLYALT') >= 0) && (acao == 'A'))
			    {
			         InLineNoneComponentes(elems[i].id,'show');    				   
				}

		        // ONLYINC é colocado na frente do campo quando ele é visivel somente na inclusão
		        if ((elems[i].id.lastIndexOf('ONLYINC') >= 0) && (acao != 'I'))
			    {
			         if (FindObj(msg_erro)!=null) { document.getElementById(msg_erro).enabled=false; } 
			         if (FindObj(msg_erro2)!=null) { document.getElementById(msg_erro2).enabled=false; }
			         InLineNoneComponentes(msg_erro,'hide');    
			         InLineNoneComponentes(msg_erro2,'hide');    
			         ShowHideComponentes(img_data,'hide');    
			         InLineNoneComponentes(elems[i].id,'hide');    				   
			    } 
			    else if ((elems[i].id.lastIndexOf('ONLYINC') >= 0) && (acao == 'I'))
			    {
			         InLineNoneComponentes(elems[i].id,'show');    				   
				}
				
				// NOTINC é colocado na frente do campo quando ele NÃO é visivel somente na inclusão
		        if ((elems[i].id.lastIndexOf('NOTINC') >= 0) && (acao == 'I'))
			    {
			         if (FindObj(msg_erro)!=null) { document.getElementById(msg_erro).enabled=false; } 
			         if (FindObj(msg_erro2)!=null) { document.getElementById(msg_erro2).enabled=false; }
			         InLineNoneComponentes(msg_erro,'hide');    
			         InLineNoneComponentes(msg_erro2,'hide');    
			         ShowHideComponentes(img_data,'hide');    
			         InLineNoneComponentes(elems[i].id,'hide');    				   
			    } 
			    else if ((elems[i].id.lastIndexOf('NOTINC') >= 0) && (acao != 'I'))
				{
			         InLineNoneComponentes(elems[i].id,'show');    				   
				}
				
		        // N é colocado na frente do campo quando ele não é editável na alteração/inclusão
		        if ((elems[i].id.substr(0,1) == 'N') && (s))
			    {
				     if (FindObj(msg_erro)!=null) { document.getElementById(msg_erro).enabled=false; } 
			         if (FindObj(msg_erro2)!=null) { document.getElementById(msg_erro2).enabled=false; }
			         InLineNoneComponentes(msg_erro,'hide');    
			         InLineNoneComponentes(msg_erro2,'hide');    
			         ShowHideComponentes(img_data,'hide');    
			         elems[i].readOnly=true;		
					 if (elems[i].id.lastIndexOf('txtUcComboBox') >= 0) 
			         {
			           elems[i].disabled = true;		   
			         }			         
			    }
			    
   		        // filtro é colocado na frente do campo quando ele tem que ser desabilitado na edição/inclusão de um registro
		        if ((elems[i].id.substr(0,6) == 'filtro') && (s))
			    {
 		             elems[i].disabled = true;
 		             elems[i].readOnly = true; 		             		   
			    }	
			    else if ((elems[i].id.substr(0,6) == 'filtro') && (!s))	
			    {
 		             elems[i].disabled = false;
 		             elems[i].readOnly = false; 		             		   			    
			    }
			    
			    if ((elems[i].id == 'bt_filtrar') && (s))	
			    {
 		             elems[i].disabled = true;
			    }	
			    else if ((elems[i].id == 'bt_filtrar') && (!s))	
			    {
 		             elems[i].disabled = false;
			    }	
			    
			     if ((elems[i].id == 'rb_select') && (s))	
			    {
 		             elems[i].disabled = true;
			    }	
			    else if ((elems[i].id == 'rb_select') && (!s))	
			    {
 		             elems[i].disabled = false;
			    }
			 }
		   }	
		   
		   
function DeleteItem() {
    if (confirm("Apagar Registro?"))
    {
	  form = document.forms[0];	  
	  var newInputHidden = document.createElement("<INPUT TYPE='HIDDEN' NAME='excluir' VALUE='1'>")
      form.insertBefore(newInputHidden);
      form.submit();
    }	  
}	
