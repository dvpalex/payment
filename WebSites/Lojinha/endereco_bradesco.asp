<%
Response.Buffer = TRUE

' Grava forma de pagamento para ser usada mais adiante
Session("forma_pagamento") = "BRADESCONET"
%>

<HTML> 

<HEAD> 
	<TITLE> 
		Carteira de endereços
	</TITLE> 
	<META HTTP-EQUIV="Expires" CONTENT="Tue, 01 Jan 1990 12:00:00 GMT">
</HEAD> 


<SCRIPT LANGUAGE="Javascript" SRC="bsniffer.js"></SCRIPT>
<SCRIPT LANGUAGE="Javascript">
var fMSWltLoaded = false;
var objAddrSelector;

function MSWltLoadDone()
{
	if (is.nav) {
		if (document.addrSelector == null)
		{
			if (confirm("Click OK para instalar os Pluggins da Microsoft Wallet."))
				window.open("http://www.microsoft.com/merchant/wallet/local/plginst.htm");
			else
				//history.back(); // volta para a página anterior ou para alguma página definida pela loja
			return;
		}
		objAddrSelector = document.addrSelector;
		fVersionOK = objAddrSelector.VersionCheck();
		if (!fVersionOK)
		{
			if (confirm("O pluggin utilizado está desatualizado, você precisa fazer uma atualização."))
				window.open("http://www.microsoft.com/merchant/wallet/local/plginst.htm");
			return;
		}
		fMSWltLoaded = true;
	}
}

function MSWltCheckLoaded()
{
	if (!fMSWltLoaded)
	{
		if (is.ie) {
			if (addrSelector) {
				objAddrSelector = addrSelector;
				fMSWltLoaded = true;
			} else alert("A página ainda não terminou de carregar. Tente novamente quando a página carregar. Recarregue a página caso tenha dificuldade (e então espere até a página terminar de carregar).");
		} else alert("A página ainda não terminou de carregar. Tente novamente quando a página carregar. Recarregue a página caso tenha dificuldade (e então espere até a página terminar de carregar).");
	}
	return fMSWltLoaded;
}

function doNothing() {  }


function MSWltPrepareForm(form, cParams, xlationArray)
{	
	if (!MSWltCheckLoaded())
		return false;

	shipTo = objAddrSelector.GetValues();

	errorStatus = objAddrSelector.GetLastError();
	if (errorStatus < 0)
	{
		if (errorStatus != (-2147220991) && errorStatus != (-2147220990));  
			alert("Address selection failed due to an unknown problem.");
		return false;
	}
	elements = form.elements;

	xlate = new doNothing();

	for (i = 2; i < cParams; i += 2)
	{
		value = MSWltPrepareForm.arguments[i+1];
		if (value.length > 0)
			xlate[MSWltPrepareForm.arguments[i]] = value;
	}

	for (i = 0; i < elements.length; i++)
	{
		if (form.elements[i].name.length > 0)
			{
				xlateValue = xlate[elements[i].name];
				if (xlateValue)
					name = xlateValue;
				else
					name = elements[i].name;
					value = 'a' + objAddrSelector.GetValue(shipTo, name, 0);
					if (value.length > 1)
						elements[i].value = value.substring(1);
			}
	}
	return true;
}

function submitShipToAddr()
{
	if (MSWltPrepareForm(document.shipinfo, 10, "ship_to_address1", "ship_to_street", "ship_to_address2", "ship_to_city", "ship_to_address3", "ship_to_state", "ship_to_address4", "ship_to_zip")) {
		document.shipinfo.submit();
	}
}
</SCRIPT>

<BODY BGCOLOR="#333399" onLoad="MSWltLoadDone()">


<TABLE width="70%" ALIGN="center">
	<TR align=center>
		
    
    <TD align=center> 
      <h1><font color="#FF0000">Endere&ccedil;o de Entrega</font></h1>
    </TD>
	</TR>
	<TR ALIGN=center>
		<TD align=center>
			<!-- WALLET   FORMULÁRIO COM DADOS DE ENDERECO -->
			<FORM NAME="shipinfo" METHOD=POST ACTION="grava_endereco.asp">
                <font color="#FFFFFF">
				<INPUT TYPE="HIDDEN" NAME="use_form" VALUE="0">
				<INPUT TYPE="HIDDEN" NAME=ship_to_name>
				<INPUT TYPE="HIDDEN" NAME=ship_to_street>
				<INPUT TYPE="HIDDEN" NAME=ship_to_city>
				<INPUT TYPE="HIDDEN" NAME=ship_to_state>
				<INPUT TYPE="HIDDEN" NAME=ship_to_zip>
				<INPUT TYPE="HIDDEN" NAME=ship_to_country>
				<INPUT TYPE="HIDDEN" NAME=ship_to_phone>
				<INPUT TYPE="HIDDEN" NAME=ship_to_email>
        Instru&ccedil;&otilde;es:<br>
        <textarea name="instrucoes" cols="20" rows="3"></textarea>
                </font>
      </FORM>
			<!-- /WALLET   FORMULÁRIO COM DADOS DE ENDERECO -->
		</TD>
	</TR>
	<TR><TD><font color="#FFFFFF">&nbsp;</font></TD></TR>
	<TR align=center>
		
    <TD align=center> <font color="#FFFFFF"> <IMG SRC="/images/topplugin2_cart01.gif" 
			ALIGN="BOTTOM" NATURALSIZEFLAG="3" BORDER="0" ALT="Selecione um Endereço" width="153" height="29"> 
      <BR>   
			
      
      
      
      <SCRIPT LANGUAGE="JavaScript">
				if (is.ie) {
					document.write('<OBJECT ID=\"addrSelector\"');
					document.write(' CLASSID=\"clsid:87D3CB63-BA2E-11cf-B9D6-00A0C9083362\"');
					document.write(' HEIGHT=\"123\"');
					document.write(' WIDTH=\"154\">');
					document.write('</OBJECT>');
				} else {
					document.write('<EMBED NAME=\"addrSelector\" SRC=\"empty.adr\"');
					document.write(' PLUGINSPAGE=\"http://www.microsoft.com/merchant/wallet/local/plginst.htm\"');
					document.write(' VERSION=\"2,0,0,1307\"');
					document.write(' HEIGHT=\"123\"');
					document.write(' WIDTH=\"154\">');
				}
			</SCRIPT>                
      </font>                
		</TD>
	</TR>
	<TR align=center>
		<TD align=center>
			<FORM> 
                <font color="#FFFFFF"> 
				<INPUT	TYPE="BUTTON"
						VALUE="Confirma Endereço"
						onClick="submitShipToAddr()"
						ALT="Confirma Endereço">
                </font>
			</FORM> 
		</TD>
	</TR>
</TABLE>


</BODY> 
</HTML> 
