<!--#INCLUDE FILE="funcoes_uteis.asp" -->
<%
'primeira vez que entra na página para BOLETO e CARTAO tem que recuperar variáveis de 
'sessão. Ver documentação para maiores informações.

If (Session("forma_pagamento") = "BOLETO") OR (Request.QueryString("erro_cartao") = "") Then
	Session("forma_pagamento") = Request.QueryString("forma_pagamento")
	Session("codigo_pedido") = Request("codigo_pedido")
	Session("subtotal") = Request("subtotal")
End If

' Checa de Session("codigo_pedido") nao expirou
Call Checa_Sessao_Pedido()

' força meio seguro para caso de cartão.
If Session("forma_pagamento") = "CARTAO" Then
	URL_Confirmacao = Application("URL_Confirmacao_Segura")
Else
	URL_Confirmacao = Application("URL_Confirmacao")
End If

' A página veio redirecionada de Confirmação, por que houve problema no 
' cartão de crédito.
' ?erro_cartao = 1 -> problema no número
' ?erro_cartao = 2 -> problema na data de validade
If Request.QueryString("erro_cartao") <> "" Then
	erro_cartao = Request.QueryString("erro_cartao") 
End If

%>
<HTML>
<HEAD>
<meta http-equiv="Content-Type" content="text/html; charset=" >
<TITLE>Endereço de entrega</TITLE>
</HEAD>
<BODY BGCOLOR="#333399">
<h1><font color="#FF0000">Endereço de Entrega</font></h1>
<p><font color="#FFFFFF">Preencha todos os campos abaixo com as informações para entrega do pedido.</font></p>
<form method="POST" onsubmit="return valida_endereco()" action="<%= URL_Confirmacao %>" name="Endereco">
  <table border="1">
    <tr> 
      <td><font color="#FFFFFF">Nome</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <input type="HIDDEN" name="pagamento" value="<%= Session("forma_pagamento") %>" maxlength="100" size="30" >
        <input type="TEXT" name="ship_to_name" maxlength="100" size="30" value="<%= Session("nome") %>" >
        </font>
      </td>
    </tr>
    <tr>
      <td><font color="#FFFFFF">CGC/CPF</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <input type="TEXT" name="cgccpf" maxlength="100" size="30" value="<%= Session("cgccpf") %>">
        </font>
      </td>
    </tr>
    <tr> 
      <td><font color="#FFFFFF">Endereço</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <input type="TEXT" name="ship_to_street" maxlength="100" size="30" value="<%= Session("endereco") %>">
        </font>
      </td>
    </tr>
    <tr> 
      <td><font color="#FFFFFF">Cidade</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <input type="TEXT" maxlength="100" size="30" name="ship_to_city" value="<%= Session("cidade") %>">
        </font>
      </td>
    </tr>
    <tr> 
      <td><font color="#FFFFFF">Estado</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <select name="ship_to_state" value="<%= Session("estado") %>">
          <option value="AC">Acre</option>
          <option value="AL">Alagoas</option>
          <option value="AP">Amapá</option>
          <option value="AM">Amazonas</option>
          <option value="BA">Bahia</option>
          <option value="CE">Ceará</option>
          <option value="DF">Distrito Federal</option>
          <option value="ES">Espírito Santo</option>
          <option value="GO">Goiás</option>
          <option value="MA">Maranhão</option>
          <option value="MT">Mato Grosso</option>
          <option value="MS">Mato Grosso do Sul</option>
          <option value="MG">Minas Gerais</option>
          <option value="PA">Pará</option>
          <option value="PB">Paraíba</option>
          <option value="PR">Paraná</option>
          <option value="PE">Pernambuco</option>
          <option value="PI">Piauí</option>
          <option value="RJ">Rio de Janeiro</option>
          <option value="RN">Rio Grande do Norte</option>
          <option value="RS">Rio Grande do Sul</option>
          <option value="RO">Rondonia</option>
          <option value="RR">Roraima</option>
          <option value="SC">Santa Catarina</option>
          <option value="SP">São Paulo</option>
          <option value="SE">Sergipe</option>
          <option value="TO">Tocantins</option>
        </select>
        </font>
      </td>
    </tr>
    <tr> 
      <td><font color="#FFFFFF">CEP</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <input type="TEXT" name="ship_to_zip" size="9" maxlength="9" value="<%= Session("cep") %>">
        </font>
      </td>
    </tr>
    <tr> 
      <td><font color="#FFFFFF">País</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <input type="TEXT" maxlength="100" size="30" name="ship_to_country" value="<%= Session("pais") %>">
        </font>
      </td>
    </tr>
    <tr> 
      <td><font color="#FFFFFF">Telefone</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <input type="TEXT" maxlength="100" size="30" name="ship_to_phone" value="<%= Session("fone") %>">
        </font>
      </td>
    </tr>
    <tr> 
      <td><font color="#FFFFFF">E-mail</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <input type="TEXT" maxlength="100" size="30" name="ship_to_email" value="<%= Session("email") %>">
        </font>
      </td>
    </tr>
    <tr> 
      <td><font color="#FFFFFF">Instruções para entrega</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <textarea name="instrucoes" cols="30" rows="3" value="<%= Session("instrucoes") %>"></textarea>
        </font>
      </td>
    </tr>
  </table>
  <br>

