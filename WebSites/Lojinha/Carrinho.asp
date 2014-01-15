<%

' Abre conexao com banco de dados
Set Conexao = Server.CreateObject("ADODB.Connection")
Conexao.Open Application("StringConexaoODBC")

' Quando esta pagina é chamada pela primeira vez durante a visita, é criado um novo registro na 
' tabela de pedidos, e este tambem é gravado na variavel de sessao

'pedido_cookie = Request.Cookies(Application("Nome"))("codigo_pedido")
pedido_cookie = session("codigo_pedido")
pedido_loja = session("loja")

If Request.Form = "" Then

	' Pesquisa o codigo de pedido mais alto existente
	Set RS_Max = Server.CreateObject("ADODB.Recordset")
'	RS_Max.CursorType = adOpenKeyset
'	RS_Max.LockType = adLockOptimistic

        ' checa se pedido ja foi concluido, se nao insere na tabela.
	' Isto é necessario por medidas de seguranca. O Cookie é um arquivo texto facilmente
	' modificado e alguem pode tentar usar um pedido que ja existe e foi concluido e assim
        ' iria estraga-lo.
	If pedido_cookie <> "" Then

		RS_Max.Open "SELECT codigo_pedido, forma_pagamento FROM Pedidos WHERE codigo_pedido=" & pedido_cookie, Conexao
		If RS_Max.EOF Then
			' Nao achou. Tem que criar pedido
			pedido_cookie = ""
		Else
			' Ja que pedido nao foi completado podemos usa-lo.
			If IsNull(RS_Max("forma_pagamento")) Then
				Session("codigo_pedido") = pedido_cookie
			' Pedido ja foi usado.
			Else
				pedido_cookie = ""
			End If
		End If
		RS_Max.Close		
	End If

	' cria novo pedido se nao estiver no Cookie ja ou se for verificado que esta pedido
        ' ja foi completado.
	If pedido_cookie = "" Then

		RS_Max.Open "SELECT MAX(codigo_pedido) AS max_codigo_pedido FROM Pedidos", Conexao

		' Acrescenta 1 e grava novo codigo de pedido
		If IsNull(RS_Max("max_codigo_pedido")) Then
			novo_codigo_pedido = 1
		Else
			novo_codigo_pedido = RS_Max("max_codigo_pedido") + 1
		End If
		novo_codigo_pedido = session("codigo_pedido") ' ### inseri

		Conexao.Execute "INSERT INTO Pedidos (codigo_pedido) VALUES (" & novo_codigo_pedido & ")"
		' Grava novo codigo do pedido na sessao para que possa ser lido de outras paginas
		'Session("codigo_pedido") = novo_codigo_pedido ### tirei
		'  Grava novo codigo do pedido em um cookie.
	    Response.Cookies(Application("Nome"))("codigo_pedido") = novo_codigo_pedido
	    Response.Cookies(Application("Nome"))("loja") = pedido_loja
		Response.Cookies(Application("Nome")).Expires = now+10
		Response.Cookies(Application("Nome")).Domain = Application("URLdaLoja")
		RS_Max.Close
	End If

        Set RS_Max = Nothing

End If

' Esta pagina pode ser chamada de dois diferentes locais:
'
' Da pagina de produtos, quando o visitante clica em um produto:
' Nesse caso, deve ser lida a variavel codigo_produto, que e enviada junto com o request da pagina
' pois os links da pagina de produto sao do tipo carrinho.asp?codigo_produto=xxx
' 
' Da propria pagina carrinho.asp quando o visitante resolve mudar a quantidade de um dos itens.
' Nesse caso a quantidade alterada devera estar na variavel de POST

Set RS_Pedido_Item = Server.CreateObject("ADODB.Recordset")

If Request.QueryString("codigo_produto") <> "" Then
	' Chamada foi feita clicando em um item na pagina de produtos

	' Se ja nao esta no pedido, item é incluido na tabela Pedido_Item
	RS_Pedido_Item.Open "SELECT * FROM Pedido_Item WHERE codigo_pedido = " & Session("codigo_pedido") & " AND codigo_produto = " & Request.QueryString("codigo_produto"), Conexao
	
	If RS_Pedido_Item.EOF Then
		' Nao achou. Portanto inclui
		Conexao.Execute "INSERT INTO Pedido_Item (codigo_pedido, codigo_produto, quantidade) VALUES (" & Session("codigo_pedido") & ", " & Request.QueryString("codigo_produto") & ", 1)"
	End If
	num_itens_total = 1
	RS_Pedido_Item.Close
