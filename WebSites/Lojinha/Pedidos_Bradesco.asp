<!--#INCLUDE FILE="checa_senha.inc" -->
<!--#INCLUDE FILE="funcoes_uteis.asp" -->
<% 
' Esta página só pode ser acessada se o visitante já se autentificou
' como administrador da loja
checa_senha()

%>

<HTML>
<HEAD>
<TITLE>Lista de pedidos</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>

<BODY BGCOLOR="#FFFFFF">
<%
' Testa se houve erro no processamento de captura
If Request.QueryString("erro") <> "" Then
%>
<p>Houve um erro ao processar a captura deste pedido. Para verificar contate a 
  instituição financeira informando o código do pedido assim como as informações 
  abaixo:</p>
<p>Erro: <b><%= Request.QueryString("erro") %><br>
  </b>Assinatura: <b><%= Request.QueryString("Assinatura") %></b></p>
<%

End If
%>
<h1>Pedidos BradescoNet</h1>
<p><i>Para ver as informações de cada pedido e capturá-lo, clique sobre o link.</i></p>
<table border="1" width="90%">
  <tr>
    <td width="10%"><b>Código</b></td>
    <td width="45%"><b>Nome do Cliente</b></td>
    <td width="15%"><b>Valor</b></td>
    <td width="10%"><b>Capturado</b></td>
    <td width="10%"><div align="RIGHT"><b>Atendido</b></td>
    <td width="10%"><div align="RIGHT"><b>Pago</b></td>
  </tr>
<%
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

Set RS = Server.CreateObject("ADODB.Recordset")
RS.Open "SELECT Pedidos.*, Autorizacoes.status_captura FROM Pedidos, Autorizacoes WHERE Pedidos.codigo_pedido = Autorizacoes.codigo_pedido AND Autorizacoes.status_autorizacao = 0 AND Pedidos.forma_pagamento='BRADESCONET'", Conexao

While Not RS.EOF
%>
  <tr>
    <td width="10%"><a href="mostra_pedido.asp?codigo_pedido=<%= RS("codigo_pedido") %>"><%= RS("codigo_pedido") %></a></td>
    <td width="45%">
      <div align="LEFT">
        <%= RS("nome") %>
      </div>
    </td>
    <td width="15%">
      <div align="RIGHT">
        <%= FormatCurrency(RS("total")) %>
      </div>
    </td>
    <td width="10%">
      <div align="CENTER">
<%
	If RS("status_captura") = 1 Then
		Response.Write "OK"
	End If
%>
      </div>
    </td>
    <td width="10%">
      <div align="RIGHT">
        <% if RS("atendido") = false then
               response.write "Não"
           else
               response.write "Sim"
	   end if 
	%>
      </div>
    </td>
    <td width="10%">
      <div align="RIGHT">
        <% if RS("pago") = false then
               response.write "Não"
           else
               response.write "Sim"
	   end if 
	%>
      </div>
    </td>
  </tr>
<%
        total = total + RS("total")
	RS.MoveNext
Wend
RS.Close
Set RS = Nothing

%>
  <tr>
    <td width="10%"></td>
    <td width="45%"></td>
    <td width="15%">
      <div align="RIGHT"><b>
        <%= FormatCurrency(total) %>
      </b></div></td>
    <td width="10%"></td>
    <td width="10%"></td>
    <td width="10%"></td>
  </tr>
</table>
<h3>Pedidos sem autorização</h3>
<p><i>Os pedidos abaixo não foram autorizados pela instituição financeira por 
  algum motivo. Para ver as informações clique sobre o link.</i></p>
<table border="1" width="75%">
  <tr> 
    <td width="20%"><b>Código</b></td>
    <td width="60%"><b>Nome do Cliente</b></td>
    <td width="20%"><b>Valor</b></td>
  </tr>
  <%
total = 0
Set RS1 = Server.CreateObject("ADODB.Recordset")
RS1.Open "SELECT * FROM Pedidos WHERE forma_pagamento = 'BRADESCONET' AND codigo_pedido NOT IN (SELECT codigo_pedido FROM Autorizacoes WHERE status_autorizacao = 0)", Conexao

While Not RS1.EOF
%> 
  <tr> 
    <td width="20%"><a href="mostra_pedido.asp?codigo_pedido=<%= RS1("codigo_pedido") %>"><%= RS1("codigo_pedido") %></a></td>
    <td width="60%"> 
      <div align="LEFT">
        <%= RS1("nome") %> 
      </div>
    </td>
    <td width="20%"> 
      <div align="RIGHT">
        <%= FormatCurrency(RS1("total")) %> 
      </div>
    </td>
  </tr>
  <%
        total = total + RS1("total")
	RS1.MoveNext
Wend
RS1.Close
Conexao.Close

Set RS1 = Nothing
Set Conexao = Nothing

%> 
  <tr>
    <td width="20%"></td>
    <td width="60%"></td>
    <td width="20%">
      <div align="RIGHT"><b>
        <%= FormatCurrency(total) %>
      </b></div></td>
  </tr>
</table>
<br>
<hr>
<p align="CENTER">
<a href="<%= Application("URL_Admin_Seguro") %>"><i>Voltar para administração da loja</i></a>
<br>
</BODY>
</HTML>