<%
if Session("forma_pagamento") = "CARTAO" then
%>
<h1><font color="#FF0000">Dados do cartão de crédito</font></h1>
<p><font color="#FFFFFF">Preencha todos os campos abaixo com as informações de seu cartão de crédito.</font></p>
<% If erro_cartao = 1 Then %>
<p><b><font color="#FFFFFF">Número de cartão inválido. Prencha novamente os dados do cartão !</font></b></p>
<% ElseIf erro_cartao = 2 Then %>
<p><b><font color="#FFFFFF">Data de validade inválida ! Prencha novamente os dados do cartão !</font></b></p>
<% End If %>

  <table border="1">
    <tr> 
      <td><font color="#FFFFFF">Tipo do cartão</font></td>
      <td> 
    <font color="#FFFFFF"> 
	<SELECT NAME="tipo_cartao" >
<%  Select Case Session("tipo_cartao")  
	Case "visa" %>
	   <OPTION SELECTED VALUE="visa">Visa</OPTION>
	   <OPTION VALUE="mastercard">Master Card</OPTION>
	   <OPTION VALUE="diners">Diners Club</OPTION>
	   <OPTION VALUE="american">American Express</OPTION>
<%  Case "mastercard" %>
	   <OPTION VALUE="visa">Visa</OPTION>
	   <OPTION SELECTED VALUE="mastercard">Master Card</OPTION>
	   <OPTION VALUE="diners">Diners Club</OPTION>
	   <OPTION VALUE="american">American Express</OPTION>
<%  Case "diners" %>
	   <OPTION VALUE="visa">Visa</OPTION>
	   <OPTION VALUE="mastercard">Master Card</OPTION>
	   <OPTION SELECTED VALUE="diners">Diners Club</OPTION>
	   <OPTION VALUE="american">American Express</OPTION>
<%  Case "american" %>
	   <OPTION VALUE="visa">Visa</OPTION>
	   <OPTION VALUE="mastercard">Master Card</OPTION>
	   <OPTION VALUE="diners">Diners Club</OPTION>
	   <OPTION SELECTED VALUE="american">American Express</OPTION>
<%  Case Else %>
	   <OPTION SELECTED VALUE="visa">Visa</OPTION>
	   <OPTION VALUE="mastercard">Master Card</OPTION>
	   <OPTION VALUE="diners">Diners Club</OPTION>
	   <OPTION VALUE="american">American Express</OPTION>
