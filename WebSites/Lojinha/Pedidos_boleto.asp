<!--#INCLUDE FILE="checa_senha.inc" -->
<!--#INCLUDE FILE="funcoes_uteis.asp" -->
<% 
' Esta p�gina s� pode ser acessada se o visitante j� se autentificou
' como administrador da loja
checa_senha()

%>

<HTML>
<HEAD>
<TITLE>Lista de pedidos</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>

<BODY BGCOLOR="#333399" text="#CC0000">

<%
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")
%>
<h1><b><i><font face="Comic Sans MS" color="#FF0000" size="5">Pedidos Boleto Banc�rio</font></i></b></h1>
<p>&nbsp;</p>
<p><i><font face="Comic Sans MS" color="#99CCFF">Para ver as informa��es de cada pedido, clique sobre o link. Confira junto 
  ao seu banco para saber quais os boletos pagos.</font></i></p>
<p align="center"><font face="Comic Sans MS">
<%
' Mostra pedidos que foram feitos atrav�s de Boleto.
Call Mostra_Pedidos(Conexao, "BOLETO")

Conexao.Close
Set Conexao = Nothing

%>
<br>
</font>
<a href="<%= Application("URL_Admin_Seguro") %>"><i><font face="Comic Sans MS" color="#FFFFFF">Voltar para administra��o da loja</font></i></a>
<br>
</BODY>
</HTML>