End If

If Request.Form <> "" Then
	' Chamada foi feita atraves do botao atualizar desta pagina
	
	' Regrava todos os itens do pedido de acordo com os campos do formulario;

	' Primeiro apaga tudo para regravar
	Conexao.Execute "DELETE FROM Pedido_Item WHERE codigo_pedido = " & Session("codigo_pedido")

	' Para cada campo do formulario, nome do campo é o código do produto a incluir
	' e o valor do campo é a quantidade escolhida
	For Each field_name In Request.Form
		If field_name <> "B1" And Request.Form(field_name) <> "0" Then
			comandoSQL = "INSERT INTO Pedido_Item (codigo_pedido, codigo_produto, quantidade) VALUES ("
			comandoSQL = comandoSQL & Session("codigo_pedido") & ", " & field_name & ", " & Request.Form(field_name) & ")"
			Conexao.Execute comandoSQL
		End If
	Next
End If
%>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<!-- saved from url=(0038)http://www.e-financial.com.br/main.asp -->
<HTML><HEAD><TITLE>Novaconexao</TITLE>
<META http-equiv=Content-Type content="text/html; charset=windows-1252"><!-- frames -->
<SCRIPT language=JavaScript>
	function MM_goToURL2(url) 
	{ 
		if (url == '1')
			{	
				self.location  = document.frmMain.select.value;
			}
		if (url == '2')
			{
				self.location  = "/main.asp?SessionId=&grafico=&grupo=" + document.frmMain.grupo.value ;
			}
	}
	function SubmitNewsletter()
	{
		if(document.FrmNewsletter.strNome.value == '')
		{
			window.alert('Preencha o nome para o cadastro.');
			window.FrmNewsletter.strNome.focus();
			return false;
		}
		if (document.FrmNewsletter.strMail.value == '')
		{
			window.alert('Preencha o e-mail para o cadastro.');
			window.FrmNewsletter.strMail.focus();
			return false;
		}
		window.FrmNewsletter.action = 'insere_news.asp'
		window.FrmNewsletter.submit();
	
	}
</SCRIPT>

<META content="MSHTML 5.50.4807.2300" name=GENERATOR></HEAD>

<BODY bgColor=#ffffff leftMargin=0 topMargin=0 marginheight="0" marginwidth="0">

<form method="POST" action="carrinho.asp" id=form2 name=form2>

<table width="770" border="0" cellpadding="0" cellspacing="0" align="center">
  <tr valign="top"> 
    <td bgcolor="#FFFFFF" colspan="6"> 
      <p>&nbsp;</p>
    </td>
  </tr>
  <tr valign="middle"> 
    <td bgcolor="#FFFFFF" colspan="6"> 
      <div align="center"> 
        <p><font size="2" face="Verdana, Arial, Helvetica, sans-serif"><b><font color="#999999" size="3" face="Arial, Helvetica, sans-serif"><font face="Verdana, Arial, Helvetica, sans-serif" size="2">Suas 
          compras </font></font></b></font></p>
      </div>
    </td>
  </tr>
  <tr valign="top"> 
    <td bgcolor="#FFFFFF" colspan="6">&nbsp; </td>
  </tr>
  <tr bordercolor="#FFFFFF"> 
    <td valign="middle" height="12" colspan="6"> 
      <table border=1 width="100%">
        <tbody> 
        <tr> 
          <td width="10%"> 
            <div align=center><b><font face="Verdana, Arial, Helvetica, sans-serif" color="#999999" size="2">Código</font></b> 
            </div>
          </td>
          <td width="60%"> 
            <div align=center><b><font face="Verdana, Arial, Helvetica, sans-serif" color="#999999" size="2">Nome 
              do produto</font></b> </div>
          </td>
          <td width="10%"> 
            <div align=center><b><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#999999">Preço 
              unitário</font></b> </div>
          </td>
          <td width="10%"> 
            <div align=center><b><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#999999">Quantidade</font></b> 
            </div>
          </td>
          <td width="10%"> 
            <div align=center><b><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#999999">Total</font></b> 
            </div>
          </td>
        </tr>

