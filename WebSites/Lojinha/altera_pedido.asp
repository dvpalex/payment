<%
' Abre conexao com banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

' Le informacoes do pedido
Set RS_Pedido = Server.CreateObject("ADODB.Recordset")

%>

<HTML>
<HEAD>
<TITLE>Pedido apagado</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>

<BODY BGCOLOR="#333399" text="#CC0000" vlink="#800000" link="#000066">

<p align="LEFT"><b><i><font color="#CC0000" face="Comic Sans MS" size="5">Confirma&ccedil;ão</font></i></b></p>

<p align="LEFT">&nbsp;</p>

<%
pedido = Request("codigo_pedido")

' Marca pedido como atendido ou nao
If Request.form("atender") <> "" Then
%>
	<ul>
	<li><b><font face="Comic Sans MS" color="#6699FF">Pedido <%= pedido%> alterado.</font></b></li>
	</ul>
<%
' esta query simplesmente inverte o valor de atendido. Se for verdadeiro sera atualizado para
' falso e assim vice-versa.
	RS_Pedido.Open "UPDATE Pedidos SET atendido = NOT(atendido) WHERE codigo_pedido = " & pedido, Conexao
end if 

' Marca pedido como pago ou nao
If Request.form("pagar") <> "" Then
%>
	<ul>
	<li><b><font face="Comic Sans MS" color="#6699FF">Pedido <%= pedido%> alterado.</font></b></li>
	</ul>
<%
' esta query simplesmente inverte o valor de pago. Se for verdadeiro sera atualizado para
' falso e assim vice-versa.
	RS_Pedido.Open "UPDATE Pedidos SET pago = NOT(pago)  WHERE codigo_pedido = " & pedido, Conexao
end if 

' Apaga pedido 
If Request.form("deletar") <> "" Then
%>
	<ul>
	<li><b><font face="Comic Sans MS" color="#6699FF">Pedido <%= pedido%> removido.</font></b></li>
	</ul>
<p align="CENTER">
&nbsp;
<p align="CENTER">
<a href="<%= Application("URL_Mostra_pedidos_seguro") %>?codigo_pedido=<%= pedido %>" ><i><font face="Comic Sans MS" color="#FFFFFF">Rever o pedido atualizado</font></i></a>
<font face="Comic Sans MS" color="#FFFFFF">
<br>

<%
end if 

Conexao.Close 

Set RS_Pedido = Nothing
Set Conexao = Nothing

%>
</font>
<p align="CENTER">
<a href="<%= Application("URL_Admin_Seguro") %>"><i><font face="Comic Sans MS" color="#FFFFFF">Voltar a administra&ccedil;ão da loja</font></i></a>
<font face="Comic Sans MS" color="#FFFFFF">
<br>
</font>
</BODY>
</HTML>