<%  End Select %>
	</SELECT>
    </font>
      </td>
    </tr>
    <tr> 
      <td><font color="#FFFFFF">Nome do titular do cartão</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <input type="TEXT" name="nome_cartao" maxlength="100" size="30" value="<%= Session("nome_cartao") %>">
        </font>
      </td>
    </tr>
    <tr> 
      <td><font color="#FFFFFF">Número do cartão</font></td>
      <td> 
        <font color="#FFFFFF"> 
        <input type="TEXT" name="num_cartao" maxlength="100" size="30" >
        </font>
      </td>
    </tr>
    <tr>
      <td><font color="#FFFFFF">Data de validade</font></td>
<%
	Dim mes(12) 

	mes(1) = "Janeiro"
	mes(2) = "Fevereiro"
	mes(3) = "Março"
	mes(4) = "Abril"
	mes(5) = "Maio"
	mes(6) = "Junho"
	mes(7) = "Julho" 
	mes(8) = "Agosto"
	mes(9) = "Setembro"
	mes(10) = "Outubro"
	mes(11) = "Novembro"
	mes(12) = "Dezembro"
%>
	<td><font color="#FFFFFF"><SELECT NAME="mes_validade" >
<% 

	For i=1 to 12
		If Cint(Session("mes_validade")) = i Then    %>
	        <OPTION SELECTED VALUE="<%= i %>"><%= mes(i) %></OPTION>		
		<% Else %>
	        <OPTION VALUE="<%= i %>"><%= mes(i) %></OPTION>		
		<% End If
	Next 
%>
	</SELECT>
<%
	Dim ano(4)

	ano(1) = 2000
	ano(2) = 2001
	ano(3) = 2002
	ano(4) = 2003
%>
	<SELECT NAME="ano_validade" >
<%
	For i=1 to 4
		If Cint(Session("ano_validade")) = (i+1999) Then    %>
	        <OPTION SELECTED VALUE="<%= i+1999 %>"><%= ano(i) %></OPTION>		
		<% Else %>
	        <OPTION VALUE="<%= i+1999 %>"><%= ano(i) %></OPTION>		
		<% End If
	Next 
%>
	</SELECT>
      </font>
      </td>
    </tr>
  </table>

  <font color="#FFFFFF">

<% End If ' Session("forma_pagamento") = "CARTAO" %>