<%
RS_Pedido_Item.Open "SELECT Pedido_Item.*, Produtos.nome_produto, Produtos.preco_unitario FROM Pedido_Item, Produtos WHERE codigo_pedido = " & Session("codigo_pedido") & " AND Pedido_Item.codigo_produto = Produtos.codigo_produto", Conexao
subtotal = 0

' Checa se carrinho possui algum item 
If Not RS_Pedido_Item.EOF Then

	intConta = 0
	dim arrCodigo()
	
    RS_Pedido_Item.MoveFirst
    While Not RS_Pedido_Item.EOF
    Redim Preserve arrCodigo(intConta)
    Redim Preserve arrDescricao(intConta)
    Redim Preserve arrQuantidade(intConta)
    Redim Preserve arrValor(intConta)
%>
        <tr> 
          <td width="10%"> 
            <div align="center"><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#000000"><%= RS_Pedido_Item("codigo_produto") %>
            <% arrCodigo(intConta) = RS_Pedido_Item("codigo_produto") %>
              </font></div>
          </td>
          <td width="60%"> 
            <div align=left><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#000000"><%= RS_Pedido_Item("nome_produto") %>
            <% arrDescricao(intConta) = RS_Pedido_Item("nome_produto") %>
              </font></div>
          </td>
          <td width="10%"> 
            <div align=right><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#000000">R<%= FormatCurrency(RS_Pedido_Item("preco_unitario")) %>
			<% arrValor(intConta) = RS_Pedido_Item("preco_unitario") %>            
              </font></div>
          </td>
          <td width="10%"> 
            <div align=right><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#000000"> 
          <input type="TEXT" name="<%= RS_Pedido_Item("codigo_produto") %>" size="2" maxlength="2" value="<%= RS_Pedido_Item("quantidade") %>">
		  <% arrQuantidade(intConta) = RS_Pedido_Item("quantidade") %>
              </font></div>
          </td>
          <td width="10%"> 
            <div align=right><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#000000">R<%= FormatCurrency(cdbl(RS_Pedido_Item("quantidade")) * cdbl(RS_Pedido_Item("preco_unitario")))%>
              </font></div>
          </td>
        </tr>
<%
	    subtotal = subtotal + cdbl(RS_Pedido_Item("quantidade")) * cdbl(RS_Pedido_Item("preco_unitario"))
	    intConta = intConta + 1
	    RS_Pedido_Item.MoveNext
	Wend
end if 

RS_Pedido_Item.Close

' Agora grava e mostra o subtotal
Session("subtotal") = subtotal
	
Conexao.Execute "UPDATE Pedidos SET subtotal = " & subtotal & " WHERE codigo_pedido = " & Session("codigo_pedido")
%>
        <tr> 
          <td width="10%" colspan=4> 
            <div align=right><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#000000">Frete:</font> 
            </div>
          </td>
          <td width="10%"> 
            <div align=right><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#000000">R<%= FormatCurrency(0.00) %>
              </font></div>
          </td>
        </tr>
        <tr> 
          <td width="10%" colspan=4> 
            <div align=right><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#000000">Subtotal:</font> 
            </div>
          </td>
          <td width="10%"> 
            <div align=right><font face="Verdana, Arial, Helvetica, sans-serif" size="2" color="#000000">R<%= FormatCurrency(subtotal + 0.00) %> 
              </font></div>
          </td>
        </tr>
        </tbody> 
      </table>
    </td>
  </tr>
  <tr bordercolor="#FFFFFF"> 
    <td valign="middle" height="26" colspan="6"> 
      <hr color=#cccccc size=1 height="1">
    </td>
  </tr>
  <tr bordercolor="#FFFFFF"> 
    <td valign="middle" width="92" height="30"> 
      <div align="right"><a href="javascript:document.form2.submit();"><img src="images/teclado.gif" width="83" height="109" align="middle" border=0></a></div>
    </td>
    <td valign="bottom" width="117" height="30"> 
      <p><font size="1" face="Verdana, Arial, Helvetica, sans-serif" color="#0099CC">
      <a href="javascript:document.form2.submit();">Clique aqui para atualizar o carrinho</a></font></p>
    </td>
    <td valign="middle" width="172" height="30"> 
      <div align="right"><a href="<%= Application("URL_Categorias") %>"><img src="images/recicla.jpg" width="80" height="38" align="middle" border=0></a></div>
    </td>
    <td valign="bottom" width="111" height="30"> 
      <p><font size="1" face="Verdana, Arial, Helvetica, sans-serif" color="#0099CC">
      <a href="<%= Application("URL_Categorias") %>">Clique aqui para retornar a loja</a></font></p>
    </td>
    <td valign="middle" width="144" height="30"> 
      <div align="right"><a href="javascript:document.form1.submit();"><img src="images/SMARTPAGCENTER.jpg" width="80" height="68" align="middle" border=0></a></div>
    </td>
    <td valign="bottom" width="134" height="30"><font size="1" face="Verdana, Arial, Helvetica, sans-serif" color="#3399CC">
    <a href="javascript:document.form1.submit();">Clique aqui para efetuar o pagamento</a>
    </font></td>
  </tr>
  <tr bordercolor="#FFFFFF"> 
    <td valign="middle" height="26" colspan="6">&nbsp; </td>
  </tr>
  <tr bordercolor="#FFFFFF"> 
    <td valign="middle" height="30" width="92">&nbsp;</td>
    <td valign="middle" height="30" colspan="5"> 
      <div align="right"><font face="Comic Sans MS"> </font> </div>
    </td>
  </tr>
