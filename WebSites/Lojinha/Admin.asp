<!--#INCLUDE FILE="checa_senha.inc" -->
<%
If Request.Form("senha") <> "" Then
	
	senha = Request.Form("senha")
	
	Set DynCrypto = server.CreateObject("DynCrypto.crypto")
	ChavePublica = Trim(DynCrypto.AsymPublicKey(senha))
	
	If ChavePublica <> Application("ChavePublica") Then
		Response.Redirect "senha_admin.htm"
		Response.End
	Else
		Session("ChavePublica") = ChavePublica
		Session("Senha") = senha
	End If
	
	Set DynCrypto = nothing
End If

' Esta página só pode ser acessada se o visitante já se autenticou
' como administrador da loja
checa_senha()
	
%>

<HTML>
<HEAD>
<TITLE>Administra&ccedil;ão da loja</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=">
</HEAD>

<BODY BGCOLOR="#333399" vlink="#800000" link="#000066">
<h1><b><i><font face="Comic Sans MS" color="#FF0000" size="5">Administra&ccedil;ão dos pedidos</font><font face="Comic Sans MS" color="#000066" size="5"><img border="0" src="images/pedido.gif" width="91" height="80"></font></i></b></h1>
<p><i><a href="<%= Application("URL_Pedidos_Bradesco") %>"><font face="Comic Sans MS" color="#FFFFFF">Verificar pedidos BradescoNet</font></a></i></p>
<p><i><a href="<%= Application("URL_Pedidos_Boleto") %>"><font face="Comic Sans MS" color="#FFFFFF">Verificar pedidos Boleto</font></a></i></p>
<p><i><a href="<%= Application("URL_Pedidos_Cartao") %>"><font face="Comic Sans MS" color="#FFFFFF">Verificar pedidos por Cartão de Crédito</font></a></i></p>
<h1><b><i><font face="Comic Sans MS" color="#FF0000" size="5">Administra&ccedil;ão dos produtos&nbsp;</font></i></b></h1>
<p><i><a href="<%= Application("URL_Mostra_categorias") %>"><font face="Comic Sans MS" color="#FFFFFF">Verificar categorias</font></a></i></p>
<p><i><a href="<%= Application("URL_Mostra_produtos") %>"><font face="Comic Sans MS" color="#FFFFFF">Verificar produtos</font></a></i></p>
</BODY>
</HTML>
