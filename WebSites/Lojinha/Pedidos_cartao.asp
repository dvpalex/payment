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

<BODY BGCOLOR="#FFCC66">
<%
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")
%>
<h1><font color="#000066" face="Comic Sans MS" size="5">Pedidos com Cart�o Cr�dito</font></h1>
<p><i><font color="#CC0000" face="Comic Sans MS">Para ver as informa��es de cada pedido, clique sobre o link.</font></i></p>

<font color="#CC0000" face="Comic Sans MS">

<%
' Mostra pedidos que foram feitos atrav�s de Cart�o de Cr�dito.
Call Mostra_Pedidos(Conexao, "CARTAO")

Conexao.Close
Set Conexao = Nothing

%>
<br>
</font>
<p align="CENTER">
<font color="#CC0000" face="Comic Sans MS">
<a href="<%= Application("URL_Admin_Seguro") %>"><i>Voltar para administra��o da loja</i></a>
<br>
</font>
</BODY>
</HTML>
