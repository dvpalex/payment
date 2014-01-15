<% Response.Buffer = TRUE %>
<% Response.Expires = 0 %>
<% 	strMSWltAcceptedTypes = "" & "Bradesco:clear;" & "BradescoDebito:clear;" & "BradescoVisa:clear;" & "BradescoPoupCard:clear;" %>
<%
' Monta variavel no formato R$99,99 que o gateway de pagamento entende
valor = "R$"
valor = valor & Trim(CStr(Fix(Session("total")))) & ","
valor = valor & Right("00" & CInt(100 * (Session("total") - Fix(Session("total")))), 2)

' Checagem de seguranca. Verifica algum visitante mal intencionado
' alterou a URL para tentar mudar valor a ser pago
If Request("valor") <> valor Then
	Response.Write "Erro: valores nao correspondem"
	Response.End
End If
%>
<!--#INCLUDE FILE="i_generate_info_for_wallet.asp" -->

<HTML> 
<HEAD> 
	<SCRIPT LANGUAGE="Javascript" SRC="bsniffer.js"></SCRIPT>
	<SCRIPT LANGUAGE="Javascript">
var fMSWltLoaded = false	
var objPaySelector

function MSWltLoadDone()
{
	if (is.nav) {
		if (document.paySelector == null)
		{
			if (confirm("Click OK para instalar os Pluggins da Microsoft Wallet."))
				window.open("http://www.microsoft.com/merchant/wallet/local/plginst.htm");
			else
				history.back(); // volta para a página anterior ou para alguma página definida pela loja
			return;
		}
		objPaySelector = document.paySelector;
		fVersionOK = objPaySelector.VersionCheck();
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
			if (paySelector) {
				objPaySelector = paySelector;
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

		PI = objPaySelector.GetValues()	          
		errorStatus = objPaySelector.GetLastError()
		if (errorStatus < 0)
		{
			if (errorStatus != (-2147220991) && errorStatus != (-2147220990))  
				alert("Payment selection failed due to an unknown problem.")
			return false
		}

	elements = form.elements;
	
	xlate = new doNothing();

	for (i = 2; i < cParams; i += 2)
	{
		value = MSWltPrepareForm.arguments[i+1]
		if (value.length > 0)
			xlate[MSWltPrepareForm.arguments[i]] = value
	}

	
		for (i = 0; i < elements.length; i++)
		{ 
			if (form.elements[i].name.length > 0)
			{
				xlateValue = xlate[elements[i].name]
				if (xlateValue)
					name = xlateValue
				else
					name = elements[i].name

				
					value = 'a' + objPaySelector.GetValue(PI, name, 0)
					if (value.length > 1)
						elements[i].value = value.substring(1)
				
			}
		}
	

	return true
}


function submitPayinfo()
{
	if (MSWltPrepareForm(document.payinfo, 10, "bill_to_address1", "bill_to_street", "bill_to_address2", "bill_to_city", "bill_to_address3", "bill_to_state", "bill_to_address4", "bill_to_zip")) {
		document.payinfo.submit();
	}
}
	</SCRIPT>
	<TITLE> 
		Wallet Pagamento
	</TITLE> 
	<META HTTP-EQUIV="Expires" CONTENT="Tue, 01 Jan 1990 12:00:00 GMT">
</HEAD>
<BODY BGCOLOR="#333399"	onLoad="MSWltLoadDone()" text="#CC0000"> 
<P>
<CENTER>
</CENTER>

<P>		
<CENTER>
<TABLE>
	<TR>
		
      <TD ALIGN="CENTER"> <font face="Comic Sans MS"><font size="4" color="#FFFFFF"> Subtotal: <b><%= FormatCurrency(Session("subtotal")) %><br>
        </b>Taxa de Envio: <b><%= FormatCurrency(Session("taxa_envio")) %><br>
        </b>TOTAL: </font> <b><%= FormatCurrency(Session("total")) %></b> 
        </font> 
        <FORM NAME=payinfo METHOD=POST ACTION="processa_autorizacao.asp">
				<INPUT TYPE="HIDDEN" NAME="use_form" VALUE="0">
				<INPUT TYPE="HIDDEN" NAME="bill_to_name" VALUE="s ">
			
				<INPUT TYPE="HIDDEN" NAME="bill_to_street" VALUE="s ">
				<INPUT TYPE="HIDDEN" NAME="bill_to_city" VALUE="s ">
				<INPUT TYPE="HIDDEN" NAME="bill_to_state" VALUE="s ">
				<INPUT TYPE="HIDDEN" NAME="bill_to_zip" VALUE="s ">

				<INPUT TYPE="HIDDEN" NAME="bill_to_country" VALUE="BRA">
				<INPUT TYPE="HIDDEN" NAME="bill_to_phone" VALUE="1 ">
				<INPUT TYPE="HIDDEN" NAME="bill_to_email" VALUE="e ">
				<INPUT TYPE="HIDDEN" NAME="OI" SIZE=300>
				<input TYPE="HIDDEN" NAME="OI2" SIZE="300">
				<input TYPE="HIDDEN" NAME="OI3" SIZE="300">
				<INPUT TYPE="HIDDEN" NAME="PI" SIZE=300>
				<INPUT TYPE="HIDDEN" NAME="HashOIPI" SIZE=300>
				<INPUT TYPE="HIDDEN" NAME="HashPI" SIZE=300>
				<INPUT TYPE="HIDDEN" NAME="ClientCert1" SIZE=300>
				<INPUT TYPE="HIDDEN" NAME="ClientCert2" SIZE=300>
				<input TYPE="HIDDEN" NAME="ClientCert3" SIZE="300">
			</FORM>
			
        <font face="Comic Sans MS">
			
        <IMG SRC="/Images/titul_pagto.gif" 
				ALIGN="BOTTOM" NATURALSIZEFLAG="3" BORDER="0" ALT="Selecione um Cartão para Pagamento" width="154" height="22"> 
        </font> 
      </TD>
	</TR>
	<TR>
		
      <TD VALIGN=TOP align="CENTER"> 
        <font face="Comic Sans MS"> 
        <SCRIPT LANGUAGE="Javascript">
				orderHash = "<%= postParaCliente %>";
				if (is.ie) {
					payObj = '<OBJECT	ID=\"paySelector\"';
					payObj = payObj + ' CLASSID=\"clsid:87D3CB66-BA2E-11cf-B9D6-00A0C9083362\"';
					payObj = payObj + ' HEIGHT=\"123\"';
					payObj = payObj + ' WIDTH=\"154\">';
					payObj = payObj + '<PARAM NAME="AcceptedTypes" VALUE="<%= strMSWltAcceptedTypes %>">';
					payObj = payObj + '<PARAM NAME="Total" VALUE="<%= valor %>">';
					payObj = payObj + '<PARAM NAME="OrderHash" VALUE="'+orderHash+'">';
					payObj = payObj + '</OBJECT>';
					document.write(payObj);
				} else {
					document.write('<EMBED	NAME=\"paySelector\" SRC=\"empty.wlt\"');
					document.write(' PLUGINSPAGE=\"http://www.microsoft.com/merchant/wallet/local/plginst.htm\"');
					document.write(' VERSION=\"2,0,0,1378\"');
					document.write(' HEIGHT=\"123\"');
					document.write(' WIDTH=\"154\"');
					document.write(' ACCEPTEDTYPES="<%= strMSWltAcceptedTypes %>"');
					document.write(' TOTAL="<%= valor %>"');
					document.write(' OrderHash="<%= postParaCliente %>">');
				}
			</SCRIPT>
        </font>
		</TD>
	</TR>
	<TR>
		<TD>
				<FORM>
                    <font face="Comic Sans MS">
					<INPUT TYPE="BUTTON"
						VALUE="Confirma Pagamento"
						onClick="submitPayinfo()"
						SRC="/MSCS_Images/buttons/btnpurchase2.gif"
						WIDTH=116
						HEIGHT=25
						BORDER=0
						ALT="Confirma Pagamento">
                    </font>
				</FORM>
		</TD>
	</TR>
</TABLE>
</CENTER>

</BODY>
</HTML>