<SCRIPT LANGUAGE="JavaScript">
<!--//
function valida_endereco() {

     var Form, URL, resposta, s;
     Form = document.Endereco;
     URL = document.URL.toString();

     if (Form.ship_to_name.value.length == 0) {
	alert("O nome é um campo obrigatório !");
        Form.ship_to_name.focus();
        return false;
     }
     if (Form.cgccpf.value.length == 0) {
	alert("O CGC/CPF é um campo obrigatório !");
        Form.cgccpf.focus();
        return false;
     }
     s = limpa_string(Form.cgccpf.value);
     // checa se é cpf
     if (s.length == 11) {
	if (valida_CPF(Form.cgccpf.value) == false ) {
           alert("O CPF não é válido !");
           Form.cgccpf.focus();
           return false;
	}
     }
     // checa se é cgc
     else if (s.length == 14) {
        if (valida_CGC(Form.cgccpf.value) == false ) {
	   alert("O CGC não é válido !");
           Form.cgccpf.focus();
           return false;
	}
     }
     else {
        alert("O CPF/CGC não é válido !");
        return false;
     }
     
     if (Form.ship_to_street.value.length == 0) {
	alert("O endereço é um campo obrigatório !");
        Form.ship_to_street.focus();
        return false;
     }
     if (Form.ship_to_city.value.length == 0) {
	alert("A cidade é um campo obrigatório !");
        Form.ship_to_city.focus();
        return false;
     }
     if (Form.ship_to_zip.value.length == 0) {
	alert("O CEP é um campo obrigatório !");
        Form.ship_to_zip.focus();
        return false;
     }
     if (Form.ship_to_phone.value.length == 0) {
	alert("O telefone é um campo obrigatório !");
        Form.ship_to_phone.focus();
        return false;
     }
     if (Form.ship_to_country.value.length == 0) {
	alert("O pais é um campo obrigatório !");
        Form.ship_to_country.focus();
        return false;
     }
     if (Form.ship_to_email.value.length == 0) {
	alert("O Email é um campo obrigatório !");
        Form.ship_to_email.focus();
        return false;
     }
     if (Form.pagamento.value == "CARTAO") {
         if (Form.nome_cartao.value.length == 0) {
		alert("O nome do cartão é um campo obrigatório !");
	        Form.nome_cartao.focus();
	        return false;
	 }
         if (Form.num_cartao.value.length == 0) {
        	alert("O número do cartão é obrigatório !");
	        Form.num_cartao.focus();
	        return false;
	 }
         if (URL.substring(0,5) != "https") {
		resposta = confirm("Você está prestes a enviar as informações do seu cartão de crédito por um meio inseguro. Para que ele seja seguro a URL deve ser do tipo HTTPS. Você deseja continuar essa operação insegura ?");
		if (resposta == true) 
			return true;
		else
			return false;
	 }

     }      

     return true;
}
function limpa_string(S){
// Deixa so' os digitos no numero
var Digitos = "0123456789";
var temp = "";
var digito = "";
    for (var i=0; i<S.length; i++){
      digito = S.charAt(i);
      if (Digitos.indexOf(digito)>=0){temp=temp+digito}
    }
    return temp
}
function valida_CPF(s)
{
	var i;
	s = limpa_string(s);
	var c = s.substr(0,9);
	var dv = s.substr(9,2);
	var d1 = 0;
	for (i = 0; i < 9; i++)
	{
		d1 += c.charAt(i)*(10-i);
	}
        if (d1 == 0) return false;
	d1 = 11 - (d1 % 11);
	if (d1 > 9) d1 = 0;
	if (dv.charAt(0) != d1)
	{
		return false;
	}

	d1 *= 2;
	for (i = 0; i < 9; i++)
	{
		d1 += c.charAt(i)*(11-i);
	}
	d1 = 11 - (d1 % 11);
	if (d1 > 9) d1 = 0;
	if (dv.charAt(1) != d1)
	{
		return false;
	}
        return true;
}

function valida_CGC(s)
{
	var i;
	s = limpa_string(s);
	var c = s.substr(0,12);
	var dv = s.substr(12,2);
	var d1 = 0;
	for (i = 0; i < 12; i++)
	{
		d1 += c.charAt(11-i)*(2+(i % 8));
	}
        if (d1 == 0) return false;
        d1 = 11 - (d1 % 11);
	if (d1 > 9) d1 = 0;
	if (dv.charAt(0) != d1)
	{
		return false;
	}

	d1 *= 2;
	for (i = 0; i < 12; i++)
	{
		d1 += c.charAt(11-i)*(2+((i+1) % 8));
	}
	d1 = 11 - (d1 % 11);
	if (d1 > 9) d1 = 0;
	if (dv.charAt(1) != d1)
	{
		return false;
	}
        return true;
}


function valida_numeros(s)
{
	var i; 
	var dif = 0;
	for (i = 0; i < s.length; i++)
	{
		var c = s.charAt(i);
		if (!((c >= "0") && (c <= "9")))
		{
			dif = 1;
		}
	}
	if (dif == 1)
	{
		return false;
	}
	return true;
}

//-->
</SCRIPT>
  <br>
  </font>
  <hr>
  <br>
  <p align="CENTER"><input type="SUBMIT" name="submit" value="Gravar Dados">

<p><font color="#FFFFFF">Variavel <%= Session("forma_pagamento") %></font></p>
</form>
</BODY>
</HTML>