</table>

</form>

<table width="770" border="0" align="center">
  <tr> 
    <td height="26"> 
      <hr color=#cccccc size=1 height="1">
    </td>
  </tr>
  <tr> 
    <td> 
      <div align="center"><font 
      face="Verdana, Arial, Helvetica, sans-serif" size=1><font 
      color=#666666>Nova Conexão</font></font></div>
    </td>
  </tr>
</table>
<div align="center"></div>

<form action="http://localhost/SuperPag/handshakehtml.aspx" method="POST" target="_top" id="form1" name="form1">
	<!-- IDENTIFICACAO DA LOJA -->
<%
Select Case pedido_loja
  'Lojinha para PanAmericano
    Case "pan": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="0ACFB11EFFB047EFFB74D80BE2637C7A9BA5E842">
    	<%
  'loja de Homologação VISA 01
    Case "Loja Visa 01": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12F29A32FAB047EFFB39D80BE2631B4F8FA5D042">
    	<%
  'loja de Homologação VISA 02
    Case "Loja Visa 02": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12F29A32FAB047EFF839D80BE2631B4F8FA5D042">
    	<%
  'loja de Homologação VISA 03
    Case "Loja Visa 03": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12F29A32FAB047EFF939D80BE2631B4F8FA5D042">
    	<%
  'loja de Homologação VISA 04
    Case "Loja Visa 04": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12F29A32FAB047EFFE39D80BE2631B4F8FA5D042">
    	<%
  'loja de Homologação VISA 05
    Case "Loja Visa 05": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12F29A32FAB047EFFF39D80BE2631B4F8FA5D042">
    	<%


  'loja de Homologação VISA 445
    Case "Loja Visa 445": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12FAB047EFFE71DC0BE2637C7A8AADEB7F60BC">
    	<%
  'loja de Homologação VISA 446
    Case "Loja Visa 446": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12FAB047EFFE71DF0BE2637C7A8AADEB7F60BC">
    	<%
  'loja de Homologação VISA 447
    Case "Loja Visa 447": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12FAB047EFFE71DE0BE2637C7A8AADEB7F60BC">
    	<%
  'loja de Homologação VISA 448
    Case "Loja Visa 448": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12FAB047EFFE71D10BE2637C7A8AADEB7F60BC">
    	<%
  'loja de Homologação VISA 449
    Case "Loja Visa 449": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12FAB047EFFE71D00BE2637C7A8AADEB7F60BC">
    	<%
  'loja de Homologação VISA 451
    Case "Loja Visa 451": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12FAB047EFFE70D80BE2637C7A8AADEB7F60BC">
    	<%
  'loja de Homologação VISA 452
    Case "Loja Visa 452": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12FAB047EFFE70DB0BE2637C7A8AADEB7F60BC">
    	<%
  'loja de Homologação VISA 453
    Case "Loja Visa 453": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12FAB047EFFE70DA0BE2637C7A8AADEB7F60BC">
    	<%
  'loja de Homologação VISA 454
    Case "Loja Visa 454": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12FAB047EFFE70DD0BE2637C7A8AADEB7F60BC">
    	<%
  'loja de Homologação VISA 455
    Case "Loja Visa 455": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12FAB047EFFE70DC0BE2637C7A8AADEB7F60BC">
    	<%


  'loja de Homologação TecBan 01
    Case "Loja TecBan 01": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12F8B047EFFC39D80BE26308459ED48942">
    	<%
  'loja de Homologação TecBan 02
    Case "Loja TecBan 02": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12F8B047EFFD39D80BE26308459ED48A42">
    	<%
  'loja de Homologação TecBan 03
    Case "Loja TecBan 03": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12F8B047EFF239D80BE26308459ED48B42">
    	<%
  'loja de Homologação TecBan 04
    Case "Loja TecBan 04": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12F8B047EFF339D80BE26308459ED48C42">
    	<%
  'loja de Homologação TecBan 05
    Case "Loja TecBan 05": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA12F8B047EFFB759546AF2E31439FA6880B54">
    	<%
  'loja padrão com dados pessoais postados
    Case "Lojinha Teste": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="2DF5C4349AB047EFFC769546AF2E316AB8DC8E5954">
    	<%
    Case "Lojinha Remota 1": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="2DF5C4349AB047EFFC769546AF2E316AB8DC8E5954">
    	<%

  'loja de Homologação Bradesco 01
    Case "Loja bradesco 01": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA128CB047EFFC75D10BE2637C7A9EB6F97A6DBC">
    	<%
  'loja de Homologação Bradesco 02
    Case "Loja bradesco 02": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA128CB047EFFC75D00BE2637C7A9EB6F97A6DBC">
    	<%
    	    Session("codigo_pedido") = "TVT" & Session("codigo_pedido")
  'loja de Homologação Bradesco 03
    Case "Loja bradesco 03": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA128CB047EFFC74D90BE2637C7A9EB6F97A6DBC">
    	<%
  'loja de Homologação Bradesco 04
    Case "Loja bradesco 04": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA128CB047EFFC74D80BE2637C7A9EB6F97A6DBC">
    	<%
  'loja de Homologação Bradesco 05
    Case "Loja bradesco 05": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA128CB047EFFC74DB0BE2637C7A9EB6F97A6DBC">
    	<%
  'loja de Homologação Bradesco 06
    Case "Loja bradesco 06": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA128CB047EFFC74DA0BE2637C7A9EB6F97A6DBC">
    	<%
  'loja de Homologação Bradesco 07
    Case "Loja bradesco 07": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA128CB047EFFC74DD0BE2637C7A9EB6F97A6DBC">
    	<%
  'loja de Homologação Bradesco 08
    Case "Loja bradesco 08": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA128CB047EFFC74DC0BE2637C7A9EB6F97A6DBC">
    	<%
  'loja de Homologação Bradesco 09
    Case "Loja bradesco 09": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA128CB047EFFC74DF0BE2637C7A9EB6F97A6DBC">
    	<%
  'loja de Homologação Bradesco 10
    Case "Loja bradesco 10": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="12D2BA128CB047EFFC74DE0BE2637C7A9EB6F97A6DBC">
    	<%
  'loja SuperPag
    Case "Loja SuperPag": 
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="2DF5C4349AB047EFFC769546AF2E316AB8DC8E5955">
    	<%
    	
  'loja Padrão
    Case else:
      %>
    	<input type="hidden" name="5DED746B8F924F2E" value="2DF5C4349AB047EFFC769546AF2E316AB8DC8E5954">
    	<%
    	    	    Session("codigo_pedido") = "TVT" & Session("codigo_pedido")
End Select
%>
	<!-- NUMERO DO PEDIDO -->
	<input type="hidden" name="91D4C3128BF7DA7F" value="<%= Session("codigo_pedido") %>">
</form>

<%
Conexao.Close

Set RS_Pedido_Item = Nothing
Set Conexao = Nothing
%>


</BODY></HTML>


